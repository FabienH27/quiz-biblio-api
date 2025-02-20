using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using QuizBiblio.DataAccess.QbDbContext;
using QuizBiblio.Models.Quiz;
using QuizBiblio.Models.Quiz.Utils;

namespace QuizBiblio.DataAccess.Quiz;

public class QuizRepository : IQuizRepository
{

    private readonly IMongoDbContext _dbContext;

    private readonly FilterDefinitionBuilder<QuizEntity> Filters = Builders<QuizEntity>.Filter;

    private readonly ProjectionDefinition<QuizEntity, QuizDto> _dtoProjection;

    IMongoCollection<QuizEntity> Quizzes => _dbContext.GetCollection<QuizEntity>("Quizzes");

    public QuizRepository(IMongoDbContext dbContext)
    {
        _dbContext = dbContext;
        _dtoProjection = QuizMapper.ProjectTo<QuizEntity, QuizDto>();
    }

    public async Task<List<QuizInfo>> GetQuizzesAsync()
    {
        return await Quizzes.Find(_ => true)
            .Project(q => new QuizInfo
            {
                Id = q.Id,
                Title = q.Title,
                ImageId = q.ImageId,
                Themes = q.Themes,
                CreatorName = q.Creator.Name,
                QuestionCount = q.Questions.Count,
            })
            .ToListAsync();
    }

    public async Task<QuizDto> GetQuiz(string quizId)
    {
        var filter = Filters.Eq(q => q.Id, quizId);
        return await Quizzes.Find(filter)
            .Project(_dtoProjection)
            .FirstOrDefaultAsync();
    }

    public async Task<List<QuizInfo>> GetUserQuizzesAsync(string userId)
    {
        var filter = Filters.Eq(q => q.Creator.Id, userId);

        return await Quizzes.Find(filter)
            .Project(q => new QuizInfo
            {
                Id = q.Id,
                Title = q.Title,
                CreatorName = q.Creator.Name,
                ImageId= q.ImageId,
                Themes = q.Themes,
                QuestionCount = q.Questions.Count
            })
            .ToListAsync();
    }

    public async Task CreateQuiz(QuizEntity quiz)
    {
        await Quizzes.InsertOneAsync(quiz);
    }

    public async Task UpdateQuiz(QuizEntity quizEntity)
    {
        await Quizzes.ReplaceOneAsync(quiz => quiz.Id == quizEntity.Id, quizEntity);
    }
}

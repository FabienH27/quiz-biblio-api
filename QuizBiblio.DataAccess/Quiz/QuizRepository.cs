using MongoDB.Bson;
using MongoDB.Driver;
using QuizBiblio.DataAccess.QbDbContext;
using QuizBiblio.Models.Quiz;

namespace QuizBiblio.DataAccess.Quiz;

public class QuizRepository : IQuizRepository
{

    private readonly IMongoDbContext _dbContext;

    private readonly FilterDefinitionBuilder<QuizEntity> Filters = Builders<QuizEntity>.Filter;

    IMongoCollection<QuizEntity> Quizzes => _dbContext.GetCollection<QuizEntity>("Quizzes");

    public QuizRepository(IMongoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<QuizEntity>> GetQuizzesAsync()
    {
        return await Quizzes.AsQueryable().ToListAsync();
    }

    public async Task<List<QuizInfo>> GetUserQuizzesAsync(string userId)
    {
        var filter = Filters.Eq(q => q.Creator.Id, ObjectId.Parse(userId));

        return await Quizzes.Find(filter)
            .Project(q => new QuizInfo
            {
                Id = q.Id.ToString(),
                Title = q.Title,
                ImageId = q.ImageId,
                Themes = q.Themes,
                CreatorName = q.Creator.Name,
            })            
            .ToListAsync();
    }

    public async Task CreateQuiz(QuizEntity quiz)
    {
        await Quizzes.InsertOneAsync(quiz);
    }

    public async Task UpdateQuiz(QuizEntity quizToUpdate)
    {
        var filter = Filters.Eq(q => q.Id, quizToUpdate.Id);
        var update = Builders<QuizEntity>.Update.Set(quiz => quiz, quizToUpdate);

        await Quizzes.UpdateOneAsync(filter, update);
    }
}

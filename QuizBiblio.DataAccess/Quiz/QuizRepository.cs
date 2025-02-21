using MongoDB.Driver;
using MongoDB.Driver.Linq;
using QuizBiblio.DataAccess.QbDbContext;
using QuizBiblio.DataAccess.Utils;
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

    /// <summary>
    /// Gets all quizzes
    /// </summary>
    /// <returns></returns>
    public async Task<List<QuizInfo>> GetQuizzesAsync()
    {
        return await Quizzes.Find(_ => true)
            .ProjectToInfo()
            .ToListAsync();
    }

    /// <summary>
    /// Gets a specific quiz
    /// </summary>
    /// <param name="quizId"></param>
    /// <returns></returns>
    public async Task<QuizDto> GetQuiz(string quizId)
    {
        var filter = Filters.Eq(q => q.Id, quizId);
        return await Quizzes.Find(filter)
            .ProjectToDto()
            .FirstOrDefaultAsync();
    }

    /// <summary>
    /// Gets quiz for a specific user
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task<List<QuizInfo>> GetUserQuizzesAsync(string userId)
    {
        var filter = Filters.Eq(q => q.Creator.Id, userId);

        return await Quizzes.Find(filter)
            .ProjectToInfo()
            .ToListAsync();
    }

    /// <summary>
    /// Creates a quiz
    /// </summary>
    /// <param name="quizEntity">quiz to create</param>
    /// <returns></returns>
    public async Task CreateQuiz(QuizEntity quizEntity)
    {
        await Quizzes.InsertOneAsync(quizEntity);
    }

    /// <summary>
    /// Updates a quiz
    /// </summary>
    /// <param name="quizEntity">quiz to update</param>
    /// <returns></returns>
    public async Task UpdateQuiz(QuizEntity quizEntity)
    {
        await Quizzes.ReplaceOneAsync(quiz => quiz.Id == quizEntity.Id, quizEntity);
    }
}

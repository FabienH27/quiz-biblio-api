using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using QuizBiblio.DataAccess.QbDbContext;
using QuizBiblio.DataAccess.Utils;
using QuizBiblio.Models.Quiz;

namespace QuizBiblio.DataAccess.Quiz;

public class QuizRepository : IQuizRepository
{

    private readonly IMongoDbContext _dbContext;
    private readonly ILogger<QuizRepository> _logger;

    private readonly FilterDefinitionBuilder<QuizEntity> Filters = Builders<QuizEntity>.Filter;

    IMongoCollection<QuizEntity> Quizzes => _dbContext.GetCollection<QuizEntity>("Quizzes");

    public QuizRepository(IMongoDbContext dbContext, ILogger<QuizRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    /// <summary>
    /// Gets all quizzes
    /// </summary>
    /// <returns></returns>
    public async Task<List<QuizInfo>> GetQuizzesAsync()
    {
        return await Quizzes.Find(_ => true)
            .Limit(10)
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
    public async Task<bool> CreateQuiz(QuizEntity quizEntity)
    {
        try
        {
            await Quizzes.InsertOneAsync(quizEntity);
            return true;
        }catch(MongoException ex)
        {
            _logger.LogError("Could not create quiz : {Message}", ex.Message);
            return false;
        }
    }

    /// <summary>
    /// Updates a quiz
    /// </summary>
    /// <param name="quizEntity">quiz to update</param>
    /// <returns></returns>
    public async Task<bool> UpdateQuiz(QuizEntity quizEntity)
    {
        try
        {
            await Quizzes.ReplaceOneAsync(quiz => quiz.Id == quizEntity.Id, quizEntity);
            return true;
        }catch(MongoException ex)
        {
            _logger.LogError("Could not update quiz : {Message}", ex.Message);
            return false;
        }
    }
}

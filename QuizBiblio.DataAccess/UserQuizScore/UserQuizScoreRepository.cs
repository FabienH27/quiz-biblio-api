using MongoDB.Driver;
using QuizBiblio.DataAccess.QbDbContext;
using QuizBiblio.Models.UserQuiz;

namespace QuizBiblio.DataAccess.UserQuizScore;

public class UserQuizScoreRepository : IUserQuizScoreRepository
{
    private readonly IMongoDbContext _dbContext;

    IMongoCollection<UserQuizScoreEntity> UserQuizScores => _dbContext.GetCollection<UserQuizScoreEntity>("UserQuizScore");
    private readonly FilterDefinitionBuilder<UserQuizScoreEntity> Filters = Builders<UserQuizScoreEntity>.Filter;

    public UserQuizScoreRepository(IMongoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IAsyncCursor<UserQuizScoreEntity>> GetUserQuizScores()
    {
        var sort = Builders<UserQuizScoreEntity>.Sort.Descending("Score");
        return await UserQuizScores.Find(_ => true)
            .Limit(10)
            .Sort(sort)
            .ToCursorAsync();
    }

    public async Task<UserQuizScoreEntity?> GetUserScoreAsync(string userId) {
        var filter = Filters.Eq(scoreEntity => scoreEntity.UserId, userId);

        return await UserQuizScores.Find(filter).FirstOrDefaultAsync();
    }

    public async Task SaveUserScoreAsync(UserQuizScoreEntity userQuizScore)
    {
        var filter = Filters.Eq(scoreEntity => scoreEntity.UserId, userQuizScore.UserId);
        var replaceOptions = new FindOneAndUpdateOptions<UserQuizScoreEntity> { IsUpsert = true };
        var update = Builders<UserQuizScoreEntity>.Update
            .Inc(uqs => uqs.Score, userQuizScore.Score)
            .SetOnInsert(uqs => uqs.UserName, userQuizScore.UserName);

        await UserQuizScores.FindOneAndUpdateAsync(filter, update, replaceOptions);
    }
}

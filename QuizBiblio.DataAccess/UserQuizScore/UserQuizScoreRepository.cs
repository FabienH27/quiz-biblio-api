using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using QuizBiblio.DataAccess.QbDbContext;
using QuizBiblio.Models;
using QuizBiblio.Models.UserQuizScore;

namespace QuizBiblio.DataAccess.UserQuizScore;

public class UserQuizScoreRepository : IUserQuizScoreRepository
{
    private readonly IMongoDbContext _dbContext;

    IMongoCollection<UserQuizScoreEntity> UserQuizScores => _dbContext.GetCollection<UserQuizScoreEntity>("UserQuizScore");
    IMongoCollection<UserEntity> Users => _dbContext.GetCollection<UserEntity>("Users");


    private readonly FilterDefinitionBuilder<UserQuizScoreEntity> Filters = Builders<UserQuizScoreEntity>.Filter;

    public UserQuizScoreRepository(IMongoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IAsyncCursor<UserScoreWithUserEntity>> GetUserQuizScoresAsync()
    {
        return await (from quizScore in UserQuizScores.AsQueryable()
                      join user in Users.AsQueryable()
                      on quizScore.UserId equals user.Id
                      select new UserScoreWithUserEntity
                      {
                          UserId = user.Id,
                          UserName = user.Username,
                          Score = quizScore.Score,
                      })
                      .OrderByDescending(data => data.Score)
                      .Take(10)
                      .ToCursorAsync();
    }

    public async Task<GuestScoreResponse> SaveUserScoreAsync(UserQuizScoreEntity userQuizScore)
    {
        var filter = Filters.Eq(scoreEntity => scoreEntity.UserId, userQuizScore.UserId);
        var replaceOptions = new FindOneAndUpdateOptions<UserQuizScoreEntity> { IsUpsert = true };
        var update = Builders<UserQuizScoreEntity>.Update
            .Inc(uqs => uqs.Score, userQuizScore.Score);

        var result = await UserQuizScores.FindOneAndUpdateAsync(filter, update, replaceOptions);

        return new GuestScoreResponse(result.Score, userQuizScore.Score);
    }

    public async Task<GuestScoreResponse> SaveUserScoreAsync(string userId, int score)
    {
        var filter = Filters.Eq(scoreEntity => scoreEntity.UserId, userId);
        var replaceOptions = new FindOneAndUpdateOptions<UserQuizScoreEntity> { IsUpsert = true };
        var update = Builders<UserQuizScoreEntity>.Update
            .Inc(uqs => uqs.Score, score);

        var result = await UserQuizScores.FindOneAndUpdateAsync(filter, update, replaceOptions);

        //FindOneAndUpdateAsync() returns the previous value somehow
        var previousScore = result?.Score;
        
        var userNewScore = previousScore.HasValue ? previousScore.Value + score : score;

        return new GuestScoreResponse(userNewScore, score);
    }
}

using MongoDB.Driver;
using QuizBiblio.Models.UserQuizScore;

namespace QuizBiblio.DataAccess.UserQuizScore;

public interface IUserQuizScoreRepository
{
    public Task<IAsyncCursor<UserScoreWithUserEntity>> GetUserQuizScoresAsync();

    public Task<GuestScoreResponse> SaveUserScoreAsync(UserQuizScoreEntity userQuizScore);

    public Task<GuestScoreResponse> SaveUserScoreAsync(string userId, int score);
}

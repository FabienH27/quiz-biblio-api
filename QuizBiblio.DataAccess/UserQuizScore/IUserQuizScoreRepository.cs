using MongoDB.Driver;
using QuizBiblio.Models.UserQuiz;
using QuizBiblio.Models.UserQuizScore;

namespace QuizBiblio.DataAccess.UserQuizScore;

public interface IUserQuizScoreRepository
{
    public Task<IAsyncCursor<UserScoreWithUserEntity>> GetUserQuizScoresAsync();

    public Task SaveUserScoreAsync(UserQuizScoreEntity userQuizScore);

    public Task SaveUserScoreAsync(string userId, int score);
}

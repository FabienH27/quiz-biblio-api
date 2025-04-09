using MongoDB.Driver;
using QuizBiblio.Models.UserQuiz;

namespace QuizBiblio.DataAccess.UserQuizScore;

public interface IUserQuizScoreRepository
{
    public Task<IAsyncCursor<UserQuizScoreEntity>> GetUserQuizScores();

    public Task<UserQuizScoreEntity?> GetUserScoreAsync(string userId);

    public Task SaveUserScoreAsync(UserQuizScoreEntity userQuizScore);

    public Task SaveUserScoreAsync(string userId, int score);
}

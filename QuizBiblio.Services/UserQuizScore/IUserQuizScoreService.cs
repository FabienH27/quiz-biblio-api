using QuizBiblio.Models.UserQuiz;
using QuizBiblio.Models.UserQuizScore;

namespace QuizBiblio.Services.UserQuizScore;

public interface IUserQuizScoreService
{
    public Task<List<UserScoreWithUserEntity>> GetUserQuizScores();

    public Task SaveUserScoreAsync(UserQuizScoreEntity userQuizScore);

    public Task SaveUserScoreAsync(string userId, int score);
}

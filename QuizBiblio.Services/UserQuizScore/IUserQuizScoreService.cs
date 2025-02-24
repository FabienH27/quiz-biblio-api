using QuizBiblio.Models.UserQuiz;

namespace QuizBiblio.Services.UserQuizScore;

public interface IUserQuizScoreService
{
    public Task<List<UserQuizScoreEntity>> GetUserQuizScores();

    public Task<UserQuizScoreEntity?> GetUserScoreAsync(string userId);

    public Task SaveUserScoreAsync(UserQuizScoreEntity userQuizScore);
}

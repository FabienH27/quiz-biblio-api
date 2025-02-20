using QuizBiblio.Models.UserQuiz;

namespace QuizBiblio.DataAccess.UserQuizScore;

public interface IUserQuizScoreRepository
{
    public Task<UserQuizScoreEntity?> GetUserScoreAsync(string userId);

    public Task SaveUserScoreAsync(UserQuizScoreEntity userQuizScore);
}

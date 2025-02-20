using QuizBiblio.DataAccess.UserQuizScore;
using QuizBiblio.Models.UserQuiz;

namespace QuizBiblio.Services.UserQuizScore;

public class UserQuizScoreService(IUserQuizScoreRepository scoreRepository) : IUserQuizScoreService
{

    public async Task<UserQuizScoreEntity?> GetUserScoreAsync(string userId) => await scoreRepository.GetUserScoreAsync(userId);

    public async Task SaveUserScoreAsync(UserQuizScoreEntity userQuizScore)
    {
        await scoreRepository.SaveUserScoreAsync(userQuizScore);
    }
}

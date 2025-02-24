using QuizBiblio.DataAccess.UserQuizScore;
using QuizBiblio.Models.UserQuiz;

namespace QuizBiblio.Services.UserQuizScore;

public class UserQuizScoreService(IUserQuizScoreRepository scoreRepository) : IUserQuizScoreService
{
    public async Task<List<UserQuizScoreEntity>> GetUserQuizScores()
    {
        var scores = new List<UserQuizScoreEntity>();

        using var cursor = await scoreRepository.GetUserQuizScores();

        while (await cursor.MoveNextAsync())
        {
            scores.AddRange(cursor.Current);
        }
        return scores;
    }

    public async Task<UserQuizScoreEntity?> GetUserScoreAsync(string userId) => await scoreRepository.GetUserScoreAsync(userId);

    public async Task SaveUserScoreAsync(UserQuizScoreEntity userQuizScore)
    {
        await scoreRepository.SaveUserScoreAsync(userQuizScore);
    }
}

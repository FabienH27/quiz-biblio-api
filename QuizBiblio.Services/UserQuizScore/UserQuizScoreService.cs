using QuizBiblio.DataAccess.UserQuizScore;
using QuizBiblio.Models.UserQuiz;

namespace QuizBiblio.Services.UserQuizScore;

public class UserQuizScoreService: IUserQuizScoreService
{
    private readonly IUserQuizScoreRepository _scoreRepository;

    public UserQuizScoreService(IUserQuizScoreRepository scoreRepository)
    {
        _scoreRepository = scoreRepository;
    }

    public async Task<List<UserQuizScoreEntity>> GetUserQuizScores()
    {
        var scores = new List<UserQuizScoreEntity>();

        using var cursor = await _scoreRepository.GetUserQuizScores();

        while (await cursor.MoveNextAsync())
        {
            scores.AddRange(cursor.Current);
        }
        return scores;
    }

    public async Task<UserQuizScoreEntity?> GetUserScoreAsync(string userId) => await _scoreRepository.GetUserScoreAsync(userId);

    public async Task SaveUserScoreAsync(UserQuizScoreEntity userQuizScore)
    {
        await _scoreRepository.SaveUserScoreAsync(userQuizScore);
    }

    public async Task SaveUserScoreAsync(string userId, int score)
    {
        await _scoreRepository.SaveUserScoreAsync(userId, score);
    }

}

using QuizBiblio.DataAccess.UserQuizScore;
using QuizBiblio.Models.UserQuiz;
using QuizBiblio.Models.UserQuizScore;

namespace QuizBiblio.Services.UserQuizScore;

public class UserQuizScoreService: IUserQuizScoreService
{
    private readonly IUserQuizScoreRepository _scoreRepository;

    public UserQuizScoreService(IUserQuizScoreRepository scoreRepository)
    {
        _scoreRepository = scoreRepository;
    }

    public async Task<List<UserScoreWithUserEntity>> GetUserQuizScores()
    {
        var scores = new List<UserScoreWithUserEntity>();

        using var cursor = await _scoreRepository.GetUserQuizScoresAsync();

        while (await cursor.MoveNextAsync())
        {
            scores.AddRange(cursor.Current);
        }
        return scores;
    }

    public async Task SaveUserScoreAsync(UserQuizScoreEntity userQuizScore)
    {
        await _scoreRepository.SaveUserScoreAsync(userQuizScore);
    }

    public async Task SaveUserScoreAsync(string userId, int score)
    {
        await _scoreRepository.SaveUserScoreAsync(userId, score);
    }

}

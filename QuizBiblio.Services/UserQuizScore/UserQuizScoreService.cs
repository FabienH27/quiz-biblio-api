using QuizBiblio.DataAccess.UserQuizScore;
using QuizBiblio.Models.UserQuizScore;
using QuizBiblio.Services.Utils;

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

    /// <summary>
    /// Saves score for a given user based on correct answers count
    /// </summary>
    /// <param name="userId">id of the user to update</param>
    /// <param name="answers">user answers</param>
    /// <returns></returns>
    public async Task<GuestScoreResponse> SaveUserScoreAsync(string userId, IEnumerable<AnswerDto> answers)
    {
        var guestScore = ScoreHelper.CalculateUserScore(answers);

        return await _scoreRepository.SaveUserScoreAsync(userId, guestScore);
    }

    /// <summary>
    /// Saves new score to existing user score
    /// </summary>
    /// <param name="userId">id of the user</param>
    /// <param name="newScore">new score to combine</param>
    /// <returns></returns>
    public async Task<GuestScoreResponse> SaveUserScoreAsync(string userId, int newScore)
    {
        return await _scoreRepository.SaveUserScoreAsync(userId, newScore);
    }

}

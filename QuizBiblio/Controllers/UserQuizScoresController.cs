using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizBiblio.Helper;
using QuizBiblio.Models.UserQuiz;
using QuizBiblio.Models.UserQuizScore;
using QuizBiblio.Services.UserQuizScore;

namespace QuizBiblio.Controllers;

[ApiController]
[Route("api/scores")]
[Authorize]
public class UserQuizScoresController(IUserQuizScoreService userScoreService) : ControllerBase
{
    /// <summary>
    /// Gets score for a user
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:length(24)}")]
    public async Task<UserQuizScoreEntity?> GetUserScore(string id)
    {
        return await userScoreService.GetUserScoreAsync(id);
    }

    [HttpGet()]
    public async Task<List<UserQuizScoreEntity>> GetScores()
    {
        return await userScoreService.GetUserQuizScores();
    }

    /// <summary>
    /// Saves score for a user
    /// </summary>
    /// <param name="userScore">score to increment for this user</param>
    /// <returns></returns>
    [HttpPost]
    public async Task SaveUserScore([FromBody] UserScoreRequest userScore)
    {
        var userQuizScore = new UserQuizScoreEntity
        {
            UserId = User.Claims.GetUserId(),
            UserName = User.Claims.GetUserName(),
            Score = userScore.UserScore
        };

        await userScoreService.SaveUserScoreAsync(userQuizScore);
    }
}

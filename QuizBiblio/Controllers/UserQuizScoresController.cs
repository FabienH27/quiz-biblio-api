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

    [HttpGet()]
    public async Task<List<UserScoreWithUserEntity>> GetScores()
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
            Score = userScore.UserScore
        };

        await userScoreService.SaveUserScoreAsync(userQuizScore);
    }
}

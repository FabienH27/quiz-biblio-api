using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizBiblio.ClaimsHelper;
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
}

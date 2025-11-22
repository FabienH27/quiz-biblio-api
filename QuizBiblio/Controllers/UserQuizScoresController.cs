using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizBiblio.Models.UserQuizScore;
using QuizBiblio.Services.UserQuizScore;

namespace QuizBiblio.Controllers;

[ApiController]
[Route("api/scores")]
[Authorize]
public class UserQuizScoresController(IUserQuizScoreService userScoreService) : ControllerBase
{

    [HttpGet()]
    public async Task<List<UserScoreWithUserEntity>> GetScores([FromQuery] int pageSize = 10)
    {
        return await userScoreService.GetUserQuizScores(pageSize);
    }
}

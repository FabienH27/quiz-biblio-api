using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizBiblio.Helper;
using QuizBiblio.Models.UserQuizScore;
using QuizBiblio.Services.Guest;
using QuizBiblio.Services.UserQuizScore;
using QuizBiblio.Services.Utils;

namespace QuizBiblio.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuizPlayController : ControllerBase
{
    private readonly IGuestSessionService _guestSessionService;
    private readonly IUserQuizScoreService _userQuizScoreService;

    public QuizPlayController(IGuestSessionService guestSessionService, IUserQuizScoreService userQuizScoreService)
    {
        _guestSessionService = guestSessionService;
        _userQuizScoreService = userQuizScoreService;
    }

    [HttpPost("submit-answers")]
    public async Task<IActionResult> SubmitAnswers([FromBody] QuizAnswersRequest answersRequest)
    {
        var quizAnswers = new QuizAnswerDto
        {
            QuizId = answersRequest.QuizId,
            Answers = answersRequest.Answers.Select(x => new AnswerDto
            {
                QuestionId = x.QuestionId,
                Answers = x.Answers,
                IsCorrect = x.IsCorrect,
            })
        };

        if (User.Identity?.IsAuthenticated == true)
        {
            var userScore = ScoreHelper.CalculateUserScore(quizAnswers.Answers);

            await _userQuizScoreService.SaveUserScoreAsync(User.Claims.GetUserId(), userScore);
        }
        else
        {
            var guestId = Request.Cookies["guestId"];

            if (guestId != null)
            {
                await _guestSessionService.SaveUserAnswersAsync(guestId, quizAnswers);
            }
            else
            {
                return BadRequest();
            }
        }

        return Ok();
    }

    [HttpPost("merge-guest")]
    [Authorize]
    public async Task<IActionResult> MergeToUser()
    {
        var guestId = Request.Cookies["guestId"];
        var userId = User.Claims.GetUserId();

        if (guestId != null)
        {
            return Ok(await _guestSessionService.MergeToUserAsync(guestId, userId));
        }
        else
        {
            return BadRequest();
        }
    }
}

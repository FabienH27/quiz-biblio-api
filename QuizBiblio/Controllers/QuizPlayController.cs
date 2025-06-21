using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizBiblio.ClaimsHelper;
using QuizBiblio.Models.UserQuizScore;
using QuizBiblio.Services.Guest;
using QuizBiblio.Services.UserQuizScore;

namespace QuizBiblio.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuizPlayController : ControllerBase
{
    private readonly IGuestSessionService _guestSessionService;
    private readonly IUserQuizScoreService _userQuizScoreService;
    private readonly IMapper _mapper;

    public QuizPlayController(IGuestSessionService guestSessionService, IUserQuizScoreService userQuizScoreService, IMapper mapper)
    {
        _guestSessionService = guestSessionService;
        _userQuizScoreService = userQuizScoreService;
        _mapper = mapper;
    }

    [HttpPost("submit-answers")]
    public async Task<ActionResult<GuestScoreResponse>> SubmitAnswers([FromBody] QuizAnswersRequest answersRequest)
    {
        var quizAnswers = _mapper.Map<QuizAnswerDto>(answersRequest);

        if (User.Identity?.IsAuthenticated == true)
        {
            var result = await _userQuizScoreService.SaveUserScoreAsync(User.Claims.GetUserId(), quizAnswers.Answers);
            return Ok(result);
        }
        else
        {
            var guestId = Request.Cookies["guestId"];

            if (guestId != null)
            {
                await _guestSessionService.SaveGuestAnswersAsync(guestId, quizAnswers.Answers);

                return Ok();
            }
        }
        return BadRequest();
    }

    [HttpPost("merge-guest")]
    [Authorize]
    public async Task<ActionResult<GuestScoreResponse>> MergeToUser()
    {
        var guestId = Request.Cookies["guestId"];
        var userId = User.Claims.GetUserId();

        if (guestId != null)
        {
            var result = await _guestSessionService.MergeToUserAsync(guestId, userId);
            return Ok(result);
        }
        else
        {
            return BadRequest();
        }
    }
}

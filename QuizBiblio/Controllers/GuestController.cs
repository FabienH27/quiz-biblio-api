using Microsoft.AspNetCore.Mvc;
using QuizBiblio.Services.Guest;

namespace QuizBiblio.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GuestController : ControllerBase
{
    private readonly IGuestSessionService _guestSessionService;
    private readonly ILogger<GuestController> _logger;

    public GuestController(IGuestSessionService guestSessionService, ILogger<GuestController> logger)
    {
        _guestSessionService = guestSessionService;
        _logger = logger;
    }

    [HttpPost("init-session")]
    public async Task<IActionResult> StartGuestSession([FromQuery] string userName)
    {
        var guestSessionId = await _guestSessionService.StartGuestSessionAsync(userName);

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            MaxAge = TimeSpan.FromDays(1),
            Path = "/"
        };

        Response.Cookies.Append("guestId", guestSessionId, cookieOptions);

        return Ok();
    }

    [HttpDelete("end-session")]
    public async Task<IActionResult> DeleteGuestSession()
    {
        var guestId = Request.Cookies["guestId"];

        if(!string.IsNullOrEmpty(guestId))
        {
            Response.Cookies.Delete("guestId");

            await _guestSessionService.DeleteGuestSessionAsync(guestId);
        }

        return Ok();
    }
}

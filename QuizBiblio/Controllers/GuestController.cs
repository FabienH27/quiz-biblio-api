using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using QuizBiblio.Services.Guest;

namespace QuizBiblio.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GuestController : ControllerBase
{
    private readonly IGuestSessionService _guestSessionService;
    private readonly ILogger<GuestController> _logger;
    private readonly CookieSettings _cookieSettings;

    public GuestController(IGuestSessionService guestSessionService, ILogger<GuestController> logger, IOptions<CookieSettings> options)
    {
        _guestSessionService = guestSessionService;
        _logger = logger;
        _cookieSettings = options.Value;
    }

    [HttpPost("init-session")]
    public async Task<IActionResult> StartGuestSession([FromQuery] string userName)
    {
        var guestSessionId = await _guestSessionService.StartGuestSessionAsync(userName);

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = _cookieSettings.Secure,
            SameSite = _cookieSettings.SameSite switch
            {
                "Lax" => SameSiteMode.Lax,
                "Strict" => SameSiteMode.Strict,
                _ => SameSiteMode.None
            },
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

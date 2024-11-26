using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using QuizBiblio.Models;
using QuizBiblio.Services.User;
using QuizBiblio.Services.User.Helper;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace QuizBiblio.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly JwtSettings _jwtSettings;

    private readonly string cookieName = "AuthToken";

    public AuthController(IUserService userService, IOptions<JwtSettings> jwtSettings)
    {
        _userService = userService;
        _jwtSettings = jwtSettings.Value;
        cookieName = jwtSettings.Value.CookieName;
    }

    [HttpPost("register")]
    public void Register([FromBody] RegisterRequest request)
    {
        var user = new User
        {
            Username = request.Username,
            Password = PasswordHelper.HashPassword(request.Password),
            Email = request.Email
        };

        _userService.Create(user);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await _userService.GetUser(request.Email);

        if (user != null || PasswordHelper.VerifyPassword(request.Password, user?.Password ?? ""))
        {
            var token = GenerateJwtToken(user?.Id.ToString() ?? "", user?.Username ?? "");

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiresInMinutes),
                SameSite = SameSiteMode.None,
                Path = "/",
            };
            Response.Cookies.Append(cookieName, token, cookieOptions);

            return Ok(new{ Token = token });
        }

        return Unauthorized("Invalid username or password");
    }

    [Authorize]
    [HttpGet("user-info")]
    public IActionResult GetUserInfo()
    {
        var user = HttpContext.User;
        if (user.Identity?.IsAuthenticated == true)
        {
            var userName = user.FindFirst(ClaimTypes.Name)?.Value;
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return Ok(new
            {
                userName,
                userId
            });
        }

        return Unauthorized();
    }

    [Authorize]
    [HttpPost("logout")]
    public IActionResult Logout()
    {
        Response.Cookies.Append(cookieName, "", new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = DateTimeOffset.UtcNow.AddDays(-1),
        });

        return Ok(new { message = "Logged out successfully" });
    }

    private string GenerateJwtToken(string userId, string name)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

        // Claims included in the token
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, userId),
            new Claim(ClaimTypes.Name, name)
        };

        // Create the token
        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(_jwtSettings.ExpiresInMinutes),
            signingCredentials: credentials
        );

        // Return the token as a string
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

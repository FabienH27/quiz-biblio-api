using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using QuizBiblio.Models;
using QuizBiblio.Models.Auth;
using QuizBiblio.Models.Settings;
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

    private CookieOptions _cookieOptions;

    public AuthController(IUserService userService, IOptions<JwtSettings> jwtSettings)
    {
        _userService = userService;
        _jwtSettings = jwtSettings.Value;
        cookieName = _jwtSettings.CookieName;

        _cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiresInMinutes),
            SameSite = SameSiteMode.None,
            Path = "/",
        };
    }

    /// <summary>
    /// Creates user account
    /// </summary>
    /// <param name="request"></param>
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var user = new UserEntity
        {
            Username = request.Username,
            Password = PasswordHelper.HashPassword(request.Password),
            Email = request.Email,
        };

        var token = GenerateJwtToken(user.Id.ToString(), user.Username, user.Password);

        await _userService.CreateAsync(user);

        Response.Cookies.Append(cookieName, token, _cookieOptions);

        return Ok(new { Message = "Successfully logged in" });
    }

    /// <summary>
    /// Authenticates the user
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await _userService.GetUserFromMail(request.Email);

        if (user != null && PasswordHelper.VerifyPassword(request.Password, user?.Password ?? ""))
        {
            var token = GenerateJwtToken(user?.Id.ToString() ?? "", user?.Username ?? "", user?.Role ?? "");

            Response.Cookies.Append(cookieName, token, _cookieOptions);

            return Ok(new{ Message = "Successfully logged in" });
        }

        return Unauthorized("Invalid username or password");
    }

    /// <summary>
    /// Get user information
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpGet("user-info")]
    public IActionResult GetUserInfo()
    {
        var user = HttpContext.User;
        if (user.Identity?.IsAuthenticated == true)
        {
            var userName = user.FindFirstValue(ClaimTypes.Name);
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            var role = user.FindFirstValue(ClaimTypes.Role);

            return Ok(new
            {
                userName,
                userId,
                role
            });
        }

        return Unauthorized();
    }

    /// <summary>
    /// disconnect the user
    /// </summary>
    /// <returns></returns>
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


    /// <summary>
    /// Checks if the user is authenticated or not
    /// </summary>
    /// <returns>401 if not authenticated, "authenticated" flag to true otherwise</returns>
    [HttpGet("status")]
    public IActionResult IsAuthenticated()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            return Ok(new { authenticated = true });
        }
        return Unauthorized();
    }

    /// <summary>
    /// Generate a new jwt token from parameters
    /// </summary>
    /// <param name="userId">id of the user</param>
    /// <param name="name">user name</param>
    /// <param name="role">user role</param>
    /// <returns></returns>
    private string GenerateJwtToken(string userId, string name, string role)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

        // Claims included in the token
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Role, role),
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

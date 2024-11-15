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

    public AuthController(IUserService userService, IOptions<JwtSettings> jwtSettings)
    {
        _userService = userService;
        _jwtSettings = jwtSettings.Value;
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

        if (user == null || !PasswordHelper.VerifyPassword(request.Password, user.Password))
        {
            return Unauthorized("Invalid username or password");
        }

        var token = GenerateJwtToken(user.Id.ToString(), user.Username);
        
        return Ok(new { Token = token });
    }

    private string GenerateJwtToken(string userId, string userName)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        // Claims included in the token
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("userId", userId),
            new Claim("userName", userName)
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

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizBiblio.Models;
using QuizBiblio.Services.User;
using System.Security.Claims;

namespace QuizBiblio.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "ADMIN")]
public class UsersController(IUserService userService) : ControllerBase
{
    /// <summary>
    /// Gets users
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public Task<List<UserEntity>> GetUsersAsync() => userService.GetUsersAsync();

    /// <summary>
    /// Get a specific user
    /// </summary>
    /// <param name="id">id of the user to get</param>
    /// <returns></returns>
    [HttpGet("{id:length(24)}")]
    public Task<UserEntity?> GetUser(string id) => userService.GetUserAsync(id);

    /// <summary>
    /// Get role from authenticated user
    /// </summary>
    /// <returns></returns>
    [HttpGet("role")]
    [Authorize]
    public IActionResult GetUserRole()
    {
        var claims = HttpContext.User.Identities.First().Claims;
        var role = claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).First();
        return Ok(new { role });
    }

    /// <summary>
    /// Creates a user
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<UserEntity>> Create(UserEntity user)
    {
        await userService.CreateAsync(user);

        return user;
    }


    [HttpPatch("grant-access")]
    public async Task<IActionResult> GrantAccess(string userId)
    {
        await userService.GrantAccess(userId);

        return Ok(new { message = "Access granted to user"});
    }
}

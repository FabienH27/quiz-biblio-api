using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizBiblio.Models;
using QuizBiblio.Services.User;
using System.Security.Claims;

namespace QuizBiblio.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController(IUserService userService) : ControllerBase
{

    /// <summary>
    /// Gets users
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public Task<List<UserEntity>> GetUsers()
    {
        return userService.GetUsers();
    }

    /// <summary>
    /// Get a specific user
    /// </summary>
    /// <param name="id">id of the user to get</param>
    /// <returns></returns>
    [HttpGet("{id:length(24)}")]
    public Task<UserEntity?> GetUser(string id)
    {
        return userService.GetUser(id);
    }

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
    public ActionResult<UserEntity> Create(UserEntity user)
    {
        userService.Create(user);

        return user;
    }
}

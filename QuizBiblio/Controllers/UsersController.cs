using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizBiblio.Models.Users;
using QuizBiblio.Services.Users;
using System.Security.Claims;

namespace QuizBiblio.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "ADMIN")]
public class UsersController(IUserService userService, IMapper mapper) : ControllerBase
{
    /// <summary>
    /// Get users
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<List<UserDto>> GetUsers()
    {
        var users = await userService.GetUsers();
        return mapper.Map<List<UserDto>>(users);
    }

    /// <summary>
    /// Get a specific user
    /// </summary>
    /// <param name="id">id of the user to get</param>
    /// <returns></returns>
    [HttpGet("{id:length(24)}")]
    public async Task<UserDto?> GetUser(string id)
    {
        return mapper.Map<UserDto?>(await userService.GetUser(id));
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
        userService.CreateAsync(user);

        return user;
    }
}

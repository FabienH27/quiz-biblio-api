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

    [HttpGet]
    public Task<List<User>> GetUsers()
    {
        return userService.GetUsers();
    }

    [HttpGet("{id:length(24)}")]
    public Task<User?> GetUser(string id)
    {
        return userService.GetUser(id);
    }


    [HttpGet("role")]
    [Authorize]
    public IActionResult GetUserRole()
    {
        var claims = HttpContext.User.Identities.First().Claims;
        var role = claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).First();
        return Ok(new { role });
    }

    [HttpPost]
    public ActionResult<User> Create(User user)
    {
        userService.Create(user);

        return user;
    }
}

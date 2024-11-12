using Microsoft.AspNetCore.Mvc;
using QuizBiblio.Models;
using QuizBiblio.Services.User;

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

    [HttpPost]
    public ActionResult<User> Create(User user)
    {
        userService.Create(user);

        return user;
    }
}

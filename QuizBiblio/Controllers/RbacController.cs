using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using QuizBiblio.Models;
using QuizBiblio.Models.Rbac;

namespace QuizBiblio.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RbacController(IOptions<List<Role>> roles) : ControllerBase
{

    private readonly List<Role> _roles = roles.Value;

    [HttpGet()]
    public IActionResult GetRoles()
    {
        return Ok(new
        {
            Roles = _roles
        });
    }

}

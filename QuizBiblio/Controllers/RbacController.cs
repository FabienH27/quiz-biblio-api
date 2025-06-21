using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using QuizBiblio.Models.Rbac;

namespace QuizBiblio.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "ADMIN")]
public class RbacController(IOptions<List<Role>> roles) : ControllerBase
{
    private readonly List<Role> _roles = roles.Value;

    /// <summary>
    /// Gets roles
    /// </summary>
    /// <returns></returns>
    [HttpGet("roles")]
    public IActionResult GetRoles()
    {
        return Ok(new
        {
            Roles = _roles
        });
    }

}

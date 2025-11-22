using QuizBiblio.Models.Rbac;

namespace QuizBiblio.Models.Users;

public class UserDto
{
    public required string Id { get; set; }

    public required string Email { get; set; }

    public required string UserName { get; set; }

    public string Role { get; set; } = RbacRole.Member.Name;
}
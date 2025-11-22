using QuizBiblio.Models.Rbac;

namespace QuizBiblio.Models.Users;

public class User
{
    public required string Id { get; set; }

    public required string Email { get; set; }
    public required string Password { get; set; }

    public required string Username { get; set; }

    public string Role { get; set; } = RbacRole.Member.Name;
}
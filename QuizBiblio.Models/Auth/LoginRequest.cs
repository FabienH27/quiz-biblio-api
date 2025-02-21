namespace QuizBiblio.Models.Auth;

public class LoginRequest
{
    public required string Password { get; set; }
    public required string Email { get; set; }
}
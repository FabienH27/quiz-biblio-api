namespace QuizBiblio.Models.Settings;
public class JwtSettings
{
    public string Secret { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public int ExpiresInMinutes { get; set; }

    public string CookieName { get; set; } = "AuthToken";
}

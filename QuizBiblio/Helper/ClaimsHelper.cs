using System.Security.Claims;

namespace QuizBiblio.Helper;

public static class ClaimsHelper
{
    public static string GetUserId(this IEnumerable<Claim> claims)
    {
        return claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
    }

    public static string GetUserName(this IEnumerable<Claim> claims)
    {
        return claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value ?? string.Empty;
    }
}

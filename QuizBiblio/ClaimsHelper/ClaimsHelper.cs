using System.Security.Claims;

namespace QuizBiblio.ClaimsHelper;

public static class ClaimsHelper
{
    /// <summary>
    /// Get user id from claims
    /// </summary>
    /// <param name="claims"></param>
    /// <returns></returns>
    public static string GetUserId(this IEnumerable<Claim> claims)
    {
        return claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
    }

    /// <summary>
    /// Get user name from claims 
    /// </summary>
    /// <param name="claims"></param>
    /// <returns></returns>
    public static string GetUserName(this IEnumerable<Claim> claims)
    {
        return claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value ?? string.Empty;
    }
}

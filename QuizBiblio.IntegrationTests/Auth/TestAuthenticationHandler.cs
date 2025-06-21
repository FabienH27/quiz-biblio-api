using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace QuizBiblio.IntegrationTests.Auth;

public class TestAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public TestAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder) : base(options, logger, encoder)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (Request.Headers.TryGetValue("SkipAuth", out var _))
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        var claims = new List<Claim>();

        if (Request.Headers.TryGetValue("role", out var roles))
            claims.Add(new Claim(ClaimTypes.Role, roles[0]!));

        var identity = new ClaimsIdentity(claims, TestAuthenticationSchemeProvider.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, TestAuthenticationSchemeProvider.Name);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }

}

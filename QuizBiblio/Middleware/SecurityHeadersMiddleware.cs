namespace QuizBiblio.Middleware;

public class SecurityHeadersMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context)
    {

        context.Response.Headers["Content-Security-Policy"] = "default-src 'none';";
        context.Response.Headers["X-Frame-Options"] = "DENY";
        context.Response.Headers["Permissions-Policy"] = "geolocation=(), microphone=(), camera=()";
        context.Response.Headers["X-Content-Type-Options"] = "nosniff";
        context.Response.Headers["Referrer-Policy"] = "strict-origin-when-cross-origin";

        // Passer au middleware suivant dans la chaîne
        await _next(context);
    }

}

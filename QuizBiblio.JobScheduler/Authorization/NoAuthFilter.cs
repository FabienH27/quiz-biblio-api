using Hangfire.Annotations;
using Hangfire.Dashboard;

namespace QuizBiblio.JobScheduler.Authorization;

public class NoAuthFilter : IDashboardAuthorizationFilter
{
    public bool Authorize([NotNull] DashboardContext context)
    {
        return true;
    }
}

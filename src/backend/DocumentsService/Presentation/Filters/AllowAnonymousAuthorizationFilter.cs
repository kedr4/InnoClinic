using Hangfire.Dashboard;

namespace Presentation.Filters;

public class AllowAnonymousAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context) => true;
}

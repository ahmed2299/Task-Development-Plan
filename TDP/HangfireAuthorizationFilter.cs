using Hangfire.Dashboard;

namespace TDP.Web
{
    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            // In production, you should implement proper authorization
            // For now, allow access in development
            return true;
        }
    }
} 
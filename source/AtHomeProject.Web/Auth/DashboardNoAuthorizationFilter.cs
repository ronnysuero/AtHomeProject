using System.Diagnostics.CodeAnalysis;
using Hangfire.Dashboard;

namespace AtHomeProject.Web.Auth
{
    [ExcludeFromCodeCoverage]
    public class DashboardNoAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context) => true;
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Hangfire.Dashboard;

namespace TaskSchedulingWithHangfire
{
    public class HangfireDashboardAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize([NotNull] DashboardContext context)
        {
            var httpContext = context.GetHttpContext();
            var userRole = httpContext.User.FindFirst(ClaimTypes.Role)?.Value;
            //return userRole == "roleNameOrId";  
            return true;
        }
    }
}

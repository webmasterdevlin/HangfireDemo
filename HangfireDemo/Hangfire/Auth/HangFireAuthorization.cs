using Hangfire.Dashboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace HangfireDemo.Hangfire.Auth
{
    public class HangFireAuthorization : IDashboardAuthorizationFilter
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IHttpContextAccessor _hContextAccessor;

        public HangFireAuthorization(IAuthorizationService authorizationService, IHttpContextAccessor hContextAccessor)
        {
            _authorizationService = authorizationService;
            _hContextAccessor = hContextAccessor;
        }

        public bool Authorize(DashboardContext context)
        {
            return true;
        }
    }
}
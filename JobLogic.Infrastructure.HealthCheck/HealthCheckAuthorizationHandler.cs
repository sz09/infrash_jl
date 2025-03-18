using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace JobLogic.Infrastructure.HealthCheck
{
    public class HealthCheckAuthorizationHandler : AuthorizationHandler<HealthCheckRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HealthCheckAuthorizationHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HealthCheckRequirement requirement)
        {
            var httpContext = _httpContextAccessor.HttpContext;

            var agent = httpContext.Request.Headers["User-Agent"];
            var apikey = httpContext.Request.Query["apikey"];
            if (agent == requirement.UserAgent && requirement.ApiKey == apikey)
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }
            return Task.CompletedTask;
        }
    }
}

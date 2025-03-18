using Microsoft.AspNetCore.Authorization;

namespace JobLogic.Infrastructure.HealthCheck
{
    public class HealthCheckRequirement : IAuthorizationRequirement
    {
        public HealthCheckRequirement(string apiKey)
        {
            ApiKey = apiKey;
        }
        public string UserAgent { get; private set; } = HealthCheckHelper.UserAgent;
        public string ApiKey { get; set; }
    }
}

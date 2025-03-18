namespace JobLogic.Infrastructure.HealthCheck
{
    public static class HealthCheckHelper
    {
        public readonly static string Endpoint = "/healthcheck";
        public readonly static string UserAgent = "HealthCheck/1.0";
        public readonly static string HealthCheckPolicyName = "HealthCheckPolicy";
    }
}

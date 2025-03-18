namespace JobLogic.InfrastructureCore.IdentityServer.Configurations
{
    public class IdentityServiceClientOptions
    {
        public string BaseUrl { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string ApiName { get; set; }
        public string ApiSecret { get; set; }
        public string Scope { get; set; }
        public string DefaultPassword { get; set; }
    }
}

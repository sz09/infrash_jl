using Newtonsoft.Json;

namespace JobLogic.Infrastructure.AuthenticationHandler
{
    public class TokenErrorResponse
    {
        [JsonProperty(PropertyName = "error")]
        public string Error { get; set; }
    }
}
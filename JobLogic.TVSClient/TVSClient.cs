using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace JobLogic.TVSClient
{
    public interface ITVSClient
    {
        Task<HttpResponseMessage> PostAsync(object payload);
    }

    public class TVSClient : ITVSClient
    {
        private const string JsonFormat = "application/json";
        private const string ApiKeyName = "ApiKey";

        private readonly HttpClient _httpClient = new HttpClient();
        public readonly string _tvsJobApiUrl;

        public TVSClient(string token, string apiUrl)
        {
            _tvsJobApiUrl = apiUrl;

            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(apiUrl))
            {
                throw new ArgumentNullException("token || apiUrl");
            }

            if (!_httpClient.DefaultRequestHeaders.Accept.Any(x => x.MediaType == JsonFormat))
            {
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(JsonFormat));
            }

            _httpClient.DefaultRequestHeaders.Add(ApiKeyName, token);
        }

        public async Task<HttpResponseMessage> PostAsync(object payload)
        {
            var requestPayload = JsonConvert.SerializeObject(payload);
            var stringContent = new StringContent(requestPayload, System.Text.Encoding.UTF8, JsonFormat);

            var result = await _httpClient.PostAsync(_tvsJobApiUrl, stringContent);

            return result;
        }
    }
}

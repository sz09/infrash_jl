using JobLogic.Infrastructure.ServiceResponders;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace JobLogic.Infrastructure.CAPTCHA
{
    public class GoogleCaptchaRequest : BaseCAPTCHARequest
    {
        public string Content { get; set; }
    }

    public class GoogleCAPTCHAValidatorResult
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("challenge_ts")]
        public string Challenge { get; set; }

        [JsonProperty("hostname")]
        public string Hostname { get; set; }

        [JsonProperty("error-codes")]
        public List<string> ErrorCodes { get; set; }
    }

    public class GoogleCAPTCHAValidator : BaseCAPTCHAValidator<GoogleCaptchaRequest>
    {
        private readonly string _apiUrl = "";
        private readonly string _secretKey = "";
        private static HttpClient _httpClient = new HttpClient();

        public GoogleCAPTCHAValidator(string apiUrl, string secretKey)
        {
            _apiUrl = apiUrl;
            _secretKey = secretKey;
        }

        protected override async Task<Response> DoValidateAsync(GoogleCaptchaRequest captchaRequest, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (string.IsNullOrWhiteSpace(captchaRequest.Content))
            {
                return ResponseFactory.ReturnWithError(BaseCAPTCHAMessage.InvalidCAPTCHA);
            }

            var response = await _httpClient.PostAsync(_apiUrl, CreateHttpContent(captchaRequest));

            if (!response.IsSuccessStatusCode)
            {
                return ResponseFactory.ReturnWithError(BaseCAPTCHAMessage.InvalidCAPTCHA);
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var googleValidatorResult = JsonConvert.DeserializeObject<GoogleCAPTCHAValidatorResult>(responseContent);

            if (!googleValidatorResult.Success)
            {
                return ResponseFactory.ReturnWithError(googleValidatorResult.ErrorCodes);
            }

            return ResponseFactory.Return();
        }

        private HttpContent CreateHttpContent(GoogleCaptchaRequest captchaRequest)
        {
            var parameters = new Dictionary<string, string>
            {
                { "secret", _secretKey },
                { "response", captchaRequest.Content }
            };

            return new FormUrlEncodedContent(parameters);
        }
    }
}

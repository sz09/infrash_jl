using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace JobLogic.Infrastructure.Microservice.Client.Factory
{
    class RequestSender : IRequestSender
    {
        private const string JsonFormat = "application/json";
        
        private readonly IRequestSenderHttpClientFactory _requestSenderHttpClientFactory;

        public static StringContent BuildStringContent(object arg)
        {
            StringContent stringContent = null;
            if (arg != null)
            {
                var strArg = JsonConvert.SerializeObject(arg);
                stringContent = new StringContent(strArg, Encoding.UTF8, JsonFormat);
            }
            return stringContent;
        }

        public RequestSender(IRequestSenderHttpClientFactory requestSenderHttpClientFactory)
        {
            _requestSenderHttpClientFactory = requestSenderHttpClientFactory;
        }
        public virtual async Task SendAsync(RequestAddress address, MicroservicePayload payload)
        {
            var formatedStrUri = address.FullUrl;
            StringContent stringContent = BuildStringContent(payload);
            var httpClient = _requestSenderHttpClientFactory.HttpClientFor(address); ;
            var result = await httpClient.PostAsync(formatedStrUri, stringContent);
            if (!result.IsSuccessStatusCode)
                throw new MicroserviceClientException(result.ReasonPhrase);
        }

        public virtual async Task<T> SendAsync<T>(RequestAddress address, MicroservicePayload payload)
        {
            var formatedStrUri = address.FullUrl;
            StringContent stringContent = BuildStringContent(payload);
            var httpClient = _requestSenderHttpClientFactory.HttpClientFor(address); ;
            var result = await httpClient.PostAsync(formatedStrUri, stringContent);
            if (!result.IsSuccessStatusCode)
                throw new MicroserviceClientException(result.ReasonPhrase);
            var strRs = await result.Content.ReadAsStringAsync();
            if (typeof(T) == typeof(string))
                return (T)(object)strRs;
            return JsonConvert.DeserializeObject<T>(strRs);
        }
    }
}

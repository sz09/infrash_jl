using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;

namespace JobLogic.Infrastructure.HttpClientProvider
{
    public static class HttpClientExtensions
    {
        public static Task<HttpResponseMessage> PatchAsJsonAsync<T>(this HttpClient client, string requestUri, T value)
        {
            var content = new ObjectContent<T>(value, new JsonMediaTypeFormatter());
            var request = new HttpRequestMessage(new HttpMethod("PATCH"), requestUri) { Content = content };

            return client.SendAsync(request);
        }
    }
}

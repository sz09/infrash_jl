using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace JobLogic.Infrastructure.HttpClientProvider
{
    public interface IHttpClientProvider
    {
        Task<string> GetAsync(string url);

        Task PostEventAsync(string url, object data, Action notFoundServiceCallback = null);

        Task<T> PostEventAsync<T>(string url, object data, Action notFoundServiceCallback = null);

        Task<string> PostUrlEncodedContentAsync(string url, Dictionary<string, string> data);

    }

    public class HttpClientProvider : IHttpClientProvider
    {
        private const string JsonFormat = "application/json";
        private readonly HttpClient _client = new HttpClient();

        public HttpClientProvider(bool enableSecurityProtocol = false, int? timeout = null, AuthenticationHeaderValue authValue = null)
        {
            if (enableSecurityProtocol)
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            }

            if (!_client.DefaultRequestHeaders.Accept.Any(x => x.MediaType == JsonFormat))
            {
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(JsonFormat));
            }

            if (authValue != null)
            {
                _client.DefaultRequestHeaders.Authorization = authValue;
            }

            if (timeout != null && timeout.Value > 0)
            {
                _client.Timeout = new TimeSpan(0, 0, timeout.Value);
            }
        }

        public async Task<string> GetAsync(string url)
        {
            try
            {
                return await GetContentAsync(await _client.GetAsync(url));
            }
            catch (Exception ex)
            {
                throw ex.InnerException?.InnerException ?? ex.InnerException ?? ex;
            }
        }

        public async Task PostEventAsync(string url, object data, Action notFoundServiceCallback = null)
        {
            try
            {
                var response = await _client.PostAsJsonAsync(url, data);

                if (!response.IsSuccessStatusCode)
                {
                    if (notFoundServiceCallback != null && response.StatusCode == HttpStatusCode.NotFound)
                    {
                        notFoundServiceCallback();
                    }

                    throw new Exception(response.ReasonPhrase);
                }
            }
            catch (Exception)
            {
                // TODO: one exception will be throw if the endpoint url is invalid or the httpClient returns
                // This is a workaround to ignore the error in that case 
                // because many places in services call this method but not cacth to handle exception
                // And it will crash the application later.
                // Need one rework to get and handle the exception
            }
        }

        public async Task<T> PostEventAsync<T>(string url, object data, Action notFoundServiceCallback = null)
        {
            try
            {
                var response = await _client.PostAsJsonAsync(url, data);
                var result = await response.Content.ReadAsAsync<T>();
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    if (notFoundServiceCallback != null && response.StatusCode == HttpStatusCode.NotFound)
                    {
                        notFoundServiceCallback();
                    }

                    throw new Exception(await GetContentAsync(response));
                }

                return result;
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    if (ex.InnerException.InnerException != null)
                    {
                        throw ex.InnerException.InnerException;
                    }

                    throw ex.InnerException;
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<string> PostUrlEncodedContentAsync(string url, Dictionary<string, string> data)
        {
            try
            {
                using (var req = new HttpRequestMessage(HttpMethod.Post, url) { Content = new FormUrlEncodedContent(data) })
                {
                    return await GetContentAsync(await _client.SendAsync(req));
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        private Task<string> GetContentAsync(HttpResponseMessage response)
        {
            if (response != null)
            {
                return response.Content.ReadAsStringAsync();
            }

            return Task.FromResult(string.Empty);
        }
    }
}

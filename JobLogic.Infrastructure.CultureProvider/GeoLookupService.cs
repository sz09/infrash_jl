using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace JobLogic.Infrastructure.CultureProvider
{
    public class GeoLookupResponse
    {
        public string JsonResponse { get; set; }
        public string country_name { get; set; }
        public string time_zone { get; set; }
        public string calling_code { get; set; }
        public string currency_code { get; set; }
        public string region_name { get; set; }
        public string country_code { get; set; }
    }
    public interface IGeoLookupService
    {
        GeoLookupResponse GetGeoData(string ipAddress);
        Task<GeoLookupResponse> GetGeoDataAsync(string ipAddress, CancellationToken cancellationToken = default);
    }

    public class GeoLookupService : IGeoLookupService
    {
        private readonly string _apiKey;

        public GeoLookupService(string apiKey)
        {
            _apiKey = apiKey;
        }

        [Obsolete("Should use Async version instead")]
        public GeoLookupResponse GetGeoData(string ipAddress)
        {
            GeoLookupResponse geoResponse = null;
            var apiUrl = "https://api.ipstack.com";

            try
            {
                var ipAddressSplit = ipAddress.Split(':');
                var response = string.Empty;

                using (var client = new WebClient())
                {
                    client.Headers.Add("access-control-allow-origin", "*");
                    client.Headers.Add("Content-Type", "application/json");
                    client.Headers.Add("Content-Type", "Charset=UTF-8");
                    response = client.DownloadString($"{apiUrl}/{ipAddressSplit[0]}?access_key={_apiKey}");
                }
                if (!string.IsNullOrWhiteSpace(response))
                {
                    var jsonResponse = JsonConvert.DeserializeObject<dynamic>(response);
                    geoResponse = new GeoLookupResponse
                    {
                        JsonResponse = response,
                        country_name = jsonResponse["country_name"],
                        calling_code = jsonResponse["location"]["calling_code"],
                        time_zone = jsonResponse["time_zone"]["id"],
                        currency_code = jsonResponse["currency"]["code"],
                        region_name = jsonResponse["region_name"],
                        country_code = jsonResponse["country_code"]
                    };
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return geoResponse;
        }

        public async Task<GeoLookupResponse> GetGeoDataAsync(string ipAddress, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            GeoLookupResponse geoResponse = null;
            var apiUrl = "https://api.ipstack.com";

            var ipAddressSplit = ipAddress.Split(':');
            var response = string.Empty;

            using (var client = new WebClient())
            {
                client.Headers.Add("access-control-allow-origin", "*");
                client.Headers.Add("Content-Type", "application/json");
                client.Headers.Add("Content-Type", "Charset=UTF-8");
                response = await client.DownloadStringTaskAsync($"{apiUrl}/{ipAddressSplit[0]}?access_key={_apiKey}");
            }

            if (!string.IsNullOrWhiteSpace(response))
            {
                var jsonResponse = JsonConvert.DeserializeObject<dynamic>(response);
                geoResponse = new GeoLookupResponse
                {
                    JsonResponse = response,
                    country_name = jsonResponse["country_name"],
                    calling_code = jsonResponse["location"]["calling_code"],
                    time_zone = jsonResponse["time_zone"]["id"],
                    currency_code = jsonResponse["currency"]["code"],
                    region_name = jsonResponse["region_name"],
                    country_code = jsonResponse["country_code"]
                };
            }

            return geoResponse;
        }
    }
}

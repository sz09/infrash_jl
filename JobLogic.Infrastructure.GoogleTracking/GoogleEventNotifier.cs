using JobLogic.Infrastructure.ServiceResponders;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace JobLogic.Infrastructure.GoogleTracking
{
    public interface IGoogleEventNotifier
    {
        Task<Response> Track(string clientId, string eventCategory, string eventAction, string eventValue, string trackingId, string type = "", string eventLabel = "");
        Task<Response> TrackPost(string clientId, string eventCategory, string eventAction, string eventValue, string trackingId, string type = "event", string eventLabel = "");
    }

    public class GoogleEventNotifier : IGoogleEventNotifier
    {
        public async Task<Response> Track(string clientId, string eventCategory, string eventAction, string eventValue, string trackingId, string type = "event", string eventLabel = "")
        {
            if (string.IsNullOrWhiteSpace(trackingId)) return ResponseFactory.ReturnWithError("Missing UA Code");

            var client = new HttpClient();

            var url = $"https://www.google-analytics.com/collect?v=1&t={type}&tid={trackingId}&cid={clientId}&ec={eventCategory}&ea={eventAction}&ev={eventValue}&el={eventLabel}";
            var response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode) return ResponseFactory.Return();
            return ResponseFactory.ReturnWithError();
        }

        public async Task<Response> TrackPost(string clientId, string eventCategory, string eventAction, string eventValue, string trackingId, string type = "event", string eventLabel = "")
        {
            if (string.IsNullOrWhiteSpace(trackingId)) return ResponseFactory.ReturnWithError("Missing UA Code");

            // https://developers.google.com/analytics/devguides/collection/protocol/v1/reference
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://www.google-analytics.com/");

            var requestDictionary = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("v", "1"),
                    new KeyValuePair<string, string>("t", type),
                    new KeyValuePair<string, string>("tid", trackingId),
                    new KeyValuePair<string, string>("cid", clientId),
                    new KeyValuePair<string, string>("ec", eventCategory),
                    new KeyValuePair<string, string>("ea", eventAction),
                    new KeyValuePair<string, string>("ev", eventValue)
                };

            if (!string.IsNullOrWhiteSpace(eventLabel))
                requestDictionary.Add(new KeyValuePair<string, string>("el", eventLabel));

            var request = new FormUrlEncodedContent(requestDictionary);

            var response = await client.PostAsync("collect?", request);

            if (response.IsSuccessStatusCode) return ResponseFactory.Return();

            return ResponseFactory.ReturnWithError(response.ReasonPhrase);
        }
    }
}

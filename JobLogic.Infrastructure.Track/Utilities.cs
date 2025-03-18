using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace JobLogic.Infrastructure.Track
{
    public static class Utilities
    {
        public static IDictionary<string, string> MapToAppInsightCustomDimentionDataType(this object data)
        {
            if (data == null)
                return new Dictionary<string, string>();
            IDictionary<string, string> result;
            string stringData = data.ToString();
            try
            {
                stringData = Serialize(data);
                var temp = Deserialize<IDictionary<string, object>>(stringData);
                result = new Dictionary<string, string>();
                foreach (var k in temp)
                {
                    var value = Serialize(k.Value);
                    result.Add(k.Key, value);
                }
            }
            catch (Exception ex)
            {
                result = new Dictionary<string, string>() { { ApplicationInsightsTrackService.DFCD_MESSAGE, stringData } };
            }
            return result;
        }

        private static string Serialize(object source)
        {
            return Serialize(source, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        }

        private static string Serialize(object source, JsonSerializerSettings settings)
        {
            return source != null
                ? JsonConvert.SerializeObject(source, settings)
                : string.Empty;
        }

        private static TData Deserialize<TData>(string source)
        {
            if (!string.IsNullOrWhiteSpace(source))
            {
                return JsonConvert.DeserializeObject<TData>(source);
            }

            return default(TData);
        }
    }
}

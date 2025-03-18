using Newtonsoft.Json;

namespace JobLogic.Infrastructure.Utilities
{
    public static class JsonSerialization
    {
        public static string Serialize(object source)
        {
            return Serialize(source, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        }
        /// <summary>
        /// Serialize an object using Newtonsoft.Json
        /// </summary>
        /// <param name="source">object need to serialize</param>
        /// <param name="isIgnoreJsonProperty"> if true, result string will use the object properties value instead of value in [JsonProperty]. Example
        ///<para/>  [JsonProperty("first-name")]
        /// <para/> public string FirstName { get; set; }
        /// <para/> Will be serialized to {"FirstName":"..."} instead of {"first-name":"..."}
        /// </param>
        /// <returns></returns>
        public static string Serialize(object source, bool isIgnoreJsonProperty)
        {
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            };
            if (isIgnoreJsonProperty)
            {
                settings.ContractResolver = new IgnoreJsonPropertyResolver();
            }

            return Serialize(source, settings);
        }
        public static string Serialize(object source, JsonSerializerSettings settings)
        {
            return source != null
                ? JsonConvert.SerializeObject(source, settings)
                : string.Empty;
        }

        public static TData Deserialize<TData>(string source)
        {
            if (!string.IsNullOrWhiteSpace(source))
            {
                return JsonConvert.DeserializeObject<TData>(source);
            }

            return default(TData);
        }

        public static TData Deserialize<TData>(string source, bool isIgnoreJsonProperty)
        {
            if (!string.IsNullOrWhiteSpace(source))
            {
                return JsonConvert.DeserializeObject<TData>(source, isIgnoreJsonProperty ? new JsonSerializerSettings()
                {
                    ContractResolver = new IgnoreJsonPropertyResolver()
                } : null);
            }

            return default(TData);
        }
    }
}

using Newtonsoft.Json;

namespace Microsoft.Extensions.Logging
{
    public static class Extensions
    {
        public static string ToJsonLogString(this object source)
        {
            return source != null
                ? JsonConvert.SerializeObject(source, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                })
                : string.Empty;
        }
    }
}

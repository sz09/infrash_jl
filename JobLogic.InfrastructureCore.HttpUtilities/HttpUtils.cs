using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace JobLogic.InfrastructureCore.HttpUtilities
{
    public static class HttpUtils
    {
        private const string localIPv6 = "::1";
        private const string localIPv4 = "127.0.0.1";
        public static string GetIpAddress(this HttpContext currentContext, bool isDebug = false, bool isWebJob = false)
        {
            if (isWebJob || isDebug)
                return Environment.MachineName;

            if (currentContext != null)
            {
                var ip = currentContext.Request.Headers["X-Forwarded-For"].FirstOrDefault() ?? currentContext.Connection.RemoteIpAddress.ToString();
                var currentUserIp = ip?.Split(',')[0];

                #if DEBUG
                if (currentUserIp == localIPv6)
                {
                    // set dev user IP to v4, as GetGeoDataAsync unable to parse ::1 
                    currentUserIp = localIPv4;
                }
                #endif

                return currentUserIp;
            }
            return string.Empty;
        }

        public static string GetCurrentUrl(this HttpContext httpContext, string path)
        {
            if (!string.IsNullOrWhiteSpace(path))
                if (path.StartsWith("/")) path = path.Substring(1);

            if (httpContext != null)
            {
                var urlRequest = httpContext.Request;

                // HTTP(S)://HOST:PORT
                return string.Format("{0}://{1}:{2}/{3}", urlRequest.Scheme, urlRequest.Host.Host, urlRequest.Host.Port, path);
            }
            else
            {
                return string.Format("{0}/{1}", Directory.GetCurrentDirectory(), path);
            }
        }

        public static T QueryStringToJson<T>(this string request)
        {
            var dict = HttpUtility.ParseQueryString(request);
            var json = System.Text.Json.JsonSerializer.Serialize(dict.Keys.Cast<string>().ToDictionary(k => k, k => dict[k]));

            return JsonConvert.DeserializeObject<T>(json);
        }

        public static string ConvertNVCToJsonString(this IFormCollection source, string[] ignoreKeys = null)
        {
            if (source == null || source.Count == 0)
                return null;
            var dict = new Dictionary<string, string>();
            foreach (var key in source)
            {
                if (ignoreKeys == null || !ignoreKeys.Contains(key.Key))
                {
                    dict.Add(key.Key, key.Value.ToString());
                }
            }
            return System.Text.Json.JsonSerializer.Serialize(dict);
        }

        public static bool IsAjaxRequest(this HttpRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (request.Headers != null)
                return request.Headers["X-Requested-With"] == "XMLHttpRequest";
            return false;
        }

        public static string GetUserAgent(this HttpRequest request)
        {
            return request.Headers["User-Agent"];
        }

        public static string GetFullUrl(this HttpContext context)
        {
            return context?.Request?.GetFullUrl();
        }

        public static string GetFullUrl(this HttpRequest request)
        {
            return request == null ? null : $"{request.Scheme}://{request.Host}{request.Path}{request.QueryString}";
        }
    }
}

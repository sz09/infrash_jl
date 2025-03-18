using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace JobLogic.Infrastructure.DistributedCache
{
    public static class Extensions
    {
        public static async Task<FindCacheResult<T>> GetObjectAsync<T>(this IDistributedCacheClient distributedCacheClient, string key)
        {
            var str = await distributedCacheClient.GetValueAsync(key);
            if (str == null) return FindCacheResult<T>.CreateNotFoundResult();
            T val;
            if (typeof(T) == typeof(string))
                val = (T)(object)str;
            else
            {
                val = JsonConvert.DeserializeObject<T>(str);
            }
            return FindCacheResult<T>.CreateFoundResult(val);
        }

        public static Task<bool> SetObjectAsync<T>(this IDistributedCacheClient distributedCacheClient, string key, T value)
        {
            if (value == null)
            {
                return distributedCacheClient.SetValueAsync(key, null);
            }
            if (typeof(T) == typeof(string))
            {
                return distributedCacheClient.SetValueAsync(key, value.ToString());
            }
            return distributedCacheClient.SetValueAsync(key, JsonConvert.SerializeObject(value));
        }

        public static Task<bool> SetObjectAsync<T>(this IDistributedCacheClient distributedCacheClient, string key, T value, TimeSpan expire)
        {
            if (value == null)
            {
                return distributedCacheClient.SetValueAsync(key, null);
            }
            if (typeof(T) == typeof(string))
            {
                return distributedCacheClient.SetValueAsync(key, value.ToString(), expire);
            }
            return distributedCacheClient.SetValueAsync(key, JsonConvert.SerializeObject(value), expire);
        }

        public const string DefaultTryMarkIdempotentOperationAsHandledPrefix = "JL_TryMarkIdempotentOperationAsHandled89/";
        public const int DefaultTryMarkIdempotentOperationAsHandledExpiredInMinutes = 1440;

        public static async Task<bool> TryMarkIdempotentOperationAsHandledAsync(this IDistributedCacheClient distributedCacheClient, string handlerName, Guid idempotentId, int expiredInMinutes = DefaultTryMarkIdempotentOperationAsHandledExpiredInMinutes, string prefixWithSlash = DefaultTryMarkIdempotentOperationAsHandledPrefix)
        {
            if(idempotentId == Guid.Empty)
            {
                throw new ArgumentException("idempotentId cannot be empty");
            }
            return await TryMarkIdempotentOperationAsHandledAsync(distributedCacheClient, handlerName, idempotentId.ToString(), expiredInMinutes, prefixWithSlash);
        }

        public static async Task<bool> TryMarkIdempotentOperationAsHandledAsync(this IDistributedCacheClient distributedCacheClient, string handlerName, string idempotentId, int expiredInMinutes = DefaultTryMarkIdempotentOperationAsHandledExpiredInMinutes, string prefixWithSlash = DefaultTryMarkIdempotentOperationAsHandledPrefix)
        {
            if (string.IsNullOrWhiteSpace(handlerName) || string.IsNullOrWhiteSpace(idempotentId))
            {
                throw new ArgumentException("handlerName and idempotentId cannot be null or empty");
            }
            var db = await distributedCacheClient.GetInnerRedisIDatabaseAsync();
            var key = $"{prefixWithSlash}{handlerName}/{idempotentId}";
            var result = await db.StringSetAsync(key, DateTime.UtcNow.ToString(), TimeSpan.FromMinutes(expiredInMinutes), When.NotExists);
            return result;
        }
    }
}

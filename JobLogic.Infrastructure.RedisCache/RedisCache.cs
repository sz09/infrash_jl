using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobLogic.Infrastructure.RedisCache
{
    public class RedisCache : ICache
    {
        public RedisCache(IDatabase database)
        {
            CacheDatabase = database;
        }

        public IDatabase CacheDatabase { get; }

        [Obsolete("Should use Async version instead")]
        public string GetValue(string key)
        {
            var cacheValue = CacheDatabase.StringGet(key);
            return cacheValue.HasValue ? cacheValue.ToString() : string.Empty;
        }

        public async Task<string> GetValueAsync(string key)
        {
            var cacheValue = await CacheDatabase.StringGetAsync(key);
            return cacheValue.HasValue ? cacheValue.ToString() : string.Empty;
        }

        public Task<bool> HasKeyAsync(string key)
        {
            return CacheDatabase.KeyExistsAsync(key);
        }

        public Task<bool> SetValueAsync(string key, string value)
        {
            return CacheDatabase.StringSetAsync(key, value);
        }

        [Obsolete("Should use Async version instead")]
        public bool SetValue(string key, string value, TimeSpan expire)
        {
            return CacheDatabase.StringSet(key, value, expire);
        }

        public Task<bool> SetValueAsync(string key, string value, TimeSpan expire)
        {
            return CacheDatabase.StringSetAsync(key, value, expire);
        }

        public Task<bool> RemoveAsync(string key)
        {
            return CacheDatabase.KeyDeleteAsync(key);
        }

        public async Task<long> RemoveAsync(IEnumerable<string> keys)
        {
            if (keys == null || !keys.Any())
                return 0;

            var redisKeys = keys.Select(s => new RedisKey(s));
            return await CacheDatabase.KeyDeleteAsync(redisKeys.ToArray());
        }
    }
}

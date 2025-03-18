using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobLogic.Infrastructure.DistributedCache
{
    public interface IDistributedCacheClient
    {
        Task<bool> HasKeyAsync(string key);
        Task<string> GetValueAsync(string key);
        Task<bool> SetValueAsync(string key, string value);
        Task<bool> SetValueAsync(string key, string value, TimeSpan expire);

        Task<bool> RemoveAsync(string key);
        Task<long> RemoveAsync(IEnumerable<string> keys);
        Task<long> IncreaseValueAsync(string key, long value = 1L);
        Task<long> DecreaseValueAsync(string key, long value = 1L);
        Task<bool> UpdateExpiryAsync(string key, DateTime newExpiryUtc);
        Task<IDatabase> GetInnerRedisIDatabaseAsync();
    }
    public class RedisCacheClient : IDistributedCacheClient
    {
        private readonly Lazy<Task<IDatabase>> _lzIDatabase;

        static async Task<IDatabase> GetIDatabaseAsync(string cacheConnStr)
        {
            var multiplexer = await ConnectionMultiplexerUtils.GetConnectionMultiplexerAsync(cacheConnStr);

            return multiplexer.GetDatabase();
        }

        public RedisCacheClient(string cacheConnStr)
        {
            _lzIDatabase = new Lazy<Task<IDatabase>>(() => GetIDatabaseAsync(cacheConnStr));
        }

        public async Task<string> GetValueAsync(string key)
        {
            var database = await _lzIDatabase.Value;
            var cacheValue = await database.StringGetAsync(key);
            return cacheValue.HasValue ? cacheValue.ToString() : null;
        }

        public async Task<bool> HasKeyAsync(string key)
        {
            var database = await _lzIDatabase.Value;
            return await database.KeyExistsAsync(key);
        }

        public async Task<bool> SetValueAsync(string key, string value)
        {
            var database = await _lzIDatabase.Value;
            return await database.StringSetAsync(key, value);
        }

        public async Task<bool> SetValueAsync(string key, string value, TimeSpan expire)
        {
            var database = await _lzIDatabase.Value;
            return await database.StringSetAsync(key, value, expire);
        }

        public async Task<bool> RemoveAsync(string key)
        {
            var database = await _lzIDatabase.Value;
            return await database.KeyDeleteAsync(key);
        }

        public async Task<long> RemoveAsync(IEnumerable<string> keys)
        {
            var database = await _lzIDatabase.Value;
            if (keys == null || !keys.Any())
                return 0;

            var redisKeys = keys.Select(s => new RedisKey(s));
            return await database.KeyDeleteAsync(redisKeys.ToArray());
        }

        /// <summary>
        ///     Increments the number stored at key by increment. If the key does not exist,
        ///     it is set to 0 before performing the operation. An error is returned if the key
        ///     contains a value of the wrong type or contains a string that is not representable
        ///     as integer. This operation is limited to 64 bit signed integers.
        /// </summary>
        /// <param name="key">The key of the string.</param>
        /// <param name="value">The amount to increment by (defaults to 1).</param>
        /// <returns>The value of key after the increment.</returns>
        public async Task<long> IncreaseValueAsync(string key, long value = 1L)
        {
           var database = await _lzIDatabase.Value;
           return await database.StringIncrementAsync(key, value);
        }
        /// <summary>
        ///     Decrements the number stored at key by decrement. If the key does not exist,
        ///     it is set to 0 before performing the operation. An error is returned if the key
        ///     contains a value of the wrong type or contains a string that is not representable
        ///     as integer. This operation is limited to 64 bit signed integers.
        /// </summary>
        /// <param name="key">The key of the string.</param>
        /// <param name="value">The amount to decrement by (defaults to 1).</param>
        /// <returns>The value of key after the decrement.</returns>
        public async Task<long> DecreaseValueAsync(string key, long value = 1L)
        {
            var database = await _lzIDatabase.Value;
            return await database.StringDecrementAsync(key, value);
        }

        public async Task<bool> UpdateExpiryAsync(string key, DateTime newExpiryUtc)
        {
            var database = await _lzIDatabase.Value;
            return await database.KeyExpireAsync(key, newExpiryUtc);
        }

        public Task<IDatabase> GetInnerRedisIDatabaseAsync()
        {
            return _lzIDatabase.Value;
        }
    }
}

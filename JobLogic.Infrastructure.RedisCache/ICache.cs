using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobLogic.Infrastructure.RedisCache
{
    public interface ICache
    {
        Task<bool> HasKeyAsync(string key);

        [Obsolete("Should use Async version instead")]
        string GetValue(string key);
        Task<string> GetValueAsync(string key);

        Task<bool> SetValueAsync(string key, string value);

        [Obsolete("Should use Async version instead")]
        bool SetValue(string key, string value, TimeSpan expire);
        Task<bool> SetValueAsync(string key, string value, TimeSpan expire);

        Task<bool> RemoveAsync(string key);
        Task<long> RemoveAsync(IEnumerable<string> keys);
    }
}

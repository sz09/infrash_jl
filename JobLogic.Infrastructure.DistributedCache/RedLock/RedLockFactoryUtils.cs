using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;
using StackExchange.Redis;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace JobLogic.Infrastructure.DistributedCache
{
    static class RedLockFactoryUtils
    {
        static object _locker = new object();
        static SemaphoreSlim _asyncLocker = new SemaphoreSlim(1, 1);
        static ConcurrentDictionary<string, RedLockFactory> RedLockFactoryDict = new ConcurrentDictionary<string, RedLockFactory>();

        public static async Task<RedLockFactory> GetRedLockFactoryAsync(string redisConnstr)
        {
            if (!RedLockFactoryDict.ContainsKey(redisConnstr))
            {
                await _asyncLocker.WaitAsync();
                try
                {
                    if (!RedLockFactoryDict.ContainsKey(redisConnstr))
                    {
                        var multiplexer = await ConnectionMultiplexer.ConnectAsync(redisConnstr);
                        var factory = RedLockFactory.Create(new List<RedLockMultiplexer> { multiplexer });
                        RedLockFactoryDict.TryAdd(redisConnstr, factory);
                    }
                }
                finally
                {
                    _asyncLocker.Release();
                }

            }
            return RedLockFactoryDict[redisConnstr];
        }

        public static RedLockFactory GetRedLockFactory(string redisConnStr)
        {
            if (!RedLockFactoryDict.ContainsKey(redisConnStr))
            {
                lock (_locker)
                {
                    if (!RedLockFactoryDict.ContainsKey(redisConnStr))
                    {
                        var multiplexer = ConnectionMultiplexer.Connect(redisConnStr);
                        var factory = RedLockFactory.Create(new List<RedLockMultiplexer> { multiplexer });
                        RedLockFactoryDict.TryAdd(redisConnStr, factory);
                    }
                }
            }
            return RedLockFactoryDict[redisConnStr];
        }
    }
}
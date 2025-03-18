using StackExchange.Redis;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace JobLogic.Infrastructure.DistributedCache
{
    static class ConnectionMultiplexerUtils
    {
        static SemaphoreSlim _asyncLocker = new SemaphoreSlim(1, 1);
        static ConcurrentDictionary<string, ConnectionMultiplexer> ConnectionMultiplexerDict = new ConcurrentDictionary<string, ConnectionMultiplexer>();
        public static async Task<ConnectionMultiplexer> GetConnectionMultiplexerAsync(string redisConnstr)
        {
            if (!ConnectionMultiplexerDict.ContainsKey(redisConnstr))
            {
                await _asyncLocker.WaitAsync();
                try
                {
                    if (!ConnectionMultiplexerDict.ContainsKey(redisConnstr))
                    {
                        var multiplexer = await ConnectionMultiplexer.ConnectAsync(redisConnstr);
                        ConnectionMultiplexerDict.TryAdd(redisConnstr, multiplexer);
                    }
                }
                finally
                {
                    _asyncLocker.Release();
                }

            }
            return ConnectionMultiplexerDict[redisConnstr];
        }
    }
}

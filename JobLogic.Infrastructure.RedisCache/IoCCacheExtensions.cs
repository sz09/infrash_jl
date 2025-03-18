using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System;
using System.Collections.Concurrent;

namespace JobLogic.Infrastructure.RedisCache
{
    public static class IoCCacheExtensions
    {
        public static void AddRedisIDatabase(this IServiceCollection services, Func<IServiceProvider,string> redisConnStringFunc)
        {
            services.AddTransient(x =>
            {
                var cacheConnection = redisConnStringFunc(x);
                return GetConnectionMultiplexer(cacheConnection).GetDatabase();
            });
        }

        static ConcurrentDictionary<string, ConnectionMultiplexer> ConnectionMultiplexerDict = new ConcurrentDictionary<string, ConnectionMultiplexer>();

        static ConnectionMultiplexer GetConnectionMultiplexer(string cacheConn)
        {
            return ConnectionMultiplexerDict.GetOrAdd(cacheConn, x =>
            {
                var multiplexer = ConnectionMultiplexer.Connect(x);
                return multiplexer;
            });
        }
    }
}

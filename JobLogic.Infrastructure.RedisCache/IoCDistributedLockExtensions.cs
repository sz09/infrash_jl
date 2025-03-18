using Microsoft.Extensions.DependencyInjection;
using RedLockNet;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;
using StackExchange.Redis;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace JobLogic.Infrastructure.RedisCache
{
    public static class IoCDistributedLockExtensions
    {
        public static void AddRedisIDistributedLockFactory(this IServiceCollection services, Func<IServiceProvider, string> redisConnStringFunc)
        {
            services.AddTransient(x =>
            {
                var cacheConnection = redisConnStringFunc(x);
                return GetRedLockFactory(cacheConnection);
            });
        }

        static ConcurrentDictionary<string, ConnectionMultiplexer> ConnectionMultiplexerDict = new ConcurrentDictionary<string, ConnectionMultiplexer>();
        static ConcurrentDictionary<string, IDistributedLockFactory> DistributedLockFactoryDict = new ConcurrentDictionary<string, IDistributedLockFactory>();

        static ConnectionMultiplexer GetConnectionMultiplexer(string cacheConn)
        {
            return ConnectionMultiplexerDict.GetOrAdd(cacheConn, x =>
            {
                var multiplexer = ConnectionMultiplexer.Connect(x);
                return multiplexer;
            });
        }
        static IDistributedLockFactory GetRedLockFactory(string cacheConn)
        {
            return DistributedLockFactoryDict.GetOrAdd(cacheConn, x =>
            {
                var multiplexer = GetConnectionMultiplexer(x);
                return RedLockFactory.Create(new List<RedLockMultiplexer> { multiplexer });
            });
        }
    }
}

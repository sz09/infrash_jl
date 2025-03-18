using Microsoft.Extensions.DependencyInjection;
using RedLockNet;
using System;

namespace JobLogic.Infrastructure.DistributedCache
{
    public static class IoCExtensions
    {
        public static void AddIDistributedCacheClient(this IServiceCollection services, Func<IServiceProvider, string> redisCacheConnStrEval)
        {
            services.AddTransient<IDistributedCacheClient>(x => new RedisCacheClient(redisCacheConnStrEval(x)));
        }

        public static void AddIDistributedLockFactory(this IServiceCollection services, Func<IServiceProvider, string> redisCacheConnStrEval)
        {
            services.AddTransient<IDistributedLockFactory>(x => new RedLockClient(redisCacheConnStrEval(x)));
        }
    }
}

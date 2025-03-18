namespace Tests.Integration.JobLogic.Infrastructure.DistributedCache
{
    public class AppConfig
    {
        public static string CacheConnection { get; set; } = "JobLogicDevRedisCache.redis.cache.windows.net,abortConnect=false,ssl=true,password=blHDpRFKFA1OdmJPVEOMu6qAQf7z9r7RQ1PxuaflF5M=";
    }
}

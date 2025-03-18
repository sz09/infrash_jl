using JobLogic.Infrastructure.DistributedCache;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace JobLogic.Infrastructure.Microservice.Client
{
    public class Cachetserter : ICachetserter
    {
        private readonly IDistributedCacheClient _distributedCacheClient;
        private readonly ILogger<Cachetserter> _logger;

        public Cachetserter(IDistributedCacheClient distributedCacheClient, ILogger<Cachetserter> logger)
        {
            _distributedCacheClient = distributedCacheClient;
            _logger = logger;
        }

        public async Task<T> CachetsertAsync<T>(string cacheKey, Func<Task<T>> originalDataFetchFunc, Func<T, bool> doSetCachePredicate, TimeSpan cacheExpiration)
        {
            try
            {
                var findRs = await _distributedCacheClient.GetObjectAsync<T>(cacheKey);
                switch (findRs.State)
                {
                    case FindCacheResultState.NotFound:
                        var rs = await originalDataFetchFunc();
                        bool doSetCache = true;
                        if(doSetCachePredicate != null)
                        {
                            doSetCache = doSetCachePredicate(rs);
                        }
                        if (doSetCache)
                        {
                            await InsertCacheAsync(rs, cacheKey, cacheExpiration);
                        }
                        return rs;
                    case FindCacheResultState.Found:
                        return findRs.GetValueOnlyWhenStateIsFound();
                    default:
                        throw new NotSupportedException();
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning("[CachetsertAsync failed] General failure: CacheKey: {0} . Message: {1}", cacheKey, ex.Message);
                return await originalDataFetchFunc();
            }
        }

        private async Task InsertCacheAsync<T>(T value, string cacheKey, TimeSpan cacheExpiration)
        {
            try
            {
                await _distributedCacheClient.SetObjectAsync(cacheKey, value, cacheExpiration);
            }
            catch (Exception ex)
            {
                _logger.LogWarning("[CachetsertAsync failed] Failed to set cache. CacheKey: {0} . Message: {1}", cacheKey, ex.Message);
            }
        }
    }
}

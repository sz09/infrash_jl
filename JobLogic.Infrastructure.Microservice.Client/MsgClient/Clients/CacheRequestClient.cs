using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace JobLogic.Infrastructure.Microservice.Client
{
    public interface ICachetserter
    {
        public Task<T> CachetsertAsync<T>(string cacheKey, Func<Task<T>> originalDataFetchFunc, Func<T, bool> doSetCachePredicate, TimeSpan cacheExpiration);
    }
    public class CacheRequestClient
    {
        private readonly ICachetserter _cachetserter;
        private readonly RequestClient _requestClient;

        public CacheRequestClient(ICachetserter cachetserter,RequestClient requestClient)
        {
            _cachetserter = cachetserter;
            _requestClient = requestClient;
        }

        public async Task<R> CachetsertAsync<T, R>(string cacheKey , ITenantlessOperationInfo operationInfo,T input, 
            Func<R,bool> doSetCachePredicate = null, double cacheExpiredInMinutes = 60) where T:TenantlessMsg,IHasReturn<R>
        {
            var cacheExpire = TimeSpan.FromMinutes(cacheExpiredInMinutes);
            var val = await _cachetserter.CachetsertAsync(cacheKey, 
                    () => _requestClient.RequestTenantlessHandlerAsync<T, R>(
                            operationInfo, input
                            ),
                    doSetCachePredicate,
                    cacheExpire);
            return val;
        }



        public async Task<R> CachetsertAsync<T, R>(string cacheKey, ITenancyOperationInfo operationInfo, T input,
            Func<R, bool> doSetCachePredicate = null, double cacheExpiredInMinutes = 60) where T : TenancyMsg, IHasReturn<R>
        {
            var cacheExpire = TimeSpan.FromMinutes(cacheExpiredInMinutes);
            var val = await _cachetserter.CachetsertAsync(cacheKey,
                () => _requestClient.RequestTenancyHandlerAsync<T, R>(
                            operationInfo, input
                            ),
                doSetCachePredicate,
                cacheExpire);
            return val;
        }
    }
}

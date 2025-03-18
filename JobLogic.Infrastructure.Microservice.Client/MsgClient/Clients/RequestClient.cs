using System;
using System.Threading.Tasks;

namespace JobLogic.Infrastructure.Microservice.Client
{
    

    public class RequestClient
    {
        private readonly IRequestAddressResolver _requestAddressResolver;
        private readonly IRequestSender _requestSender;

        public RequestClient(IRequestAddressResolver requestAddressResolver, IRequestSender requestSender)
        {
            _requestAddressResolver = requestAddressResolver;
            _requestSender = requestSender;
        }

        public Task RequestTenancyHandlerAsync(ITenancyOperationInfo operationInfo, TenancyMsg message)
        {
            var address = _requestAddressResolver.Resolve(message);
            var payload = MicroservicePayload.Create(operationInfo, message);
            return _requestSender.SendAsync(address, payload);
        }

        public Task RequestTenantlessHandlerAsync(ITenantlessOperationInfo operationInfo, TenantlessMsg message)
        {
            var address = _requestAddressResolver.Resolve(message);
            var payload = MicroservicePayload.Create(operationInfo, message);
            return _requestSender.SendAsync(address, payload);
        }

        public Task RequestTenancyHandlerAsync(ITenantlessOperationInfo operationInfo, TenancyMsg message, Guid tenantId)
        {
            var address = _requestAddressResolver.Resolve(message);
            var payload = MicroservicePayload.Create(operationInfo, message, tenantId);
            return _requestSender.SendAsync(address, payload);
        }

        public Task<R> RequestTenancyHandlerAsync<T, R>(ITenancyOperationInfo operationInfo, T message) where T : TenancyMsg, IHasReturn<R>
        {
            var address = _requestAddressResolver.Resolve(message);
            var payload = MicroservicePayload.Create(operationInfo, message);
            return _requestSender.SendAsync<R>(address, payload);
        }

        public Task<R> RequestTenantlessHandlerAsync<T, R>(ITenantlessOperationInfo operationInfo, T message) where T : TenantlessMsg, IHasReturn<R>
        {
            var address = _requestAddressResolver.Resolve(message);
            var payload = MicroservicePayload.Create(operationInfo, message);
            return _requestSender.SendAsync<R>(address, payload);
        }

        public Task<R> RequestTenancyHandlerAsync<T, R>(ITenantlessOperationInfo operationInfo, T message, Guid tenantId) where T : TenancyMsg, IHasReturn<R>
        {
            var address = _requestAddressResolver.Resolve(message);
            var payload = MicroservicePayload.Create(operationInfo, message, tenantId);
            return _requestSender.SendAsync<R>(address, payload);
        }
    }
}

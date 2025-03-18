using System;
using System.Threading.Tasks;

namespace JobLogic.Infrastructure.Microservice.Client
{
    public class EventClient
    {
        private readonly IEventAddressResolver _eventAddressResolver;
        private readonly IEventBroadcaster _eventBroadcaster;

        public EventClient(IEventAddressResolver eventAddressResolver, IEventBroadcaster eventBroadcaster)
        {
            _eventAddressResolver = eventAddressResolver;
            _eventBroadcaster = eventBroadcaster;
        }

        public Task PublishAsync(ITenancyOperationInfo operationInfo, TenancyEvent tenancyEvent, EventServiceBusOption serviceBusOption)
        {
            var payload = MicroservicePayload.Create(operationInfo, tenancyEvent);
            var address = _eventAddressResolver.Resolve(tenancyEvent);
            return _eventBroadcaster.BroadcastAsync(address, payload, serviceBusOption);
        }

        public Task PublishAsync(ITenantlessOperationInfo operationInfo, TenantlessEvent tenantlessEvent, EventServiceBusOption serviceBusOption)
        {
            var payload = MicroservicePayload.Create(operationInfo, tenantlessEvent);
            var address = _eventAddressResolver.Resolve(tenantlessEvent);
            return _eventBroadcaster.BroadcastAsync(address, payload, serviceBusOption);
        }

        public Task PublishAsync(ITenantlessOperationInfo operationInfo, TenancyEvent tenancyEvent, Guid tenantId, EventServiceBusOption serviceBusOption)
        {
            var payload = MicroservicePayload.Create(operationInfo, tenancyEvent, tenantId);
            var address = _eventAddressResolver.Resolve(tenancyEvent);
            return _eventBroadcaster.BroadcastAsync(address, payload, serviceBusOption);
        }
    }
}

using JobLogic.Infrastructure.Contract;
using System;
using System.Threading.Tasks;

namespace JobLogic.Infrastructure.Event.Publisher
{
    public interface ITenantlessEventPublisher
    {
        Task PublishAsync(TenancyEvent jlEvent, Guid tenantId, EventServiceBusOption serviceBusOption = null);
        Task PublishAsync(TenantlessEvent jlEvent, EventServiceBusOption serviceBusOption = null);
    }
    public class TenantlessEventPublisher : ITenantlessEventPublisher
    {
        private readonly ITenantlessOperationInfo _operationInfo;
        private readonly EventClient _eventClient;
        public TenantlessEventPublisher(ITenantlessOperationInfo operationInfo, PublisherClient client)
        {
            _operationInfo = operationInfo;
            _eventClient = client.EventClient;
        }

        public Task PublishAsync(TenantlessEvent jlEvent, EventServiceBusOption serviceBusOption = null)
        {
            return _eventClient.PublishAsync(_operationInfo, jlEvent, serviceBusOption);
        }

        public Task PublishAsync(TenancyEvent jlEvent, Guid tenantId, EventServiceBusOption serviceBusOption = null)
        {
            return _eventClient.PublishAsync(_operationInfo, jlEvent, tenantId, serviceBusOption);
        }
    }
}

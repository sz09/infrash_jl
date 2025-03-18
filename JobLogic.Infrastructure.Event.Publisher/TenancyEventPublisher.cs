using JobLogic.Infrastructure.Contract;
using System.Threading.Tasks;

namespace JobLogic.Infrastructure.Event.Publisher
{
    public interface ITenancyEventPublisher
    {
        Task PublishAsync(TenancyEvent jlEvent, EventServiceBusOption serviceBusOption = null);
    }
    public class TenancyEventPublisher : ITenancyEventPublisher
    {
        private readonly ITenancyOperationInfo _operationInfo;
        private readonly EventClient _eventClient;

        public TenancyEventPublisher(ITenancyOperationInfo operationInfo, PublisherClient client)
        {
            _operationInfo = operationInfo;
            _eventClient = client.EventClient;
        }

        public Task PublishAsync(TenancyEvent jlEvent, EventServiceBusOption serviceBusOption = null)
        {
            return _eventClient.PublishAsync(_operationInfo, jlEvent, serviceBusOption);
        }
    }

    
}

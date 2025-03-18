
using System.Threading.Tasks;

namespace JobLogic.Infrastructure.Microservice.Client
{
    public interface IEventBroadcaster
    {
        Task BroadcastAsync(EventAddress address, MicroservicePayload payload, EventServiceBusOption serviceBusOption);
    }
    public interface IEventBroadcasterFactory
    {
        IEventBroadcaster GetEventBroadcaster();
    }
}

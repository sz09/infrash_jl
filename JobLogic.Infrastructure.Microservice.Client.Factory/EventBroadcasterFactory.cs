using JobLogic.Infrastructure.Microservice.Client.Factory;

namespace JobLogic.Infrastructure.Microservice.Client
{
    public class EventBroadcasterFactory : IEventBroadcasterFactory
    {
        
        public EventBroadcasterFactory()
        {
        }
        public IEventBroadcaster GetEventBroadcaster()
        {
            return new EventBroadcaster();
        }
    }
}

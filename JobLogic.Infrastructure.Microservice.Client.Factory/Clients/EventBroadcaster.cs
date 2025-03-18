using Azure.Messaging.ServiceBus;
using System.Threading.Tasks;

namespace JobLogic.Infrastructure.Microservice.Client.Factory
{
    public class EventBroadcaster : IEventBroadcaster
    {
        public Task BroadcastAsync(EventAddress address, MicroservicePayload payload, EventServiceBusOption serviceBusOption)
        {
            var sbmessage = payload.ToTopicEventMessage(serviceBusOption, address.SubscribingServicesNotation);
            ServiceBusSender topicClient = TopicClientFactory.For(address.EventSBNS, address.TopicName);
            return topicClient.SendMessageAsync(sbmessage);
        }     
    }
}

using Azure.Messaging.ServiceBus;
using System.Threading.Tasks;

namespace JobLogic.Infrastructure.Microservice.Client.Factory
{
    class CommandSessionSender : ICommandSessionSender
    {
        public virtual Task SendAsync(CommandAddress address, MicroservicePayload payload, ServiceBusSession session, ServiceBusOption serviceBusOption = null)
        {
            ServiceBusSender queueClient = QueueClientFactory.For(address.SBNS, address.QueueName);
            var sbmessage = payload.ToMessage(session, serviceBusOption);
            return queueClient.SendMessageAsync(sbmessage);
        }
    }
}

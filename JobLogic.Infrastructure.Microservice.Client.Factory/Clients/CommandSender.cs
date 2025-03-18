using Azure.Messaging.ServiceBus;
using System.Threading.Tasks;

namespace JobLogic.Infrastructure.Microservice.Client.Factory
{
    class CommandSender : ICommandSender
    {
        public virtual Task SendAsync(CommandAddress address, MicroservicePayload payload, ServiceBusOption serviceBusOption)
        {
            ServiceBusSender queueClient = QueueClientFactory.For(address.SBNS, address.QueueName);
            var sbmessage = payload.ToMessage(serviceBusOption);
            return queueClient.SendMessageAsync(sbmessage);
        }        
    }
}

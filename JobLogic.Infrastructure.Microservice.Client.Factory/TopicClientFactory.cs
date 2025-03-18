using Azure.Messaging.ServiceBus;
using System.Collections.Concurrent;

namespace JobLogic.Infrastructure.Microservice.Client.Factory
{
    static class TopicClientFactory
    {
        static ConcurrentDictionary<string, ServiceBusSender> _cacheSenders = new ConcurrentDictionary<string, ServiceBusSender>();
        static ConcurrentDictionary<string, ServiceBusClient> _cacheSBClients = new ConcurrentDictionary<string, ServiceBusClient>();
        public static ServiceBusSender For(string sbns, string topicName)
        {
            var sender = _cacheSenders.GetOrAdd(sbns + "|" + topicName, k =>
            {
                var client = _cacheSBClients.GetOrAdd(sbns, _ => new ServiceBusClient(sbns));
                return client.CreateSender(topicName);
            });
            return sender;
        }
    }
}

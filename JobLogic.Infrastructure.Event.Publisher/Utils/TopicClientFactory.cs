using Microsoft.Azure.ServiceBus;
using System.Collections.Concurrent;

namespace JobLogic.Infrastructure.Event.Publisher
{
    public static class TopicClientFactory
    {
        static ConcurrentDictionary<string, TopicClient> _cacheClients = new ConcurrentDictionary<string, TopicClient>();
        public static TopicClient For(string sbns, string topicName)
        {
            var key = $"{sbns}|{topicName}";
            return _cacheClients.GetOrAdd(key, new TopicClient(sbns, topicName));
        }
    }
}

using JobLogic.Common.Models;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JobLogic.Infrastructure.ServiceBus
{
    public class TopicPublisher<TMessage> : ITopicPublisher<TMessage>
        where TMessage : BaseServiceBusMessage
    {
        private TopicClient topicClient;

        public TopicPublisher(TopicConfiguration configuration)
        {
            InitializeTopicClient(configuration);
        }

        private void InitializeTopicClient(TopicConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException("configuration");
            }

            if (string.IsNullOrWhiteSpace(configuration.ConnectionString))
            {
                throw new ArgumentNullException("ConnectionString");
            }

            if (string.IsNullOrWhiteSpace(configuration.TopicName))
            {
                throw new ArgumentNullException($"TopicName, TopicName {configuration.TopicName}");
            }

            topicClient = new TopicClient(configuration.ConnectionString, configuration.TopicName);
        }

        public Task SendMessageAsync(TMessage message, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            var brokeredMessage = new Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            })));

            brokeredMessage.SessionId = message.SessionId;

            if (message.DelayMessageInMinutes.HasValue && message.DelayMessageInMinutes.Value > 0)
            {
                brokeredMessage.ScheduledEnqueueTimeUtc = DateTime.UtcNow.AddMinutes(message.DelayMessageInMinutes.Value);
            }
            return topicClient.SendAsync(brokeredMessage);
        }
    }
}

using JobLogic.Common.Models;
using JobLogic.Infrastructure.ServiceBus;
using JobLogic.Infrastructure.Utilities;
using Microsoft.ServiceBus.Messaging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Test.Integration.Joblogic.Infrastructure.ServiceBus
{
    [TestClass]
    public class TopicPublisherTest : BaseServiceBusTest
    {
        [TestMethod]
        public async Task TestServiceBusWorkFlow_WorkCorrectly()
        {
            // Arrange
            var topicPublisher = CreateTopicPublisher();
            var message = CreateMessage();

            // Action
            await topicPublisher.SendMessageAsync(message);

            // Assert
            ReadMessageFromServiceBusAndVerify(message);
        }

        private void VerifyMessage(BaseServiceBusMessage source, BaseServiceBusMessage target)
        {
            Assert.AreEqual(source.WorkflowId, target.WorkflowId);
            Assert.AreEqual(source.EntityId, target.EntityId);
        }

        private BaseServiceBusMessage CreateMessage()
        {
            return new BaseServiceBusMessage()
            {
                WorkflowId = Guid.NewGuid(),
                EntityId = Guid.NewGuid(),
            };
        }

        private void ReadMessageFromServiceBusAndVerify(BaseServiceBusMessage sentMessage)
        {
            bool waitingFlag = true;

            var subscriptionClient = SubscriptionClient.CreateFromConnectionString(ServiceBusConnectionString, topicName, topicSubscriptionName);
            subscriptionClient.OnMessage((brokeredMessage) =>
            {
                var body = brokeredMessage.GetBody<String>();
                var gettingMessage = JsonSerialization.Deserialize<BaseServiceBusMessage>(body);
                VerifyMessage(sentMessage, gettingMessage);
                waitingFlag = false;
            });

            // Wait here so that subscriptionClient tries to get message from service bus and compare it with source
            int count = 0;
            do
            {
                count++;
                // If waiting for 5 seconds
                if (count > 50)
                {
                    Assert.Fail("Could not get message from service bus.");
                }

                Thread.Sleep(100);
            } while (waitingFlag);
        }

        private TopicPublisher<BaseServiceBusMessage> CreateTopicPublisher()
        {
            var topicConfiguration = new TopicConfiguration
            {
                ConnectionString = ServiceBusConnectionString,
                TopicName = topicName
            };

            return new TopicPublisher<BaseServiceBusMessage>(topicConfiguration);
        }
    }
}

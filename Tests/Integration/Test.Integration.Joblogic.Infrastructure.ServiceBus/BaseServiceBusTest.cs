using Microsoft.ServiceBus;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Configuration;

namespace Test.Integration.Joblogic.Infrastructure.ServiceBus
{
    [TestCategory("Integration")]
    [TestClass]
    public class BaseServiceBusTest
    {
        protected NamespaceManager namespaceManager;
        protected string topicName;
        protected string topicSubscriptionName;
        protected string connectionString;

        [TestInitialize]
        public void BaseTestInitialize()
        {
            topicName = Guid.NewGuid().ToString();
            topicSubscriptionName = Guid.NewGuid().ToString();

            namespaceManager = GetNamespaceManager();
            CreateTopic(topicName);
            CreateTopicSubscription(topicName, topicSubscriptionName);
        }

        [TestCleanup]
        public void BaseTestCleanup()
        {
            if (!string.IsNullOrWhiteSpace(topicName))
            {
                namespaceManager.DeleteTopic(topicName);
            }
        }

        protected void CreateTopic(string topicName)
        {
            if (string.IsNullOrWhiteSpace(topicName))
            {
                Assert.Fail("Could not create topic because its name is empty");
            }

            namespaceManager.CreateTopic(topicName);
        }

        protected void CreateTopicSubscription(string topicName, string topicSubscriptionName)
        {
            if (string.IsNullOrWhiteSpace(topicName))
            {
                Assert.Fail("Could not create topic subscription because the topic name is empty");
            }

            if (string.IsNullOrWhiteSpace(topicSubscriptionName))
            {
                Assert.Fail("Could not create topic subscription because its name is empty");
            }

            namespaceManager.CreateSubscription(topicName, topicSubscriptionName);
        }

        private NamespaceManager GetNamespaceManager()
        {
            TokenProvider tokenProvider = TokenProvider.CreateSharedAccessSignatureTokenProvider(TokenProviderName, TokenProviderKey);
            return new NamespaceManager(ServiceBusUri, tokenProvider);
        }

        private Uri ServiceBusUri
        {
            get
            {
                return ServiceBusEnvironment.CreateServiceUri("sb", NamespaceServiceBus, string.Empty);
            }
        }

        protected string ServiceBusConnectionString
        {
            get
            {
                return string.Format("Endpoint={0};SharedAccessKeyName={1};SharedAccessKey={2}", ServiceBusUri.OriginalString, TokenProviderName, TokenProviderKey);
            }
        }

        private string NamespaceServiceBus
        {
            get
            {
                return ConfigurationManager.AppSettings["NamespaceServiceBus"];
            }
        }

        private string TokenProviderName
        {
            get
            {
                return ConfigurationManager.AppSettings["TokenProviderName"];
            }
        }

        private string TokenProviderKey
        {
            get
            {
                return ConfigurationManager.AppSettings["TokenProviderKey"];
            }
        }
    }
}

using Audit.Core;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Text;

namespace JobLogic.Infrastructure.QueueAuditDataProvider.Provider
{
    public class QueueAuditDataProvider: AuditDataProvider
    {
        public string ServiceBusConnectionString { get; set; }
        public string QueueName { get; set; }
        IQueueClient _queueClient;

        private IQueueClient GetQueueClient()
        {
            if (_queueClient == null)
            {
                _queueClient = new QueueClient(ServiceBusConnectionString, QueueName);
            }
            return _queueClient;
        }
        public override object InsertEvent(AuditEvent auditEvent)
        {
            var auditEventStr = JsonConvert.SerializeObject(auditEvent);
            var message = new Message(Encoding.UTF8.GetBytes(auditEventStr));

            try
            {
                GetQueueClient().SendAsync(message);
            }
            catch (Exception ex)
            {
            }
            return 1;
        }
    }
}

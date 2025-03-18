using System;

namespace JobLogic.Common.Models
{
    public class BaseServiceBusMessage
    {
        public BaseServiceBusMessage()
        {
            WorkflowId = Guid.NewGuid();
        }
        
        public Guid WorkflowId { get; set; }

        public Guid TenantId { get; set; }
        public Guid UserId { get; set; }

        public Guid EntityId { get; set; }

        public int? DelayMessageInMinutes { get; set; }
        public string SessionId { get; set; }
        public string ParentId { get; set; }
        public string RootId { get; set; }
        public object AdditionalData { get; set; }
    }
}

using System;

namespace JobLogic.Infrastructure.Log
{
    public class ApplicationLog : BaseLog
    {
        public string ComponentName { get; set; }
        public string EventName { get; set; }
        public object Data { get; set; }
        public object ExtraInformation { get; set; }
        public Guid TenantId { get; set; }
        public Guid WorkflowId { get; set; }

        public ApplicationLog(Guid tenantId, string componentName, string eventName, object data, LogType type = LogType.Info, object extraInformation = null, Guid? workflowId = null)
        {
            TenantId = tenantId;
            ComponentName = componentName;
            EventName = eventName;
            Data = data;
            Type = type;
            WorkflowId = workflowId ?? Guid.Empty;
            ExtraInformation = extraInformation;
        }
    }
}

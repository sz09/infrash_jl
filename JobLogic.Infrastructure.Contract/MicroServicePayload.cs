using Newtonsoft.Json;
using System;
using System.Linq;

namespace JobLogic.Infrastructure.Contract
{
    public sealed class MicroservicePayload
    {
        [JsonConstructor]
        private MicroservicePayload() { }
        [JsonProperty]
        internal OperationInfo OperationInfo;
        [JsonProperty]
        public object Data { get; private set; }
        [JsonProperty]
        public string MessageSignature { get; private set; }

        public ITenancyOperationInfo ExtractTenancyOperationInfo()
        {
            if (!OperationInfo.HasValidTenantId)
            {
                throw new ContractException("Invalid TenantId");
            }
            return new OperationInfo(OperationInfo.OperationId, OperationInfo.CreationData, OperationInfo.MessageTravelLog, OperationInfo.RuntimeData, OperationInfo.ContextTenantId);
        }

        public ITenantlessOperationInfo ExtractTenantlessOperationInfo()
        {
            return new OperationInfo(OperationInfo.OperationId, OperationInfo.CreationData, OperationInfo.MessageTravelLog, OperationInfo.RuntimeData, null);
        }

        private static (string msgSignature, string msgLogId) ValidateMessage(OperationInfo operationInfo, object data)
        {
            var msgType = data.GetType();
            var msgSignature = msgType.FullName;
            var msgLogId = msgSignature.GetHashCode() + msgType.Name;
            var formatedTravelLog = $"|{operationInfo.MessageTravelLog}|";
            if (!string.IsNullOrEmpty(operationInfo.MessageTravelLog)
                && formatedTravelLog.Contains($"|{msgLogId}|"))
            {
                throw new ContractException("Circular messaging detected!");
            }
            return (msgSignature, msgLogId);
        }

        private static MicroservicePayload PrivateCreate(object operationInfo, object data, Guid? overwriteTenantId = null)
        {
            var opInfo = operationInfo as OperationInfo;
            if (opInfo == null)
                throw new ContractException("Invalid OperationInfo type");
            var rs = ValidateMessage(opInfo, data);



            Guid? contextTenantId = opInfo.ContextTenantId;
            if (overwriteTenantId.HasValue)
            {
                if (opInfo.ContextTenantId.HasValue)
                    throw new ContractException("TenantId already exists");
                contextTenantId = overwriteTenantId.Value;
            }
            var messageTravelLog = AppendMessageTravelLog(opInfo.MessageTravelLog, rs.msgLogId);
            var rsOpInfo = new OperationInfo(opInfo.OperationId, opInfo.CreationData, messageTravelLog, opInfo.RuntimeData, contextTenantId);

            return new MicroservicePayload
            {
                Data = data,
                MessageSignature = rs.msgSignature,
                OperationInfo = rsOpInfo
            };
        }

        static string AppendMessageTravelLog(string messageTravelLog, string msgLogId)
        {
            messageTravelLog += string.IsNullOrEmpty(messageTravelLog) ? msgLogId : $"|{msgLogId}";
            return messageTravelLog;
        }

        public static MicroservicePayload Create(ITenantlessOperationInfo operationInfo, TenancyMsg data, Guid overwriteTenantId) => PrivateCreate(operationInfo, data, overwriteTenantId);
        public static MicroservicePayload Create(ITenantlessOperationInfo operationInfo, TenancyEvent data, Guid overwriteTenantId) => PrivateCreate(operationInfo, data, overwriteTenantId);
        public static MicroservicePayload Create(ITenantlessOperationInfo operationInfo, TenantlessMsg data) => PrivateCreate(operationInfo, data);
        public static MicroservicePayload Create(ITenantlessOperationInfo operationInfo, TenantlessEvent data) => PrivateCreate(operationInfo, data);
        public static MicroservicePayload Create(ITenancyOperationInfo operationInfo, TenantlessMsg data) => PrivateCreate(operationInfo, data);
        public static MicroservicePayload Create(ITenancyOperationInfo operationInfo, TenancyMsg data) => PrivateCreate(operationInfo, data);
        public static MicroservicePayload Create(ITenancyOperationInfo operationInfo, TenancyEvent data) => PrivateCreate(operationInfo, data);
        public static MicroservicePayload Create(ITenancyOperationInfo operationInfo, TenantlessEvent data) => PrivateCreate(operationInfo, data);

    }
}

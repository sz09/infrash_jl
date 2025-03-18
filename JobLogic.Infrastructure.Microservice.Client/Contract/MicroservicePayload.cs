using Newtonsoft.Json;
using System;

namespace JobLogic.Infrastructure.Microservice.Client
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

        internal static void ThrowWhenCircularMessaging(string messageTravelLog, string msgLogId, int allowedCircularTime)
        {
            if (string.IsNullOrEmpty(messageTravelLog)) return;

            var formatedTravelLog = $"|{messageTravelLog}|";
            if (formatedTravelLog.IndexOfOccurence($"|{msgLogId}|", allowedCircularTime) >= 0)
            {
                throw new ContractException("Circular messaging detected!");
            }
        }

        private static MicroservicePayload PrivateCreate(object operationInfo, BaseMicroserviceEvent data, Guid? overwriteTenantId = null)
        {
            return DoPrivateCreate(operationInfo,data, 3, overwriteTenantId);
        }

        private static MicroservicePayload PrivateCreate(object operationInfo, BaseMicroserviceMsg data, Guid? overwriteTenantId = null)
        {
            return DoPrivateCreate(operationInfo, data, 1 , overwriteTenantId);
        }

        private static MicroservicePayload DoPrivateCreate(object operationInfo, BaseMicroserviceData data, int allowedCircularTime, Guid? overwriteTenantId)
        {
            var opInfo = operationInfo as OperationInfo;
            if (opInfo == null)
                throw new ContractException("Invalid OperationInfo type");
            var msgLogId = data.GetMsgTravelLog();

            ThrowWhenCircularMessaging(opInfo.MessageTravelLog, msgLogId, allowedCircularTime);



            Guid? contextTenantId = opInfo.ContextTenantId;
            if (overwriteTenantId.HasValue)
            {
                if (opInfo.ContextTenantId.HasValue && opInfo.ContextTenantId.Value != overwriteTenantId.Value)
                    throw new ContractException("TenantId already exists");
                contextTenantId = overwriteTenantId.Value;
            }
            var messageTravelLog = AppendMessageTravelLog(opInfo.MessageTravelLog, msgLogId);
            var rsOpInfo = new OperationInfo(opInfo.OperationId, opInfo.CreationData, messageTravelLog, opInfo.RuntimeData, contextTenantId);

            return new MicroservicePayload
            {
                Data = data,
                MessageSignature = data.GetSignature(),
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

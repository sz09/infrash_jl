using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JobLogic.Infrastructure.Contract
{
    public interface ITenantlessOperationInfo
    {
        Guid OperationId { get; }
        IReadOnlyDictionary<string, string> CreationData { get; }
        string MessageTravelLog { get; }
        bool TryGetRuntimeData(string key, out string value);
    }
    public interface ITenancyOperationInfo : ITenantlessOperationInfo
    {
        Guid TenantId { get; }
    }
    class OperationInfo: ITenantlessOperationInfo, ITenancyOperationInfo
    {
        private OperationInfo() { }
        public OperationInfo(Guid operationId, IReadOnlyDictionary<string, string> creationData, string messageTravelLog, Dictionary<string, string> runtimeData, Guid? contextTenantId)
        {
            OperationId = operationId;
            CreationData = creationData?.ToDictionary(x => x.Key, x => x.Value) ?? new Dictionary<string, string>();
            MessageTravelLog = messageTravelLog;
            RuntimeData = runtimeData?.ToDictionary(x => x.Key, x => x.Value) ?? new Dictionary<string, string>();
            ContextTenantId = contextTenantId;
        }

        [JsonProperty]
        public Guid OperationId { get; private set; }
        [JsonProperty]
        public IReadOnlyDictionary<string, string> CreationData { get; private set; }
        [JsonProperty]
        public string MessageTravelLog { get; private set; }

        [JsonProperty]
        public Dictionary<string, string> RuntimeData { get; private set; }

        [JsonProperty]
        public Guid? ContextTenantId { get; private set; }

        [JsonIgnore]
        public Guid TenantId
        {
            get
            {
                if(!ContextTenantId.HasValue)
                {
                    throw new ContractException("ContextTenantId required! your current OperationInfo is meant for Tenantless App");
                }
                return ContextTenantId.Value;
            }
        }

        

        public bool TryGetRuntimeData(string key, out string value)
        {
            return RuntimeData.TryGetValue(key, out value);
        }

        private object LockObj_TrySetRuntimeData = new object();
        public bool TrySetRuntimeData(string key, string value, bool doOverwriteIfExist)
        {
            lock (LockObj_TrySetRuntimeData)
            {
                if (RuntimeData.ContainsKey(key) && doOverwriteIfExist == false)
                {
                    return false;
                }
                else
                {
                    RuntimeData[key] = value;
                    return true;
                }
            }
        }

        public bool HasValidTenantId => ContextTenantId.HasValue ? ContextTenantId.Value != Guid.Empty : false;

        
    }
}

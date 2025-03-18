using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace JobLogic.Infrastructure.Contract
{
    public static class OperationInfoFactory 
    {
        public static ITenancyOperationInfo CreateTenancy(Guid tenantId, IReadOnlyDictionary<string, string> creationData = null)
        {
            return new OperationInfo(Guid.NewGuid(), creationData, null, null, tenantId);

        }

        public static ITenantlessOperationInfo CreateTenantless(IReadOnlyDictionary<string, string> creationData = null)
        {
            return new OperationInfo(Guid.NewGuid(), creationData, null, null, null);
        }

        
    }
}

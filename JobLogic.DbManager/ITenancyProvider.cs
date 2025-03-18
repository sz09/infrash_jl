using System;

namespace JobLogic.DatabaseManager
{
    public interface ITenancyProvider
    {
        Guid TenantId { get; }
        string ConnectionString { get; }

        Guid LoginUserId { get; }
        string LoginUserName { get; }
    }


    public class MockJobLogicTenancyProvider : ITenancyProvider
    {
        public MockJobLogicTenancyProvider(string connectionString, Guid tenantId)
        {
            ConnectionString = connectionString;
            TenantId = tenantId;
        }

        public Guid TenantId
        {
            get;
            private set;
        }

        public string ConnectionString
        {
            get;
            private set;
        }

        public Guid LoginUserId { get; }
        public string LoginUserName { get; }
    }
}

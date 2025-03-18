using System;

namespace JobLogic.DatabaseManager
{
    public class DbManagerContext : IDisposable
    {
        protected DbExecutionManager _dbManager;
        protected Guid _tenantId;

        public DbManagerContext()
        {
        }

        public DbManagerContext(string ConnectionString, Guid? tenantId = null)
        {
            _dbManager = new DbExecutionManager(ConnectionString, tenantId);
            _tenantId = tenantId != null ? tenantId.Value : Guid.Empty;
        }

        public void Dispose()
        {
            _dbManager = null;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Infrastructure.Interception;
using System.Linq;
using System.Text;

namespace JobLogic.Infrastructure.Entityframework.TenancyInterceptor
{
    /// <summary>
    /// Uses Session context to apply tenant and deletion filter to queries executed against database
    /// </summary>
    /// <typeparam name="T">Some type which inherits from FilteredDbContext</typeparam>
    public class TenancySessionContextInterceptor : IDbConnectionInterceptor
    {
        private string Name { get; set; }
        public TenancySessionContextInterceptor(string name)
        {
            Name = name ?? string.Empty;
        }
        public override bool Equals(object obj)
        {
            if (obj == null || Name == null)
            {
                return false;
            }
            return this.Name.Equals((obj as TenancySessionContextInterceptor)?.Name);
        }
        public override int GetHashCode()
        {
            return (Name ?? string.Empty).GetHashCode();
        }
        public void Opened(DbConnection connection, DbConnectionInterceptionContext interceptionContext)
        {
            try
            {
                var jobLogicContext = interceptionContext.DbContexts.OfType<ITenancyDbContext>().FirstOrDefault();

                if (jobLogicContext != null)
                {
                    var tenantId = jobLogicContext.TenantId;
                    var isIncludedDeletedEntities = jobLogicContext.IncludedDeletedEntities;

                    DbCommand command = connection.CreateCommand();
                    command.CommandText = GetCommandStatement(isIncludedDeletedEntities);

                    DbParameter parameter = command.CreateParameter();
                    parameter.ParameterName = "@TenantId";
                    parameter.Value = tenantId;

                    command.Parameters.Add(parameter);
                    command.ExecuteNonQuery();
                }
            }
            catch
            {
                // If no user is logged in, leave SESSION_CONTEXT null (all rows will be filtered)
            }
        }

        private static string GetCommandStatement(bool isIncludedDeletedEntities) =>
            $@"EXEC sp_set_session_context 'TenantId', @TenantId; 
               EXEC sp_set_session_context 'ShowAll', { (isIncludedDeletedEntities ? 1 : 0) };";

        #region Unused Methods

        public void BeganTransaction(DbConnection connection, BeginTransactionInterceptionContext interceptionContext) { }

        public void BeginningTransaction(DbConnection connection, BeginTransactionInterceptionContext interceptionContext) { }

        public void Closed(DbConnection connection, DbConnectionInterceptionContext interceptionContext) { }

        public void Closing(DbConnection connection, DbConnectionInterceptionContext interceptionContext) { }

        public void ConnectionStringGetting(DbConnection connection, DbConnectionInterceptionContext<string> interceptionContext) { }

        public void ConnectionStringGot(DbConnection connection, DbConnectionInterceptionContext<string> interceptionContext) { }

        public void ConnectionStringSet(DbConnection connection, DbConnectionPropertyInterceptionContext<string> interceptionContext) { }

        public void ConnectionStringSetting(DbConnection connection, DbConnectionPropertyInterceptionContext<string> interceptionContext) { }

        public void ConnectionTimeoutGetting(DbConnection connection, DbConnectionInterceptionContext<int> interceptionContext) { }

        public void ConnectionTimeoutGot(DbConnection connection, DbConnectionInterceptionContext<int> interceptionContext) { }

        public void DatabaseGetting(DbConnection connection, DbConnectionInterceptionContext<string> interceptionContext) { }

        public void DatabaseGot(DbConnection connection, DbConnectionInterceptionContext<string> interceptionContext) { }

        public void DataSourceGetting(DbConnection connection, DbConnectionInterceptionContext<string> interceptionContext) { }

        public void DataSourceGot(DbConnection connection, DbConnectionInterceptionContext<string> interceptionContext) { }

        public void Disposed(DbConnection connection, DbConnectionInterceptionContext interceptionContext) { }

        public void Disposing(DbConnection connection, DbConnectionInterceptionContext interceptionContext) { }

        public void EnlistedTransaction(DbConnection connection, EnlistTransactionInterceptionContext interceptionContext) { }

        public void EnlistingTransaction(DbConnection connection, EnlistTransactionInterceptionContext interceptionContext) { }

        public void Opening(DbConnection connection, DbConnectionInterceptionContext interceptionContext) { }

        public void ServerVersionGetting(DbConnection connection, DbConnectionInterceptionContext<string> interceptionContext) { }

        public void ServerVersionGot(DbConnection connection, DbConnectionInterceptionContext<string> interceptionContext) { }

        public void StateGetting(DbConnection connection, DbConnectionInterceptionContext<ConnectionState> interceptionContext) { }

        public void StateGot(DbConnection connection, DbConnectionInterceptionContext<ConnectionState> interceptionContext) { }

        #endregion
    }
}

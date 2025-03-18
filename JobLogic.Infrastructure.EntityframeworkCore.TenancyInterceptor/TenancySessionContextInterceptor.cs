using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;

namespace JobLogic.Infrastructure.EntityframeworkCore.TenancyInterceptor
{
    /// <summary>
    /// Uses Session context to apply tenant and deletion filter to queries executed against database
    /// </summary>
    /// <typeparam name="T">Some type which inherits from FilteredDbContext</typeparam>
    public class TenancySessionContextInterceptor : DbConnectionInterceptor
    {        
        public override void ConnectionOpened(DbConnection connection, ConnectionEndEventData eventData)
        {
            var jobLogicContext = eventData.Context as ITenancyDbContext;

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

            base.ConnectionOpened(connection, eventData);
        }

        public override async Task ConnectionOpenedAsync(DbConnection connection, ConnectionEndEventData eventData, CancellationToken cancellationToken = default)
        {
            var jobLogicContext = eventData.Context as ITenancyDbContext;

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
                await command.ExecuteNonQueryAsync();
            }

            await base.ConnectionOpenedAsync(connection, eventData, cancellationToken);
        }

        private static string GetCommandStatement(bool isIncludedDeletedEntities) =>
            $@"EXEC sp_set_session_context 'TenantId', @TenantId; 
               EXEC sp_set_session_context 'ShowAll', {(isIncludedDeletedEntities ? 1 : 0)};";

    }
}

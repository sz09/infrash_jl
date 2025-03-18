using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;

namespace JobLogic.Infrastructure.Dapper
{
    public interface IDbExecutionDapper
    {
        Task<int> ExecuteAsync(SqlStatement sqlStatement, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null);
        Task<T> ExecuteReaderAsync<T>(SqlStatement sqlStatement, MapDbDataReaderDelg<T> mapDbDataReaderDelg, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null);
        Task<T> ExecuteScalarAsync<T>(SqlStatement sqlStatement, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null);
        Task<IEnumerable<T>> QueryAsync<T>(SqlStatement sqlStatement, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null);
        Task<T> QueryFirstAsync<T>(SqlStatement sqlStatement, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null);
        Task<T> QueryFirstOrDefaultAsync<T>(SqlStatement sqlStatement, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null);
        Task<T> QuerySingleAsync<T>(SqlStatement sqlStatement, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null);
        Task<T> QuerySingleOrDefaultAsync<T>(SqlStatement sqlStatement, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null);
    }

    public delegate Task<T> MapDbDataReaderDelg<T>(DbDataReader dbDataReader);

    public abstract class BaseDbExecutionDapper : IDbExecutionDapper
    {
        private readonly string _connectionString;
        private readonly Guid? _tenantId;

        public BaseDbExecutionDapper(string connectionString, Guid? tenantId = null)
        {
            _connectionString = connectionString;
            _tenantId = tenantId;
        }

        public async Task<T> ExecuteScalarAsync<T>(SqlStatement sqlStatement, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            await using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                await SetSessionContextAsync(connection, sqlStatement);
                var result = await connection.ExecuteScalarAsync<T>(sqlStatement.SQLStatementString, sqlStatement.SQLParams,
                    transaction, commandTimeout, commandType);
                return result;
            }
        }

        public async Task<int> ExecuteAsync(SqlStatement sqlStatement, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            await using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                await SetSessionContextAsync(connection, sqlStatement);
                var result = await connection.ExecuteAsync(sqlStatement.SQLStatementString, sqlStatement.SQLParams,
                    transaction, commandTimeout, commandType);
                return result;
            }

        }

        public async Task<IEnumerable<T>> QueryAsync<T>(SqlStatement sqlStatement, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            await using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                await SetSessionContextAsync(connection, sqlStatement);
                var result = await connection.QueryAsync<T>(sqlStatement.SQLStatementString, sqlStatement.SQLParams,
                    transaction, commandTimeout, commandType);
                return result;
            }

        }

        public async Task<T> ExecuteReaderAsync<T>(SqlStatement sqlStatement, MapDbDataReaderDelg<T> mapDbDataReaderDelg, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            await using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                await SetSessionContextAsync(connection, sqlStatement);
                await using (var dataReader = await connection.ExecuteReaderAsync(sqlStatement.SQLStatementString, sqlStatement.SQLParams,
                    transaction, commandTimeout, commandType))
                {
                    var rs = await mapDbDataReaderDelg(dataReader);
                    return rs;
                }
            }
        }

        public async Task<T> QueryFirstAsync<T>(SqlStatement sqlStatement, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            await using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                await SetSessionContextAsync(connection, sqlStatement);
                var result = await connection.QueryFirstAsync<T>(sqlStatement.SQLStatementString, sqlStatement.SQLParams,
                    transaction, commandTimeout, commandType);
                return result;
            }

        }

        public async Task<T> QueryFirstOrDefaultAsync<T>(SqlStatement sqlStatement, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            await using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                await SetSessionContextAsync(connection, sqlStatement);
                var result = await connection.QueryFirstOrDefaultAsync<T>(sqlStatement.SQLStatementString, sqlStatement.SQLParams,
                    transaction, commandTimeout, commandType);
                return result;
            }

        }

        public async Task<T> QuerySingleAsync<T>(SqlStatement sqlStatement, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            await using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                await SetSessionContextAsync(connection, sqlStatement);
                var result = await connection.QuerySingleAsync<T>(sqlStatement.SQLStatementString, sqlStatement.SQLParams,
                    transaction, commandTimeout, commandType);
                return result;
            }

        }

        public async Task<T> QuerySingleOrDefaultAsync<T>(SqlStatement sqlStatement, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            await using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                await SetSessionContextAsync(connection, sqlStatement);
                var result = await connection.QuerySingleOrDefaultAsync<T>(sqlStatement.SQLStatementString, sqlStatement.SQLParams,
                    transaction, commandTimeout, commandType);
                return result;
            }

        }

        const string SQL_TENANCY_ACCESS_STM = @"EXEC sp_set_session_context 'TenantId', @tenantId; ";
        const string SQL_SHOWALL_STM = @"EXEC sp_set_session_context 'ShowAll', 1; ";

        private async Task SetSessionContextAsync(SqlConnection connection, SqlStatement sqlStatement)
        {
            object? param = null;
            StringBuilder finalStmt = new StringBuilder();
            if (_tenantId.HasValue)
            {
                finalStmt.AppendLine(SQL_TENANCY_ACCESS_STM);
                param = new { tenantId = _tenantId.Value };
            }
            if (sqlStatement.DoShowAll)
            {
                finalStmt.AppendLine(SQL_SHOWALL_STM);
            }

            if (finalStmt.Length > 0)
            {
                await connection.ExecuteAsync(finalStmt.ToString(), param);
            }
        }
    }
}

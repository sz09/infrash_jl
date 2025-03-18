using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace JobLogic.DatabaseManager
{
    public interface IDbExecutionManager
    {
        #region For SqlBuilder
        Task<int> ExecNonQueryWithSqlBuilderAsync(BaseSqlBuilder sqlBuilder, CancellationToken cancellationToken = default);
        Task<int> ExecNonQueryWithSqlBuilderAsync(BaseSqlBuilder sqlBuilder, CommandType type = CommandType.Text, int Timeout = 0, CancellationToken cancellationToken = default);
        Task<T> ExecScalarWithSqlBuilderAsync<T>(BaseSqlBuilder sqlBuilder, CancellationToken cancellationToken = default);
        Task<T> ExecSqlBuilderGetSingleAsync<T>(BaseSqlBuilder sqlBuilder, Func<IDataReader, T> cd, CancellationToken cancellationToken = default);
        Task<List<T>> ExecSqlBuilderGetListAsync<T>(BaseSqlBuilder sqlBuilder, Func<IDataReader, T> cd, CancellationToken cancellationToken = default);
        Task<DataTable> ExecSqlBuilderGetDataTableAsync(BaseSqlBuilder sqlBuilder, int Timeout = 90, CancellationToken cancellationToken = default);
        #endregion

        [Obsolete("Should use Async version instead")]
        int ExecuteNonQuery(string sql, params IDbDataParameter[] parameters);
        Task<int> ExecuteNonQueryAsync(string sql, params IDbDataParameter[] parameters);

        [Obsolete("Should use Async version instead")]
        int ExecuteNonQuery(string sql, CommandType type = CommandType.Text, int Timeout = 0, params IDbDataParameter[] parameters);
        Task<int> ExecuteNonQueryAsync(string sql, CommandType type = CommandType.Text, int Timeout = 0, CancellationToken cancellationToken = default, params IDbDataParameter[] parameters);

        [Obsolete("Should use Async version instead")]
        T ExecuteScalar<T>(string sql, params IDbDataParameter[] parameters);
        Task<T> ExecuteScalarAsync<T>(string sql, params IDbDataParameter[] parameters);

        [Obsolete("Should use Async version instead")]
        T GetObject<T>(string sql, Func<IDataReader, T> cd, params IDbDataParameter[] parameters);
        Task<T> GetObjectAsync<T>(string sql, Func<IDataReader, T> cd, CancellationToken cancellationToken = default, params IDbDataParameter[] parameters);

        [Obsolete("Should use Async version instead")]
        IEnumerable<T> GetObjectList<T>(string sql, Func<IDataReader, T> cd, params IDbDataParameter[] parameters);
        Task<IEnumerable<T>> GetObjectListAsync<T>(string sql, Func<IDataReader, T> cd, CancellationToken cancellationToken = default, params IDbDataParameter[] parameters);

        [Obsolete("Should use Async version instead")]
        PagedList<T> GetObjectList<T>(SqlSelectBuilder sql, Func<IDataReader, T> cd, params IDbDataParameter[] parameters);
        Task<PagedList<T>> GetObjectListAsync<T>(SqlSelectBuilder sql, Func<IDataReader, T> cd, CancellationToken cancellationToken = default, params IDbDataParameter[] parameters);
        [Obsolete("Should use Async version instead")]
        DataSet GetDataTable(string sql, params IDbDataParameter[] parameters);
        [Obsolete("Should use Async version instead")]
        DataSet GetDataTable(string sql, int Timeout = 90, params IDbDataParameter[] parameters);
        Task<DataTable> GetDataTableAsync(string sql, int Timeout = 90, CancellationToken cancellationToken = default, params IDbDataParameter[] parameters);
        TransactionScope CreateTransactionScope();
        IDbCommand CreateCommand(string sql, CommandType type, int Timeout, IDbConnection connection, params IDbDataParameter[] parameters);
        Task<SqlConnection> CreateConnectionAsync();
    }

    public class DbExecutionManager : IDbExecutionManager
    {
        private const CommandType DEFAULT_COMMAND_TYPE = CommandType.Text;
        private readonly string _connectionString;
        private Guid? _tenantId;

        public DbExecutionManager(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DbExecutionManager(ITenancyProvider tenancyProvider)
        {
            _connectionString = tenancyProvider.ConnectionString;
            _tenantId = tenancyProvider.TenantId;
        }

        public DbExecutionManager(string connectionString, Guid? tenantId)
        {
            _connectionString = connectionString;
            _tenantId = tenantId;
        }

        #region For SqlBuilder
        public async Task<int> ExecNonQueryWithSqlBuilderAsync(BaseSqlBuilder sqlBuilder, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var sqlQuery = sqlBuilder.CreateSqlQuery();

            int response;
            using (var connection = await CreateConnectionAsync())
            {
                using (var transaction = connection.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                using (var command = new SqlCommand(sqlQuery.QueryString, connection, transaction))
                {
                    command.CommandTimeout = 30;

                    try
                    {
                        if (sqlQuery.QueryParams != null)
                            foreach (var parm in sqlQuery.QueryParams)
                                command.Parameters.Add(parm);

                        response = await command.ExecuteNonQueryAsync();
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                    finally
                    {
                        command.Parameters.Clear();
                    }
                }
            }
            return response;
        }

        public async Task<int> ExecNonQueryWithSqlBuilderAsync(BaseSqlBuilder sqlBuilder, CommandType type = CommandType.Text, int Timeout = 0, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var sqlQuery = sqlBuilder.CreateSqlQuery();

            using (var connection = await CreateConnectionAsync())
            {
                using (var command = (SqlCommand)CreateCommand(sqlQuery.QueryString, type, Timeout, connection, sqlQuery.QueryParams.ToArray()))
                {
                    int res = await command.ExecuteNonQueryAsync();
                    command.Parameters.Clear();
                    return res;
                }
            }
        }

        public async Task<T> ExecScalarWithSqlBuilderAsync<T>(BaseSqlBuilder sqlBuilder, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var sqlQuery = sqlBuilder.CreateSqlQuery();

            using (var connection = await CreateConnectionAsync())
            {
                using (var command = new SqlCommand(sqlQuery.QueryString, connection))
                {
                    command.CommandTimeout = 30;

                    if (sqlQuery.QueryParams != null)
                        foreach (var parm in sqlQuery.QueryParams)
                            command.Parameters.Add(parm);

                    var response = await command.ExecuteScalarAsync();
                    command.Parameters.Clear();

                    if (response == null || (response == DBNull.Value))
                        return default(T);
                    return (T)response;
                }
            }
        }

        public async Task<T> ExecSqlBuilderGetSingleAsync<T>(BaseSqlBuilder sqlBuilder, Func<IDataReader, T> cd, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var sqlQuery = sqlBuilder.CreateSqlQuery();
            T t = default;

            using (var connection = await CreateConnectionAsync())
            {
                using (var command = (SqlCommand)CreateCommand(sqlQuery.QueryString, DEFAULT_COMMAND_TYPE, 0, connection, sqlQuery.QueryParams.ToArray()))
                {
                    using (var dr = await command.ExecuteReaderAsync())
                    {
                        if (await dr.ReadAsync())
                            t = cd(dr);
                    }
                    command.Parameters.Clear();
                }
            }

            return t;
        }

        public async Task<List<T>> ExecSqlBuilderGetListAsync<T>(BaseSqlBuilder sqlBuilder, Func<IDataReader, T> cd, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var sqlQuery = sqlBuilder.CreateSqlQuery();
            var result = new List<T>();

            using (var connection = await CreateConnectionAsync())
            {
                using (var command = (SqlCommand)CreateCommand(sqlQuery.QueryString, DEFAULT_COMMAND_TYPE, 0, connection, sqlQuery.QueryParams.ToArray()))
                {
                    using (var dr = await command.ExecuteReaderAsync())
                    {
                        while (await dr.ReadAsync())
                        {
                            result.Add(cd(dr));
                        }

                        dr.Close();
                    }
                    command.Parameters.Clear();
                }
            }

            return result;
        }

        public async Task<DataTable> ExecSqlBuilderGetDataTableAsync(BaseSqlBuilder sqlBuilder, int Timeout = 90, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var sqlQuery = sqlBuilder.CreateSqlQuery();
            var columns = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

            using (var connection = await CreateConnectionAsync())
            {
                using (var command = (SqlCommand)CreateCommand(sqlQuery.QueryString, DEFAULT_COMMAND_TYPE, Timeout, connection, sqlQuery.QueryParams.ToArray()))
                {
                    var resultTable = new DataTable();
                    var cancellationSource = new CancellationTokenSource();
                    cancellationSource.Token.ThrowIfCancellationRequested();

                    var task = Task.Run(async () =>
                    {
                        using (var dr = await command.ExecuteReaderAsync(cancellationSource.Token))
                        {
                            var schemaTable = dr.GetSchemaTable();

                            foreach (DataRow dataRow in schemaTable.Rows)
                            {
                                var columnName = dataRow["ColumnName"].ToString();
                                if (columns.ContainsKey(columnName))
                                {
                                    var count = columns[columnName];
                                    columnName = $"{columnName}{count}";
                                    columns[columnName] = count + 1;
                                }
                                else
                                {
                                    columns.Add(columnName, 1);
                                }
                                var dataColumn = new DataColumn
                                {
                                    ColumnName = columnName,
                                    DataType = Type.GetType(dataRow["DataType"].ToString()),
                                    ReadOnly = (bool)dataRow["IsReadOnly"],
                                    AutoIncrement = (bool)dataRow["IsAutoIncrement"],
                                    Unique = (bool)dataRow["IsUnique"]
                                };

                                resultTable.Columns.Add(dataColumn);
                            }
                            while (await dr.ReadAsync(cancellationSource.Token))
                            {
                                var dataRow = resultTable.NewRow();
                                for (int i = 0; i < resultTable.Columns.Count; i++)
                                {
                                    dataRow[i] = dr[i];
                                }
                                resultTable.Rows.Add(dataRow);
                            }
                        }
                        command.Parameters.Clear();
                    }, cancellationSource.Token);
                    if (!task.Wait(TimeSpan.FromSeconds(Timeout > 0 ? Timeout : 30)))
                    {
                        cancellationSource.Cancel();
                        throw new TimeoutException();
                    }
                    return resultTable;
                }
            }
        }

        #endregion

        #region ExecuteNonQuery

        [Obsolete("Should use Async version instead")]
        public int ExecuteNonQuery(string sql, params IDbDataParameter[] parameters)
        {
            return ExecuteNonQuery(sql, DEFAULT_COMMAND_TYPE, 0, parameters);
        }

        /// <summary>
        /// Execute query asynchronously within a transaction.
        /// Executes transaction at an isolation level of ReadCommitted.
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<int> ExecuteNonQueryAsync(string sqlQuery, params IDbDataParameter[] parameters)
        {
            int response;
            using (var connection = await CreateConnectionAsync())
            {
                using (var transaction = connection.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                using (var command = new SqlCommand(sqlQuery, connection, transaction))
                {
                    command.CommandTimeout = 30;

                    try
                    {
                        if (parameters != null)
                            foreach (var parm in parameters)
                                command.Parameters.Add(parm);

                        response = await command.ExecuteNonQueryAsync();
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                    finally
                    {
                        command.Parameters.Clear();
                    }
                }
            }
            return response;
        }

        [Obsolete("Should use Async version instead")]
        public int ExecuteNonQuery(string sql, CommandType type = DEFAULT_COMMAND_TYPE, int Timeout = 0, params IDbDataParameter[] parameters)
        {
            using (var connection = CreateConnection())
            {
                using (var command = CreateCommand(sql, type, Timeout, connection, parameters))
                {
                    int res = command.ExecuteNonQuery();
                    command.Parameters.Clear();
                    return res;
                }
            }
        }

        public async Task<int> ExecuteNonQueryAsync(string sql, CommandType type = CommandType.Text, int Timeout = 0, CancellationToken cancellationToken = default, params IDbDataParameter[] parameters)
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (var connection = await CreateConnectionAsync())
            {
                using (var command = (SqlCommand)CreateCommand(sql, type, Timeout, connection, parameters))
                {
                    int res = await command.ExecuteNonQueryAsync();
                    command.Parameters.Clear();
                    return res;
                }
            }
        }
        #endregion

        #region ExecuteScalar

        [Obsolete("Should use Async version instead")]
        public T ExecuteScalar<T>(string sql, params IDbDataParameter[] parameters)
        {
            return ExecuteScalar<T>(sql, DEFAULT_COMMAND_TYPE, 0, parameters);
        }

        public T ExecuteScalar<T>(string sql, CommandType type = DEFAULT_COMMAND_TYPE, int Timeout = 0, params IDbDataParameter[] parameters)
        {
            using (var connection = CreateConnection())
            {
                using (var command = CreateCommand(sql, type, Timeout, connection, parameters))
                {
                    var res = command.ExecuteScalar();
                    command.Parameters.Clear();
                    if ((res == null) || (res == DBNull.Value))
                        return default(T);
                    return (T)res;
                }
            }
        }

        /// <summary>
        /// Executes a scalar operation asynchronously.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlQuery"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<T> ExecuteScalarAsync<T>(string sqlQuery, params IDbDataParameter[] parameters)
        {
            using (var connection = await CreateConnectionAsync())
            {
                using (var command = new SqlCommand(sqlQuery, connection))
                {
                    command.CommandTimeout = 30;

                    if (parameters != null)
                        foreach (var parm in parameters)
                            command.Parameters.Add(parm);

                    var response = await command.ExecuteScalarAsync();
                    command.Parameters.Clear();

                    if (response == null || (response == DBNull.Value))
                        return default(T);
                    return (T)response;
                }
            }
        }

        #endregion

        #region GetObject
        private T GetObject<T>(string sql, Func<IDataReader, T> cd, CommandType type = DEFAULT_COMMAND_TYPE, int Timeout = 0, T t = default(T), params IDbDataParameter[] parameters)
        {
            using (var connection = CreateConnection())
            {
                using (var command = CreateCommand(sql, type, Timeout, connection, parameters))
                {
                    using (var dr = command.ExecuteReader())
                    {
                        if (dr.Read())
                            t = cd(dr);
                    }
                    command.Parameters.Clear();
                }
            }
            return t;
        }

        [Obsolete("Should use Async version instead")]
        public T GetObject<T>(string sql, Func<IDataReader, T> cd, params IDbDataParameter[] parameters)
        {
            return GetObject(sql, cd, DEFAULT_COMMAND_TYPE, 0, parameters: parameters);
        }

        public async Task<T> GetObjectAsync<T>(string sql, Func<IDataReader, T> cd, CancellationToken cancellationToken = default, params IDbDataParameter[] parameters)
        {
            cancellationToken.ThrowIfCancellationRequested();

            T t = default;
            using (var connection = await CreateConnectionAsync())
            {
                using (var command = (SqlCommand)CreateCommand(sql, DEFAULT_COMMAND_TYPE, 0, connection, parameters))
                {
                    using (var dr = await command.ExecuteReaderAsync())
                    {
                        if (await dr.ReadAsync())
                            t = cd(dr);
                    }
                    command.Parameters.Clear();
                }
            }

            return t;
        }

        #endregion


        #region GetObjectList
        private IEnumerable<T> GetObjectList<T>(string sql, Func<IDataReader, T> cd, CommandType type = DEFAULT_COMMAND_TYPE, int Timeout = 0, params IDbDataParameter[] parameters)
        {
            using (var connection = CreateConnection())
            {
                using (var command = CreateCommand(sql, type, Timeout, connection, parameters))
                {
                    using (var dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            yield return cd(dr);
                        }
                        dr.Close();
                    }
                    command.Parameters.Clear();
                }
            }
        }

        [Obsolete("Should use Async version instead")]
        public IEnumerable<T> GetObjectList<T>(string sql, Func<IDataReader, T> cd, params IDbDataParameter[] parameters)
        {
            return GetObjectList(sql, cd, DEFAULT_COMMAND_TYPE, 0, parameters: parameters);
        }

        public async Task<IEnumerable<T>> GetObjectListAsync<T>(string sql, Func<IDataReader, T> cd, CancellationToken cancellationToken = default, params IDbDataParameter[] parameters)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var result = new List<T>();
            using (var connection = await CreateConnectionAsync())
            {
                using (var command = (SqlCommand)CreateCommand(sql, DEFAULT_COMMAND_TYPE, 0, connection, parameters))
                {
                    using (var dr = await command.ExecuteReaderAsync())
                    {
                        while (await dr.ReadAsync())
                        {
                            result.Add(cd(dr));
                        }

                        dr.Close();
                    }
                    command.Parameters.Clear();
                }
            }

            return result;
        }

        [Obsolete("Should use Async version instead")]
        public PagedList<T> GetObjectList<T>(SqlSelectBuilder sql, Func<IDataReader, T> cd, params IDbDataParameter[] parameters)
        {
            using (var connection = CreateConnection())
            {
                using (var _command = new SqlCommand())
                {
                    _command.Connection = connection;
                    _command.CommandType = CommandType.Text;

                    if (parameters != null)
                        foreach (var parm in parameters)
                            _command.Parameters.Add(parm);
                    _command.CommandText = sql.GetSqlCount();

                    int totalResultsCount = (int)_command.ExecuteScalar();

                    var pagedList = new PagedList<T>(sql.PageIndex, sql.PageSize, totalResultsCount);
                    if (totalResultsCount == 0)
                        return pagedList;

                    _command.CommandText = sql.GetPagedSql();
                    using (var dr = _command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            pagedList.Add(cd(dr));
                        }
                        dr.Close();
                    }
                    _command.Parameters.Clear();

                    return pagedList;
                }
            }
        }

        public async Task<PagedList<T>> GetObjectListAsync<T>(SqlSelectBuilder sql, Func<IDataReader, T> cd, CancellationToken cancellationToken = default, params IDbDataParameter[] parameters)
        {
            cancellationToken.ThrowIfCancellationRequested();

            PagedList<T> pagedList = null;
            using (var connection = await CreateConnectionAsync())
            {
                using (var _command = new SqlCommand())
                {
                    _command.Connection = connection;
                    _command.CommandType = CommandType.Text;

                    if (parameters != null)
                    {
                        foreach (var parm in parameters)
                            _command.Parameters.Add(parm);
                    }

                    _command.CommandText = sql.GetSqlCount();
                    int totalResultsCount = (int)await _command.ExecuteScalarAsync();

                    pagedList = new PagedList<T>(sql.PageIndex, sql.PageSize, totalResultsCount);
                    if (totalResultsCount == 0)
                        return pagedList;

                    _command.CommandText = sql.GetPagedSql();
                    using (var dr = await _command.ExecuteReaderAsync())
                    {
                        while (await dr.ReadAsync())
                        {
                            pagedList.Add(cd(dr));
                        }

                        dr.Close();
                    }
                    _command.Parameters.Clear();
                }
            }

            return pagedList;
        }

        #endregion


        #region GetDataTable
        private DataSet GetDataTable(string sql, CommandType type = DEFAULT_COMMAND_TYPE, int Timeout = 0, params IDbDataParameter[] parameters)
        {
            var dataset = new DataSet();
            using (var connection = CreateConnection())
            {
                using (var command = CreateCommand(sql, type, Timeout, connection, parameters))
                {
                    var dataadapter = GetConnectionDataAdapter();
                    dataadapter.SelectCommand = command;
                    dataadapter.Fill(dataset);
                    command.Parameters.Clear();
                }
            }
            return dataset;
        }

        public DataSet GetDataTable(string sql, int Timeout = 90, params IDbDataParameter[] parameters)
        {
            return GetDataTable(sql, DEFAULT_COMMAND_TYPE, Timeout, parameters);
        }

        public DataSet GetDataTable(string sql, params IDbDataParameter[] parameters)
        {
            return GetDataTable(sql, DEFAULT_COMMAND_TYPE, 90, parameters);
        }


        public async Task<DataTable> GetDataTableAsync(string sql, int Timeout = 90, CancellationToken cancellationToken = default, params IDbDataParameter[] parameters)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var columns = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            using (var connection = await CreateConnectionAsync())
            {
                using (var command = (SqlCommand)CreateCommand(sql, DEFAULT_COMMAND_TYPE, Timeout, connection, parameters))
                {
                    var resultTable = new DataTable();
                    var cancellationSource = new CancellationTokenSource();
                    cancellationSource.Token.ThrowIfCancellationRequested();

                    var task = Task.Run(async () =>
                    {
                        using (var dr = await command.ExecuteReaderAsync(cancellationSource.Token))
                        {
                            var schemaTable = dr.GetSchemaTable();

                            foreach (DataRow dataRow in schemaTable.Rows)
                            {
                                var columnName = dataRow["ColumnName"].ToString();
                                if (columns.ContainsKey(columnName))
                                {
                                    var count = columns[columnName];
                                    columnName = $"{columnName}{count}";
                                    columns[columnName] = count + 1;
                                }
                                else
                                {
                                    columns.Add(columnName, 1);
                                }
                                var dataColumn = new DataColumn
                                {
                                    ColumnName = columnName,
                                    DataType = Type.GetType(dataRow["DataType"].ToString()),
                                    ReadOnly = (bool)dataRow["IsReadOnly"],
                                    AutoIncrement = (bool)dataRow["IsAutoIncrement"],
                                    Unique = (bool)dataRow["IsUnique"]
                                };

                                resultTable.Columns.Add(dataColumn);
                            }
                            while (await dr.ReadAsync(cancellationSource.Token))
                            {
                                var dataRow = resultTable.NewRow();
                                for (int i = 0; i < resultTable.Columns.Count; i++)
                                {
                                    dataRow[i] = dr[i];
                                }
                                resultTable.Rows.Add(dataRow);
                            }
                        }
                        command.Parameters.Clear();
                    }, cancellationSource.Token);
                    if (!task.Wait(TimeSpan.FromSeconds(Timeout > 0 ? Timeout : 30)))
                    {
                        cancellationSource.Cancel();
                        throw new TimeoutException();
                    }
                    return resultTable;
                }
            }
        }
        #endregion

        private IDbDataAdapter GetConnectionDataAdapter()
        {
            return new SqlDataAdapter();
        }

        private SqlConnection CreateConnection()
        {
            var connection = new SqlConnection(_connectionString);
            connection.Open();

            if (_tenantId.HasValue)
                SetSessionContextForMultiTenant(connection);

            return connection;
        }

        /// <summary>
        /// Creates and opens an asynchronous SqlConnection.
        /// </summary>
        /// <returns></returns>
        public async Task<SqlConnection> CreateConnectionAsync()
        {
            var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            if (_tenantId.HasValue)
                await SetSessionContextForMultiTenantAsync(connection);

            return connection;
        }

        private void SetSessionContextForMultiTenant(SqlConnection connection)
        {
            try
            {
                var sql = $"EXEC sp_set_session_context 'TenantId', @tenantId; ";
                using (var cmd = new SqlCommand(sql, connection))
                {
                    cmd.Parameters.Add(_tenantId.AsParam("@tenantId"));
                    cmd.ExecuteNonQuery();
                }
            }
            catch
            {
                connection.Close();
                throw;
            }
        }

        /// <summary>
        /// Set multi tenant session asynchronously.
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        private async Task SetSessionContextForMultiTenantAsync(SqlConnection connection)
        {
            try
            {
                const string sqlQuery = "EXEC sp_set_session_context \'TenantId\', @tenantId; ";
                using (var command = new SqlCommand(sqlQuery, connection))
                {
                    command.Parameters.Add(_tenantId.AsParam("@tenantId"));
                    await command.ExecuteNonQueryAsync();
                }
            }
            catch
            {
                connection.Close();
                throw;
            }
        }

        public IDbCommand CreateCommand(string sql, CommandType type, int Timeout, IDbConnection connection, params IDbDataParameter[] parameters)
        {
            IDbCommand _command = new SqlCommand();
            _command.CommandText = sql;
            _command.Connection = connection;
            _command.CommandType = type;
            if (Timeout > 0) _command.CommandTimeout = Timeout;
            else _command.CommandTimeout = 30;

            if (parameters != null)
                foreach (var parm in parameters)
                    _command.Parameters.Add(parm);

            return _command;
        }

        public TransactionScope CreateTransactionScope()
        {
            var transactionOptions = new TransactionOptions
            {
                IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted,
                Timeout = TransactionManager.MaximumTimeout
            };
            return new TransactionScope();// TransactionScopeOption.Required, transactionOptions);
        }
    }
}

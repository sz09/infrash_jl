using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace JobLogic.Infrastructure.Azure.Storage
{
    public interface IAzureTableStorage
    {
        Task<IEnumerable<TEntity>> GetByAsync<TEntity>(string tableName, QueryTableCondition condition, CancellationToken cancellationToken = default) where TEntity : TableEntity, new();

        Task<IEnumerable<TEntity>> GetAllAsync<TEntity>(string tableName, CancellationToken cancellationToken = default) where TEntity : TableEntity, new();

        Task<TableResult> AddRowAsync<TEntity>(string tableName, TEntity entity, CancellationToken cancellationToken = default) where TEntity : TableEntity, new();

        Task<bool> DeleteAsync(string tableName, CancellationToken cancellationToken = default);

        Task<bool> CreateAsync(string tableName, CancellationToken cancellationToken = default);
    }

    public class AzureTableStorage : IAzureTableStorage
    {
        private readonly CloudTableClient cloudTableClient;

        public AzureTableStorage(string storageAccountName, string accessKeyValue)
        {
            if (string.IsNullOrWhiteSpace(storageAccountName))
            {
                throw new ArgumentNullException(nameof(storageAccountName));
            }

            if (string.IsNullOrWhiteSpace(accessKeyValue))
            {
                throw new ArgumentNullException(nameof(accessKeyValue));
            }

            var cloudStorageAccount = new CloudStorageAccount(new StorageCredentials(storageAccountName, accessKeyValue), true);
            cloudTableClient = cloudStorageAccount.CreateCloudTableClient();
        }

        public async Task<IEnumerable<TEntity>> GetByAsync<TEntity>(string tableName, QueryTableCondition condition, CancellationToken cancellationToken = default) where TEntity : TableEntity, new()
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new ArgumentNullException(nameof(tableName));
            }

            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition));
            }

            if (string.IsNullOrWhiteSpace(condition.ColumnName))
            {
                throw new ArgumentNullException(nameof(condition.ColumnName));
            }

            var cloudTable = cloudTableClient.GetTableReference(tableName);

            var query = new TableQuery<TEntity>()
                    .Where(TableQuery.GenerateFilterCondition(condition.ColumnName, GetDescription(condition.Operation), condition.Value));

            return await cloudTable.ExecuteQuerySegmentedAsync(query, null);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync<TEntity>(string tableName, CancellationToken cancellationToken = default) where TEntity : TableEntity, new()
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new ArgumentNullException(nameof(tableName));
            }

            var cloudTable = cloudTableClient.GetTableReference(tableName);

            var query = new TableQuery<TEntity>();

            return await cloudTable.ExecuteQuerySegmentedAsync(query, null);
        }

        public Task<TableResult> AddRowAsync<TEntity>(string tableName, TEntity entity, CancellationToken cancellationToken = default) where TEntity : TableEntity, new()
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new ArgumentNullException(nameof(tableName));
            }

            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var cloudTable = cloudTableClient.GetTableReference(tableName);

            // Create the TableOperation that inserts the customer entity. 
            var insertOperation = TableOperation.Insert(entity);

            // Set uniqueid for 2 properties. It will throw exception if they are null
            entity.RowKey = Guid.NewGuid().ToString();
            entity.PartitionKey = Guid.NewGuid().ToString();

            return cloudTable.ExecuteAsync(insertOperation);
        }

        public Task<bool> DeleteAsync(string tableName, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new ArgumentNullException(nameof(tableName));
            }

            var cloudTable = cloudTableClient.GetTableReference(tableName);

            return cloudTable.DeleteIfExistsAsync();
        }

        public Task<bool> CreateAsync(string tableName, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new ArgumentNullException(nameof(tableName));
            }

            var cloudTable = cloudTableClient.GetTableReference(tableName);

            return cloudTable.CreateIfNotExistsAsync();
        }

        private static string GetDescription(Enum enumValue)
        {
            if (enumValue == null) return string.Empty;

            var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());
            if (fieldInfo == null) return string.Empty;

            object[] attr = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attr.Length > 0
                   ? ((DescriptionAttribute)attr[0]).Description
                   : enumValue.ToString();
        }
    }
}

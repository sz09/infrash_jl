using FluentAssertions;
using JobLogic.Infrastructure.Azure.Storage;
using JobLogic.Infrastructure.UnitTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace Tests.Integration.JobLogic.Infrastructure.Azure.Storage
{
    public class FakeTableEntity : TableEntity
    {
        public string Name { get; set; }
        public int HouseNumber { get; set; }
        public DateTime Birthday { get; set; }
        public bool IsMale { get; set; }
    }

    [TestClass]
    public class AzureTableStorageTest : BaseUnitTest
    {
        private const string TableNamePattern = "AzureTableStorageIntegrationTest{0}";
        private string tableName;
        private AzureTableStorage azureTableStorage;

        [TestInitialize]
        public void AzureTableStorageTestTestInitialize()
        {
            var storageAccountName = ConfigurationManager.AppSettings["StorageAccountName"];
            var storageAccountKey = ConfigurationManager.AppSettings["StorageAccountKey"];
            azureTableStorage = new AzureTableStorage(storageAccountName, storageAccountKey);
        }

        [TestCleanup]
        public async Task AzureTableStorageTestCleanup()
        {
            if (azureTableStorage != null && !string.IsNullOrWhiteSpace(tableName))
            {
                await azureTableStorage.DeleteAsync(tableName);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task TestGetBy_ReturnError_IfTableNameIsNull()
        {
            // Action
            var result = await azureTableStorage.GetByAsync<FakeTableEntity>(null, new QueryTableCondition());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task TestGetBy_ReturnError_IfTableNameIsEmpty()
        {
            // Action
            var result = await azureTableStorage.GetByAsync<FakeTableEntity>(string.Empty, new QueryTableCondition());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task TestGetBy_ReturnError_IfConditionIsNull()
        {
            // Action
            var result = await azureTableStorage.GetByAsync<FakeTableEntity>(ValueGenerator.Guid().ToString(), null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task TestGetBy_ReturnError_IfColumnNameIsNull()
        {
            // Arrange
            var queryTableCondition = ValueGenerator.BuildObject<QueryTableCondition>()
                .With(x => x.ColumnName, ()=> null)
                .Create();

            // Action
            var result = await azureTableStorage.GetByAsync<FakeTableEntity>(ValueGenerator.Guid().ToString(), queryTableCondition);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task TestGetBy_ReturnError_IfColumnNameIsEmpty()
        {
            // Arrange
            var queryTableCondition = ValueGenerator.BuildObject<QueryTableCondition>()
                .With(x => x.ColumnName, string.Empty)
                .Create();

            // Action
            var result = await azureTableStorage.GetByAsync<FakeTableEntity>(ValueGenerator.Guid().ToString(), queryTableCondition);
        }

        [TestMethod]
        public async Task TestGetBy_ReturnSuccessfully()
        {
            // Arrange
            await CreateTableInStorageAsync();

            var addedEmployee = await InsertFakeTableEntityIntoTableStorageAsync();

            var queryTableCondition = ValueGenerator.BuildObject<QueryTableCondition>()
                .With(x => x.ColumnName, "Name")
                .With(x=> x.Operation, ConditionComparison.Equal)
                .With(x => x.Value, addedEmployee.Name)
                .Create();

            // Action
            var result = await azureTableStorage.GetByAsync<FakeTableEntity>(tableName, queryTableCondition);

            // Assert
            result.Count().Should().Be(1);
            CompareFakeTableEntity(addedEmployee, result.ElementAt(0));
        }

        [TestMethod]
        public async Task TestGetBy_ReturnSuccessfully_IfConditionNotExist()
        {
            // Arrange
            await CreateTableInStorageAsync ();

            var addedEmployee = await InsertFakeTableEntityIntoTableStorageAsync();

            var queryTableCondition = ValueGenerator.BuildObject<QueryTableCondition>()
                .With(x => x.ColumnName, "Name")
                .With(x => x.Operation, ConditionComparison.Equal)
                .Create();

            // Action
            var result = await azureTableStorage.GetByAsync<FakeTableEntity>(tableName, queryTableCondition);

            // Assert
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task TestGetAll_ReturnError_IfTableNameIsNull()
        {
            // Action
            var result = await azureTableStorage.GetAllAsync<FakeTableEntity>(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task TestGetAll_ReturnError_IfTableNameIsEmpty()
        {
            // Action
            var result = await azureTableStorage.GetAllAsync<FakeTableEntity>(string.Empty);
        }

        [TestMethod]
        public async Task TestGetAll_ReturnSuccessfully()
        {
            // Arrange
            await CreateTableInStorageAsync ();

            var addedEmployees = new List<FakeTableEntity> {
                await InsertFakeTableEntityIntoTableStorageAsync(),
                await InsertFakeTableEntityIntoTableStorageAsync()
            };

            // Action
            var result = await azureTableStorage .GetAllAsync<FakeTableEntity>(tableName);

            // Assert
            result.Count().Should().BeGreaterOrEqualTo(addedEmployees.Count);
            foreach (var item in addedEmployees)
            {
                var returnedEmployee = result.First(x => x.Name == item.Name);
                CompareFakeTableEntity(item, returnedEmployee);
            }
        }

        [TestMethod]
        public async Task TestAddRow_Successfully()
        {
            // Arrange
            await CreateTableInStorageAsync();

            var addingEmployee = ValueGenerator.CreateObject<FakeTableEntity>();

            // Action
            var result = await azureTableStorage .AddRowAsync(tableName, addingEmployee);

            // Assert
            var queryResult = await azureTableStorage.GetAllAsync<FakeTableEntity>(tableName);
            CompareFakeTableEntity(addingEmployee, queryResult.First(x => x.Name == addingEmployee.Name));
        }

        private void CompareFakeTableEntity(FakeTableEntity source, FakeTableEntity target)
        {
            target.Name.Should().Be(source.Name);
            target.HouseNumber.Should().Be(source.HouseNumber);
            target.Birthday.Should().Be(source.Birthday.ToUniversalTime());
            target.IsMale.Should().Be(source.IsMale);
        }

        private async Task<FakeTableEntity> InsertFakeTableEntityIntoTableStorageAsync()
        {
            var employee = ValueGenerator.CreateObject<FakeTableEntity>();
            await azureTableStorage.AddRowAsync(tableName, employee);

            return employee;
        }

        private async Task CreateTableInStorageAsync()
        {
            tableName = string.Format(TableNamePattern, DateTime.UtcNow.Ticks.ToString());
            await azureTableStorage.CreateAsync(tableName);
        }
    }
}

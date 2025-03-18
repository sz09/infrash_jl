using JobLogic.DatabaseManager;

namespace Tests.Integration.JobLogic.DatabaseManager
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class DbExecutionManagerTest : BaseTest
    {
        [TestMethod]
        public void TestExecuteNonQuery_Successfull()
        {
            // Arrange
            var linkTypeName = Guid.NewGuid().ToString();
            var insertSql = string.Format(@"INSERT INTO [dbo].[Portal_LinkTypes]([LinkType]) VALUES('{0}')", linkTypeName);
            
            // Action
            var result = dbExecutionManager.ExecuteNonQuery(insertSql, null);

            // Assert
            Assert.AreEqual(1, result);
            var dataTable = dbExecutionManager.GetDataTable(string.Format("Select * from [dbo].[Portal_LinkTypes] where LinkType ='{0}'", linkTypeName));
            Assert.IsNotNull(dataTable);
            Assert.AreEqual(1, dataTable.Tables.Count);
            Assert.AreEqual(1, dataTable.Tables[0].Rows.Count);
            Assert.AreEqual(linkTypeName, dataTable.Tables[0].Rows[0][1]);
        }

        [TestMethod]
        public void TestExecuteScalarAsync_ReturnDefaultGuid()
        {
            // Arrange
            var emailAddress = "NotExistingEmail@gmail.com";
            var sql = @"SELECT c.CompanyGuidId as TenantId
                        FROM JobLogicWeb_Users jlwu WITH(NOLOCK)
                        INNER JOIN JLDatasets jld WITH(NOLOCK)
                            ON JlDatasetsId = jld.id
                        INNER JOIN dbo.Company c WITH(NOLOCK)
                            ON jld.CompanyID = c.Id
                        WHERE jlwu.EmailAddress = @EmailAddress";
             
            // Action
            var result = dbExecutionManager.ExecuteScalarAsync<Guid>(sql, emailAddress.AsParam<string>("EmailAddress")).Result;

            // Assert
            Assert.AreEqual(Guid.Empty, result);
            
        }
    }
}

using FluentAssertions;
using JobLogic.Infrastructure.Microservice.Client;
using JobLogic.Infrastructure.UnitTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests.Unit.JobLogic.Infrastructure.Contract
{
    [TestClass]
    public class OperationInfoFactoryTests
    {
        [TestMethod]
        public void TestCreateTenantless_ShouldWork()
        {
            //Arrange
            const string TestKey = "TestKey";
            string testValue = "Value";
            var creationData = new Dictionary<string, string>
            {
                { ValueGenerator.String(), ValueGenerator.String() },
                { TestKey, testValue }
            };

            //Action
            var operationInfo = OperationInfoFactory.CreateTenantless(creationData);

            //Assert
            operationInfo.CreationData.Should().BeEquivalentTo(creationData);
            operationInfo.MessageTravelLog.Should().BeNull();
            operationInfo.OperationId.Should().NotBeEmpty();
        }

        [TestMethod]
        public void TestCreateTenancy_ShouldWork()
        {
            //Arrange
            const string TestKey = "TestKey";
            string testValue = "Value";
            var creationData = new Dictionary<string, string>
            {
                { ValueGenerator.String(), ValueGenerator.String() },
                { TestKey, testValue }
            };
            var tenantId = Guid.NewGuid();
            //Action
            var operationInfo = OperationInfoFactory.CreateTenancy(tenantId, creationData);

            //Assert
            operationInfo.CreationData.Should().BeEquivalentTo(creationData);
            operationInfo.TenantId.Should().Be(tenantId);
            operationInfo.MessageTravelLog.Should().BeNull();
            operationInfo.OperationId.Should().NotBeEmpty();
        }

        [TestMethod]
        public void TestCreateTenantless_ShouldNotAffect_WhenInputCreationDataDictChange()
        {
            //Arrange
            const string TestKey = "TestKey";
            string testValue = "Value";
            var creationData = new Dictionary<string, string>
            {
                { ValueGenerator.String(), ValueGenerator.String() },
                { TestKey, testValue }
            };

            var operationInfo = OperationInfoFactory.CreateTenantless(creationData);

            //Action
            creationData[TestKey] = ValueGenerator.String();

            //Assert
            operationInfo.CreationData.Should().NotBeEquivalentTo(creationData);
            operationInfo.CreationData[TestKey].Should().Be(testValue);
        }

        [TestMethod]
        public void TestCreateTenancy_ShouldNotAffect_WhenInputCreationDataDictChange()
        {
            //Arrange
            const string TestKey = "TestKey";
            string testValue = "Value";
            var creationData = new Dictionary<string, string>
            {
                { ValueGenerator.String(), ValueGenerator.String() },
                { TestKey, testValue }
            };

            var operationInfo = OperationInfoFactory.CreateTenancy(Guid.NewGuid(), creationData);
            //Action
            creationData[TestKey] = ValueGenerator.String();

            //Assert
            operationInfo.CreationData.Should().NotBeEquivalentTo(creationData);
            operationInfo.CreationData[TestKey].Should().Be(testValue);

        }
    }
}

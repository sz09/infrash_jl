using FluentAssertions;
using JobLogic.Infrastructure.Microservice.Client;
using JobLogic.Infrastructure.UnitTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Unit.JobLogic.Infrastructure.Contract
{
    [TestClass]
    public class OperationInfoRuntimeDataSetterTests
    {
        [TestMethod]
        public void TestTrySet_ShouldWork()
        {
            //Arrange
            string TestKey = ValueGenerator.String();
            string testValue = ValueGenerator.String();
            var operationInfo = OperationInfoFactory.CreateTenantless();
            //Action
            var setRs = OperationInfoRuntimeDataSetter.TrySet(operationInfo, TestKey, testValue, true);

            //Assert
            setRs.Should().BeTrue();
            operationInfo.TryGetRuntimeData(TestKey, out var val).Should().BeTrue();
            val.Should().Be(testValue);
            operationInfo.TryGetRuntimeData(ValueGenerator.String() /* invalid key */, out var invalidVal).Should().BeFalse();
            invalidVal.Should().BeNull();
        }

        [TestMethod]
        public void TestTrySet_ShouldWork_WhenValueIsNULL()
        {
            //Arrange
            string TestKey = ValueGenerator.String();
            string testValue = null;
            var operationInfo = OperationInfoFactory.CreateTenantless();

            //Action
            var setRs = OperationInfoRuntimeDataSetter.TrySet(operationInfo, TestKey, testValue, true);

            //Assert
            setRs.Should().BeTrue();
            operationInfo.TryGetRuntimeData(TestKey, out var val).Should().BeTrue();
            val.Should().Be(testValue);
        }

        [TestMethod]
        public void TestTrySet_ShouldWork_WhenValueExistButOverwrite()
        {
            //Arrange
            string TestKey = ValueGenerator.String();
            string testValue = ValueGenerator.String();
            var operationInfo = OperationInfoFactory.CreateTenantless();

            //Action
            OperationInfoRuntimeDataSetter.TrySet(operationInfo, TestKey, ValueGenerator.String(), false);
            var setRs = OperationInfoRuntimeDataSetter.TrySet(operationInfo, TestKey, testValue, true);

            //Assert
            setRs.Should().BeTrue();
            operationInfo.TryGetRuntimeData(TestKey, out var val).Should().BeTrue();
            val.Should().Be(testValue);
        }

        [TestMethod]
        public void TestTrySet_ShouldReject_WhenValueExistButNotOverwrite()
        {
            //Arrange
            string TestKey = ValueGenerator.String();
            string testValue = ValueGenerator.String();
            string oldTestValue = ValueGenerator.String();

            var operationInfo = OperationInfoFactory.CreateTenantless();

            //Action
            OperationInfoRuntimeDataSetter.TrySet(operationInfo, TestKey, oldTestValue, false);
            var setRs = OperationInfoRuntimeDataSetter.TrySet(operationInfo, TestKey, testValue, false);

            //Assert
            setRs.Should().BeFalse();
            operationInfo.TryGetRuntimeData(TestKey, out var val).Should().BeTrue();
            val.Should().Be(oldTestValue);
        }

        
    }
}

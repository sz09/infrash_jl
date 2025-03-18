using FluentAssertions;
using JobLogic.Infrastructure.Microservice.Client;
using JobLogic.Infrastructure.UnitTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using FluentAssertions.Json;
using System.Linq;

namespace Tests.Unit.JobLogic.Infrastructure.Contract
{
    [TestClass]
    public class MicroServicePayloadTests
    {
        [TestMethod]
        public void TestMicroServicePayload_ShouldHaveValidOperationInfo_WhenSerializeWithTenancyOperationInfo()
        {
            var operationInfo = OperationInfoFactory.CreateTenancy(Guid.NewGuid(), new Dictionary<string, string>
            {
                { ValueGenerator.String(), ValueGenerator.String() },
                { ValueGenerator.String(), ValueGenerator.String() }
            });

            var payload = MicroservicePayload.Create(operationInfo, ValueGenerator.CreateObject<TestMsg>());


            var payloadStr = JsonConvert.SerializeObject(payload);

            var payloadJToken = JToken.Parse(payloadStr);
            payloadJToken["OperationInfo"].Should()
                .HaveElement("OperationId")
            .And.HaveElement("CreationData")
            .And.HaveElement("MessageTravelLog")
            .And.HaveElement("RuntimeData")
            .And.HaveElement("ContextTenantId");
            payloadJToken["OperationInfo"].Children().Count().Should().Be(5);
        }

        [TestMethod]
        public void TestMicroServicePayload_ShouldHaveValidOperationInfo_WhenSerializeWithTenanctlessOperationInfo()
        {
            var operationInfo = OperationInfoFactory.CreateTenantless(new Dictionary<string, string>
            {
                { ValueGenerator.String(), ValueGenerator.String() },
                { ValueGenerator.String(), ValueGenerator.String() }
            });

            var payload = MicroservicePayload.Create(operationInfo, ValueGenerator.CreateObject<TestMsg>());


            var payloadStr = JsonConvert.SerializeObject(payload);

            var payloadJToken = JToken.Parse(payloadStr);
            payloadJToken["OperationInfo"].Should()
                .HaveElement("OperationId")
            .And.HaveElement("CreationData")
            .And.HaveElement("MessageTravelLog")
            .And.HaveElement("RuntimeData")
            .And.HaveElement("ContextTenantId");
            payloadJToken["OperationInfo"].Children().Count().Should().Be(5);
        }

        [TestMethod]
        public void TestMicroServicePayload_ShouldSuccess_WhenSerializeAndDeserializeWithTenancyOperationInfo()
        {
            var operationInfo = OperationInfoFactory.CreateTenancy(Guid.NewGuid(), new Dictionary<string, string>
            {
                { ValueGenerator.String(), ValueGenerator.String() },
                { ValueGenerator.String(), ValueGenerator.String() }
            });

            var payload = MicroservicePayload.Create(operationInfo, ValueGenerator.CreateObject<TestMsg>());


            var payloadStr = JsonConvert.SerializeObject(payload);

            var payloadObj = JsonConvert.DeserializeObject<MicroservicePayload>(payloadStr);

            payload.Should().BeEquivalentTo(payloadObj, x=> x.Excluding(x => x.Data));
            payload.Data.Should().BeEquivalentTo((payloadObj.Data as JToken).ToObject<TestMsg>());

            payloadObj.ExtractTenancyOperationInfo().MessageTravelLog.Should().NotBe(operationInfo.MessageTravelLog);

            var tenantlessOperationInfo = payloadObj.ExtractTenantlessOperationInfo();
            tenantlessOperationInfo.MessageTravelLog.Should().NotBe(operationInfo.MessageTravelLog);
        }

        [TestMethod]
        public void TestMicroServicePayload_ShouldSuccess_WhenSerializeAndDeserializeWithTenancyOperationInfoAndWithRuntimeData()
        {
            var operationInfo = OperationInfoFactory.CreateTenancy(Guid.NewGuid(), new Dictionary<string, string>
            {
                { ValueGenerator.String(), ValueGenerator.String() },
                { ValueGenerator.String(), ValueGenerator.String() }
            });

            var runtimeKey = "runtimeKeyTest";
            var runtimeVal = ValueGenerator.String();
            OperationInfoRuntimeDataSetter.TrySet(operationInfo, runtimeKey, runtimeVal, true);

            var payload = MicroservicePayload.Create(operationInfo, ValueGenerator.CreateObject<TestMsg>());

            var payloadStr = JsonConvert.SerializeObject(payload);

            var payloadObj = JsonConvert.DeserializeObject<MicroservicePayload>(payloadStr);

            payload.Should().BeEquivalentTo(payloadObj, x => x.Excluding(x => x.Data));
            payload.Data.Should().BeEquivalentTo((payloadObj.Data as JToken).ToObject<TestMsg>());

            var tenancyOpInfo = payloadObj.ExtractTenancyOperationInfo();
            tenancyOpInfo.MessageTravelLog.Should().NotBe(operationInfo.MessageTravelLog);
            tenancyOpInfo.TryGetRuntimeData(runtimeKey, out var runtimeDataVal).Should().BeTrue();
            runtimeDataVal.Should().Be(runtimeVal);

            var tenantlessOperationInfo = payloadObj.ExtractTenantlessOperationInfo();
            tenantlessOperationInfo.MessageTravelLog.Should().NotBe(operationInfo.MessageTravelLog);
            tenantlessOperationInfo.TryGetRuntimeData(runtimeKey, out var runtimeDataVal2).Should().BeTrue();
            runtimeDataVal2.Should().Be(runtimeVal);
        }

        [TestMethod]
        public void TestMicroServicePayload_ShouldSuccess_WhenSerializeAndDeserializeWithTenantlessOperationInfo()
        {
            var operationInfo = OperationInfoFactory.CreateTenantless(new Dictionary<string, string>
            {
                { ValueGenerator.String(), ValueGenerator.String() },
                { ValueGenerator.String(), ValueGenerator.String() }
            });

            var payload = MicroservicePayload.Create(operationInfo, ValueGenerator.CreateObject<TestMsg>());

            var payloadStr = JsonConvert.SerializeObject(payload);

            var payloadObj = JsonConvert.DeserializeObject<MicroservicePayload>(payloadStr);

            payload.Should().BeEquivalentTo(payloadObj, x => x.Excluding(x => x.Data));
            payload.Data.Should().BeEquivalentTo((payloadObj.Data as JToken).ToObject<TestMsg>());


            payloadObj.ExtractTenantlessOperationInfo().MessageTravelLog.Should().NotBe(operationInfo.MessageTravelLog);
        }

        [TestMethod]
        public void TestMicroServicePayload_ShouldSerializeCorrectly_WhenContainRuntimeData()
        {
            //Arrange
            string TestKey = ValueGenerator.String();
            string testValue = ValueGenerator.String();
            var operationInfo = OperationInfoFactory.CreateTenancy(Guid.NewGuid(), new Dictionary<string, string>
            {
                { ValueGenerator.String(),ValueGenerator.String() }
            });
            OperationInfoRuntimeDataSetter.TrySet(operationInfo, TestKey, testValue, false);
            var payload = MicroservicePayload.Create(operationInfo, new TestMsg());
            //Action
            var strPayload = JsonConvert.SerializeObject(payload);
            var deserPayload = JsonConvert.DeserializeObject<MicroservicePayload>(strPayload);
            var newOpInfo = deserPayload.ExtractTenancyOperationInfo();

            //Assert
            newOpInfo.Should().BeEquivalentTo(operationInfo, x => x.Excluding(x => x.MessageTravelLog));
            newOpInfo.TryGetRuntimeData(TestKey, out var val).Should().BeTrue();
            val.Should().Be(testValue);
        }

        const string MsgLog1 = "hash1msg1";
        const string MsgLog12 = "hash2msg2";

        [DataRow("hash1msg1|hash2msg2", "hash1msg1", 1, true)]
        [DataRow("hash1msg1|hash2msg2", "hash2msg2", 1, true)]
        [DataRow("hash1msg1|hash2msg2|hash1msg1|xxx|hash1msg1", "hash1msg1", 3, true)]
        [DataRow("hash1msg1|hash2msg2|hash1msg1|xxx|hash1msg1", "hash1msg1", 2, true)]
        [DataRow("hash1msg1|hash2msg2|hash1msg1|xxx|hash1msg1", "hash1msg1", 1, true)]
        [DataRow("hash1msg1|hash2msg2|hash1msg1|xxx", "hash1msg1", 3, false)]
        [DataRow("hash1msg1|hash2msg2|hash1msg1|xxx", "hash1msg1", 2, true)]
        [DataRow("hash1msg1|hash2msg2|xxx", "hash1msg1", 2, false)]
        [DataRow("hash1msg1|hash2msg2|xxx", "hash1msg1", 1, true)]
        [DataRow("hash1msg1|hash2msg2|xxx", "hash2msg2", 1, true)]
        [DataRow("hash1msg1|hash2msg2|xxx", "hash2msg2", 2, false)]
        [DataRow("hash1msg1|hash2msg2", "hash1msg1", 2, false)]
        [DataRow("hash1msg1|hash2msg2", "hash2msg2", 2, false)]
        [DataRow("hash1msg1|hash2msg2", "not-match-hash1msg1f", 1, false)]
        [DataRow("", "hash1msg1", 1, false)]
        [DataRow("", "", 1, false)]
        [TestMethod]
        public void TestThrowWhenCircularMessaging_ShouldThrow_WhenMatchAtBeginingAndAllow1(string messageTravelLog, string msgLogId, int allowedCircularTime, bool doThrow)
        {
            //Arrange
            
            //Action
            Action tt = () => MicroservicePayload.ThrowWhenCircularMessaging(messageTravelLog, msgLogId, allowedCircularTime);

            //Assert
            if(doThrow)
            tt.Should().Throw<ContractException>();
            else tt.Should().NotThrow();
        }
    }

    public class TestMsg : TenantlessMsg
    {
        public string Val { get; set; }
    }
}

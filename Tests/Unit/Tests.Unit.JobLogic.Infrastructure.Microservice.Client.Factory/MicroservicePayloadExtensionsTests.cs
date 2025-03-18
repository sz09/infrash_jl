using FluentAssertions;
using JobLogic.Infrastructure.Microservice.Client;
using JobLogic.Infrastructure.Microservice.Client.Factory;
using JobLogic.Infrastructure.UnitTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Tests.Unit.JobLogic.Infrastructure.Microservice.Client.Factory
{
    [TestClass]
    public class MicroservicePayloadExtensionsTests
    {
        public class MyTestEvent : TenancyEvent
        {

        }
        [TestMethod]
        public void TestToTopicEventMessage_ShouldWork_WhenTestSingleSubscribingService()
        {
            //Arrange
            var subscribingService = ValueGenerator.String();
            var payload = MicroservicePayload.Create(OperationInfoFactory.CreateTenancy(Guid.NewGuid()), new MyTestEvent());

            //Action
            var msg = payload.ToTopicEventMessage(new EventServiceBusOption(), subscribingService);

            //Assert
            msg.ApplicationProperties[MicroservicePayloadExtensions.SubscribingServicesNotation_PropertyKey].Should().Be(subscribingService);
        }

        [TestMethod]
        public void TestToTopicEventMessage_ShouldWork_WhenTestNoSubscribingService()
        {
            //Arrange
            var subscribingService = ValueGenerator.String();
            var payload = MicroservicePayload.Create(OperationInfoFactory.CreateTenancy(Guid.NewGuid()), new MyTestEvent());

            //Action
            var msg = payload.ToTopicEventMessage(new EventServiceBusOption(), "");

            //Assert
            msg.ApplicationProperties[MicroservicePayloadExtensions.SubscribingServicesNotation_PropertyKey].Should().Be($"");
        }

        [TestMethod]
        public void TestToTopicEventMessage_ShouldThrowException_WhenUserPropertyHasSubscribingServicesNotation()
        {
            //Arrange
            var subscribingService1 = ValueGenerator.String();
            var payload = MicroservicePayload.Create(OperationInfoFactory.CreateTenancy(Guid.NewGuid()), new MyTestEvent());
            var option = new EventServiceBusOption();
            option.UserProperties[MicroservicePayloadExtensions.SubscribingServicesNotation_PropertyKey] = ValueGenerator.String();
            //Action
            Action act = () =>
            {
                payload.ToTopicEventMessage(option, subscribingService1);
            };

            //Assert
            act.Should().Throw<Exception>();
        }

        [TestMethod]
        public void TestToTopicEventMessage_ShouldWork_WithNormalOption()
        {
            //Arrange
            var subscribingService1 = ValueGenerator.String();
            var payload = MicroservicePayload.Create(OperationInfoFactory.CreateTenancy(Guid.NewGuid()), new MyTestEvent());
            var option = new EventServiceBusOption();
            var userPropKey = ValueGenerator.String();
            var userPropValue = ValueGenerator.String();
            option.UserProperties[userPropKey] = userPropValue;
            option.DelayMessageInMinutes = ValueGenerator.Int(1, 100);
            //Action
            var msg = payload.ToTopicEventMessage(option, subscribingService1);

            //Assert
            msg.ApplicationProperties[userPropKey].Should().Be(userPropValue);
            msg.ScheduledEnqueueTime.Subtract(DateTime.UtcNow).TotalMinutes.Should().BeApproximately((double)option.DelayMessageInMinutes, 0.1);
        }

        [TestMethod]
        public void TestToMessage_ShouldWork_WithNormalOption()
        {
            //Arrange
            var payload = MicroservicePayload.Create(OperationInfoFactory.CreateTenancy(Guid.NewGuid()), new MyTestEvent());
            var option = new ServiceBusOption();
            var userPropKey = ValueGenerator.String();
            var userPropValue = ValueGenerator.String();
            option.UserProperties[userPropKey] = userPropValue;
            option.DelayMessageInMinutes = ValueGenerator.Int(1, 100);
            //Action
            var msg = payload.ToMessage(option);

            //Assert
            msg.ApplicationProperties[userPropKey].Should().Be(userPropValue);
            msg.ScheduledEnqueueTime.Subtract(DateTime.UtcNow).TotalMinutes.Should().BeApproximately((double)option.DelayMessageInMinutes, 0.1);
        }
    }
}

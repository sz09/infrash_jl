using FluentAssertions;
using JobLogic.Infrastructure.Contract;
using JobLogic.Infrastructure.Event.Publisher;
using JobLogic.Infrastructure.UnitTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Tests.Unit.JobLogic.Infrastructure.Event.Publisher
{
    [TestClass]
    public class SubscribingServiceExtensionTests
    {
        [TestMethod]
        public void TestGetEventSubscribingServicesNotation()
        {
            TestTenancyEventBase v = ValueGenerator.CreateObject<TestTenancyEvent1>();
            var notation = v.GetEventSubscribingServicesNotation();
            notation.Should().Be($"|{nameof(TestTenancyEvent1)}|");

            v = ValueGenerator.CreateObject<TestTenancyEvent2>();
            notation = v.GetEventSubscribingServicesNotation();
            notation.Should().Be($"|{nameof(TestTenancyEvent1)}|{nameof(TestTenancyEvent2)}|");


            v = ValueGenerator.CreateObject<TestTenancyEvent0>();
            FluentActions.Invoking(() => v.GetEventSubscribingServicesNotation()).Should().ThrowExactly<Exception>().WithMessage("Event must have SubscribingServiceAttribute to specify the target services");
        }
    }

    public class TestTenancyEventBase : TenancyEvent
    {
    }

    public class TestTenancyEvent0 : TestTenancyEventBase
    {
        public string Val { get; set; }
    }

    [SubscribingService(nameof(TestTenancyEvent1))]
    public class TestTenancyEvent1 : TestTenancyEventBase
    {
        public string Val { get; set; }
    }

    [SubscribingService(nameof(TestTenancyEvent1))]
    [SubscribingService(nameof(TestTenancyEvent2))]
    public class TestTenancyEvent2 : TestTenancyEventBase
    {
        public string Val { get; set; }
    }
}

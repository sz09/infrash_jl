using FluentAssertions;
using FluentAssertions.Extensions;
using JobLogic.Infrastructure.Microservice.Client;
using JobLogic.Infrastructure.UnitTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Unit.JobLogic.Infrastructure.Microservice.Client.Tests
{
    [TestClass]
    public class SubscribingServiceExtensionTests
    {
        const string TestSubscribingName1 = "TestSubscribingName1";
        const string TestSubscribingName2 = "TestSubscribingName2";
        const string TestCustomRouteName = "TestCustomRouteName";

        [SubscribingService(TestSubscribingName1)]
        public class NoCustomRouteSubscribing1Event : TenancyEvent
        {

        }

        [TestMethod("GetEventSubscribingServices | Should Within 1 Sec | WhenProcess1Million For " + nameof(NoCustomRouteSubscribing1Event))]
        public async Task Test1()
        {
            //Arrange
            int count = 1000000;
            var events = ValueGenerator.CreateMany<NoCustomRouteSubscribing1Event>(count);
            ConcurrentBag<SubscribingServiceAttribute[]> bags = new ConcurrentBag<SubscribingServiceAttribute[]>();
            async Task ExecTask()
            {
                List<Task> ts = new List<Task>();
                foreach (var msg in events)
                {
                    var t = Task.Run(() =>
                    {
                        var subscribers = msg.GetEventSubscribingServiceAttributes();
                        bags.Add(subscribers);
                    });
                    ts.Add(t);
                }

                await Task.WhenAll(ts);
            }

            Func<Task> execTask = () => ExecTask();

            //Action + Assert
            await execTask.Should().CompleteWithinAsync(1.Seconds());

            foreach (var subscribers in bags)
            {
                subscribers.Should().HaveCount(1);
                subscribers.Select(x => x.SubscriptionNotation).Should().Contain(TestSubscribingName1);
            }
        }

        [SubscribingService(TestSubscribingName1)]
        [SubscribingService(TestSubscribingName2)]
        public class NoCustomRouteSubscribing1And2Event : TenancyEvent
        {

        }

        [TestMethod("GetEventSubscribingServices | Should Within 1 Sec | When Process1Million For " + nameof(NoCustomRouteSubscribing1And2Event))]
        public async Task Test2()
        {
            //Arrange
            int count = 1000000;
            var events = ValueGenerator.CreateMany<NoCustomRouteSubscribing1And2Event>(count);
            ConcurrentBag<SubscribingServiceAttribute[]> bags = new ConcurrentBag<SubscribingServiceAttribute[]>();
            async Task ExecTask()
            {
                List<Task> ts = new List<Task>();
                foreach (var msg in events)
                {
                    var t = Task.Run(() =>
                    {
                        var subscribers = msg.GetEventSubscribingServiceAttributes();
                        bags.Add(subscribers);
                        
                    });
                    ts.Add(t);
                }

                await Task.WhenAll(ts);
            }

            Func<Task> execTask = () => ExecTask();

            //Action + Assert
            await execTask.Should().CompleteWithinAsync(1.Seconds());

            foreach (var subscribers in bags)
            {
                subscribers.Should().HaveCount(2);
                subscribers.Select(x => x.SubscriptionNotation).Should().Contain(TestSubscribingName1).And.Contain(TestSubscribingName2);
            }
        }


        [SubscribingService(TestSubscribingName1, CustomRouteName = TestCustomRouteName)]
        public class WithCustomRouteSubscribing1Event : TenancyEvent
        {

        }

        [TestMethod("GetEventSubscribingServices | Should Within 1 Sec | When Process1Million For " + nameof(WithCustomRouteSubscribing1Event))]
        public async Task Test3()
        {
            //Arrange
            int count = 1000000;
            var events = ValueGenerator.CreateMany<WithCustomRouteSubscribing1Event>(count);
            ConcurrentBag<SubscribingServiceAttribute[]> bags = new ConcurrentBag<SubscribingServiceAttribute[]>();
            async Task ExecTask()
            {
                List<Task> ts = new List<Task>();
                foreach (var msg in events)
                {
                    var t = Task.Run(() =>
                    {
                        var subscribers = msg.GetEventSubscribingServiceAttributes();
                        bags.Add(subscribers);
                        
                    });
                    ts.Add(t);
                }

                await Task.WhenAll(ts);
            }

            Func<Task> execTask = () => ExecTask();

            //Action + Assert
            await execTask.Should().CompleteWithinAsync(1.Seconds());

            foreach (var subscribers in bags)
            {
                subscribers.Should().HaveCount(1);
                subscribers.Select(x => x.SubscriptionNotation).Should().Contain(TestSubscribingName1);
                subscribers.Select(x => x.CustomRouteName).Should().Contain(TestCustomRouteName);
            }
        }

        [SubscribingService(TestSubscribingName2, CustomRouteName = TestCustomRouteName)]
        [SubscribingService(TestSubscribingName1)]
        public class WithCustomRouteSubscribing1And2Event : TenancyEvent
        {

        }
        [TestMethod("GetEventSubscribingServices | Should Within 1 Sec | When Process1Million For " + nameof(WithCustomRouteSubscribing1And2Event))]
        public async Task Test4()
        {
            //Arrange
            int count = 1000000;
            var events = ValueGenerator.CreateMany<WithCustomRouteSubscribing1And2Event>(count);
            ConcurrentBag<SubscribingServiceAttribute[]> bags = new ConcurrentBag<SubscribingServiceAttribute[]>();
            async Task ExecTask()
            {
                List<Task> ts = new List<Task>();
                foreach (var msg in events)
                {
                    var t = Task.Run(() =>
                    {
                        var subscribers = msg.GetEventSubscribingServiceAttributes();
                        bags.Add(subscribers);
                        
                    });
                    ts.Add(t);
                }

                await Task.WhenAll(ts);
            }

            Func<Task> execTask = () => ExecTask();

            //Action + Assert
            await execTask.Should().CompleteWithinAsync(1.Seconds());

            foreach(var subscribers in bags)
            {
                subscribers.Should().HaveCount(2);
                subscribers.Select(x => x.SubscriptionNotation).Should().Contain(TestSubscribingName1);
                subscribers.Single(x => x.CustomRouteName != null).CustomRouteName.Should().Be(TestCustomRouteName);
            }
        }
    }
}

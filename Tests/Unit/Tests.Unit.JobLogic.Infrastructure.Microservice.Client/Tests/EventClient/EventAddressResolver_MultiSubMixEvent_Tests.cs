using FluentAssertions;
using FluentAssertions.Extensions;
using JobLogic.Infrastructure.Microservice.Client;
using JobLogic.Infrastructure.UnitTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tests.Unit.JobLogic.Infrastructure.Microservice.Client.Tests
{
    [TestClass]
    public class EventAddressResolver_MultiSubMixEvent_Tests
    {
        [SubscribingService(SubscribingServiceA)]
        [SubscribingService(SubscribingServiceB)]
        [SubscribingService(SubscribingServiceC, CustomRouteName = CustomRouteName)]
        public class MyMultiSubMixEvent : TenancyEvent
        {

        }

        public const string SubscribingServiceA = nameof(SubscribingServiceA);
        public const string SubscribingServiceB = nameof(SubscribingServiceB);
        public const string SubscribingServiceC = nameof(SubscribingServiceC);


        public const string CustomRouteName = "CustomRouteName";





        [TestMethod]
        public async Task TestResolve_ShouldWork_WithMultiSubMixRoute()
        {
            //Arrange
            var sbns = ValueGenerator.String();
            var topicName = ValueGenerator.String();
            var customRoute = new Dictionary<string, EventRoute>
                {
                    {
                        CustomRouteName,
                        new EventRoute{
                            SubscriptionNotationSuffixDict =  new Dictionary<string, string> { { SubscribingServiceA, ValueGenerator.String() },
                                { SubscribingServiceB, ValueGenerator.String() },
                            { SubscribingServiceC, ValueGenerator.String() }},
                        }
                    }
                };
            var resolver = new EventAddressResolver(sbns, topicName, customRoute);


            //Action
            var addr = resolver.Resolve(new MyMultiSubMixEvent());

            //Assert
            var expectedAddr = new EventAddress(sbns, topicName, $"|{SubscribingServiceA}|{SubscribingServiceB}|{SubscribingServiceC}{customRoute[CustomRouteName].SubscriptionNotationSuffixDict[SubscribingServiceC]}|");

            addr.Should().BeEquivalentTo(expectedAddr);
        }

        [TestMethod("Resolve | Should Within 1 Sec | When Process1Million For " + nameof(MyMultiSubMixEvent))]
        public async Task Test4()
        {
            //Arrage
            var sbns = ValueGenerator.String();
            var topicName = ValueGenerator.String();
            var customRoute = new Dictionary<string, EventRoute>
                {
                    {
                        CustomRouteName,
                        new EventRoute{
                            SubscriptionNotationSuffixDict = new Dictionary<string, string> { },
                        }
                    }
                };
            var resolver = new EventAddressResolver(sbns, topicName, customRoute);

            int count = 1000000;
            var events = ValueGenerator.CreateMany<MyMultiSubMixEvent>(count);
            ConcurrentBag<EventAddress> bags = new ConcurrentBag<EventAddress>();
            async Task ExecTask()
            {
                List<Task> ts = new List<Task>();
                foreach (var evt in events)
                {
                    var t = Task.Run(() =>
                    {
                        resolver.Resolve(evt);

                    });
                    ts.Add(t);
                }

                await Task.WhenAll(ts);
            }

            Func<Task> execTask = () => ExecTask();


            //Action + Assert
            await execTask.Should().CompleteWithinAsync(1.Seconds());

        }

        [TestMethod]
        public async Task TestResolve_ShouldUseDefaultRoute_WhenCUstomRouteMissConfig()
        {
            //Arrange
            var sbns = ValueGenerator.String();
            var topicName = ValueGenerator.String();
            var customRoute = new Dictionary<string, EventRoute>
                {
                    {
                        CustomRouteName,
                        new EventRoute{
                            SubscriptionNotationSuffixDict = new Dictionary<string, string> { { SubscribingServiceA, ValueGenerator.String() },{ SubscribingServiceB, ValueGenerator.String() } },
                        }
                    }
                };
            var resolver = new EventAddressResolver(sbns, topicName, customRoute);


            //Action
            var addr = resolver.Resolve(new MyMultiSubMixEvent());

            //Assert
            var expectedAddr = new EventAddress(sbns, topicName, $"|{SubscribingServiceA}|{SubscribingServiceB}|{SubscribingServiceC}|");

            addr.Should().BeEquivalentTo(expectedAddr);
        }


        [TestMethod]
        public async Task TestResolve_ShouldUsePreferRoute_WhenItProperlyConfig()
        {
            //Arrange
            var sbns = ValueGenerator.String();
            var topicName = ValueGenerator.String();
            var customRoute = new Dictionary<string, EventRoute>
                {
                    {
                        CustomRouteName,
                        new EventRoute{
                            SubscriptionNotationSuffixDict = new Dictionary<string, string> { { SubscribingServiceA, ValueGenerator.String() },{ SubscribingServiceB, ValueGenerator.String() } },
                        }
                    },
                    {
                        SetupInitializer.PREFERRED_ROUTE_NAME,
                        new EventRoute{
                            SubscriptionNotationSuffixDict = new Dictionary<string, string> { { SubscribingServiceB, ValueGenerator.String() } },
                        }
                    }
                };
            var resolver = new EventAddressResolver(sbns, topicName, customRoute);


            //Action
            var addr = resolver.Resolve(new MyMultiSubMixEvent());

            //Assert
            var expectedAddr = new EventAddress(sbns, topicName, $"|{SubscribingServiceA}|{SubscribingServiceB}{customRoute[SetupInitializer.PREFERRED_ROUTE_NAME].SubscriptionNotationSuffixDict[SubscribingServiceB]}|{SubscribingServiceC}|");

            addr.Should().BeEquivalentTo(expectedAddr);
        }

        [TestMethod]
        public async Task TestResolve_ShouldUseCustomRoute_WhenCustomAndPreferredRouteProperlyConfig()
        {
            //Arrange
            var sbns = ValueGenerator.String();
            var topicName = ValueGenerator.String();
            var customRoute = new Dictionary<string, EventRoute>
                {
                    {
                        CustomRouteName,
                        new EventRoute{
                            SubscriptionNotationSuffixDict = new Dictionary<string, string> { { SubscribingServiceA, ValueGenerator.String() },
                                { SubscribingServiceB, ValueGenerator.String() },
                            { SubscribingServiceC, ValueGenerator.String() }},
                        }
                    },
                    {
                        SetupInitializer.PREFERRED_ROUTE_NAME,
                        new EventRoute{
                            SubscriptionNotationSuffixDict = new Dictionary<string, string> { { SubscribingServiceC, ValueGenerator.String() },{ SubscribingServiceB, ValueGenerator.String() } },
                        }
                    }
                };
            var resolver = new EventAddressResolver(sbns, topicName, customRoute);


            //Action
            var addr = resolver.Resolve(new MyMultiSubMixEvent());

            //Assert
            var expectedAddr = new EventAddress(sbns, topicName, $"|{SubscribingServiceA}|{SubscribingServiceB}{customRoute[SetupInitializer.PREFERRED_ROUTE_NAME].SubscriptionNotationSuffixDict[SubscribingServiceB]}|{SubscribingServiceC}{customRoute[CustomRouteName].SubscriptionNotationSuffixDict[SubscribingServiceC]}|");

            addr.Should().BeEquivalentTo(expectedAddr);
        }
    }

}

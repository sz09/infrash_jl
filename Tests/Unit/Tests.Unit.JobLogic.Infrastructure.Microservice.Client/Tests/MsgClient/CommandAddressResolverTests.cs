using FluentAssertions;
using JobLogic.Infrastructure.Microservice.Client;
using JobLogic.Infrastructure.UnitTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tests.Unit.JobLogic.Infrastructure.Microservice.Client.Tests
{
    [TestClass]
    public class CommandAddressResolverTests
    {
        [MsgCustomRoute(TestRouteName)]
        public class MyTestCustomRouteMsg : TenancyMsg
        {

        }

        public class MyTestDefaultMsg : TenancyMsg
        {

        }

        public class MyTestDefaultInheritFromCustomRouteMsg : MyTestCustomRouteMsg
        {

        }

        public const string TestRouteName = "TESTRouteName";
        [TestMethod]
        public async Task TestResolveCommandAddress_ShouldWork_WithCustomRoute()
        {
            //Arrage
            var queueName = "TestQueueName";
            var commandAddrResolver = new CommandAddressResolver("DefaultRouteCommandSBNS", queueName, new Dictionary<string, MsgCustomRoute>
                {
                    {
                        TestRouteName,
                        new MsgCustomRoute{
                            CommandQueueNameSuffix = "TestRouteNameCommandSBNS",
                            RequestOrigin = "TestRouteNameRequestOrigin"
                        }
                    }
                });


            //Action
            var addr = commandAddrResolver.Resolve(new MyTestCustomRouteMsg());

            //Assert
            addr.Should().Be(new CommandAddress("DefaultRouteCommandSBNS", queueName + "TestRouteNameCommandSBNS"));
        }

        [TestMethod]
        public async Task TestResolveCommandAddress_ShouldWork_WithDefaultRoute()
        {
            //Arrage
            var queueName = "TestQueueName";
            var commandAddrResolver = new CommandAddressResolver("DefaultRouteCommandSBNS", queueName, new Dictionary<string, MsgCustomRoute>
                {
                    {
                        TestRouteName,
                        new MsgCustomRoute{
                            CommandQueueNameSuffix = "TestRouteNameCommandSBNS",
                            RequestOrigin = "TestRouteNameRequestOrigin"
                        }
                    }
                });

            //Action
            var addr = commandAddrResolver.Resolve(new MyTestDefaultMsg());

            //Assert
            addr.Should().Be(new CommandAddress("DefaultRouteCommandSBNS", queueName));
        }


        [TestMethod]
        public async Task TestResolveCommandAddress_ShouldResolveDefault_WhenDefaultMsgInheritFromCustom()
        {
            //Arrage
            var queueName = "TestQueueName";
            var commandAddrResolver = new CommandAddressResolver("DefaultRouteCommandSBNS", queueName, new Dictionary<string, MsgCustomRoute>
                {
                    {
                        TestRouteName,
                        new MsgCustomRoute{
                            CommandQueueNameSuffix = "TestRouteNameCommandSBNS",
                            RequestOrigin = "TestRouteNameRequestOrigin"
                        }
                    }
                });

            //Action
            var addr = commandAddrResolver.Resolve(new MyTestDefaultInheritFromCustomRouteMsg());

            //Assert
            addr.Should().Be(new CommandAddress("DefaultRouteCommandSBNS", queueName));
        }


        [TestMethod]
        public async Task TestResolveCommandAddress_ShouldUsePreferredRoute_ForDefaultMsg()
        {
            //Arrage
            var queueName = "TestQueueName";
            var commandAddrResolver = new CommandAddressResolver("DefaultRouteCommandSBNS", queueName, new Dictionary<string, MsgCustomRoute>
                {
                    {
                        TestRouteName,
                        new MsgCustomRoute{
                            CommandQueueNameSuffix = "TestRouteNameCommandSBNS",
                            RequestOrigin = "TestRouteNameRequestOrigin"
                        }
                    },
                    {
                        SetupInitializer.PREFERRED_ROUTE_NAME,
                        new MsgCustomRoute{
                            CommandQueueNameSuffix = "MyTestPreferredRouteSBNS",
                            RequestOrigin = "MyTestPreferredRouteRequestOrigin"
                        }
                    }
                });

            //Action
            var addr = commandAddrResolver.Resolve(new MyTestDefaultMsg());

            //Assert
            addr.Should().Be(new CommandAddress("DefaultRouteCommandSBNS", queueName + "MyTestPreferredRouteSBNS"));
        }

        [TestMethod]
        public async Task TestResolveCommandAddress_ShouldUsePreferredRoute_ForCustomRouteMsgWithoutConfig()
        {
            //Arrage
            var queueName = "TestQueueName";
            var commandAddrResolver = new CommandAddressResolver("DefaultRouteCommandSBNS", queueName, new Dictionary<string, MsgCustomRoute>
                {
                    {
                        "Random",
                        new MsgCustomRoute{
                            CommandQueueNameSuffix = "TestRouteNameCommandSBNS",
                            RequestOrigin = "TestRouteNameRequestOrigin"
                        }
                    },
                    {
                        SetupInitializer.PREFERRED_ROUTE_NAME,
                        new MsgCustomRoute{
                            CommandQueueNameSuffix = "MyTestPreferredRouteSBNS",
                            RequestOrigin = "MyTestPreferredRouteRequestOrigin"
                        }
                    }
                });

            //Action
            var addr = commandAddrResolver.Resolve(new MyTestCustomRouteMsg());

            //Assert
            addr.Should().Be(new CommandAddress("DefaultRouteCommandSBNS", queueName + "MyTestPreferredRouteSBNS"));
        }

        [TestMethod]
        public async Task TestResolveCommandAddress_ShouldUseCustomRoute_ForCustomRouteMsgWithConfigOfPreferredRoute()
        {
            //Arrage
            var queueName = "TestQueueName";
            var commandAddrResolver = new CommandAddressResolver("DefaultRouteCommandSBNS", queueName, new Dictionary<string, MsgCustomRoute>
                {
                    {
                        TestRouteName,
                        new MsgCustomRoute{
                            CommandQueueNameSuffix = "TestRouteNameCommandSBNS",
                            RequestOrigin = "TestRouteNameRequestOrigin"
                        }
                    },
                    {
                        SetupInitializer.PREFERRED_ROUTE_NAME,
                        new MsgCustomRoute{
                            CommandQueueNameSuffix = "MyTestPreferredRouteSBNS",
                            RequestOrigin = "MyTestPreferredRouteRequestOrigin"
                        }
                    }
                });

            //Action
            var addr = commandAddrResolver.Resolve(new MyTestCustomRouteMsg());

            //Assert
            addr.Should().Be(new CommandAddress("DefaultRouteCommandSBNS", queueName + "TestRouteNameCommandSBNS"));
        }

        [TestMethod]
        public async Task TestResolveCommandAddress_ShouldUseDefaultRoute_WithPreferredRouteEnvironmentVariableButNoConfig()
        {
            //Arrage
            var queueName = "TestQueueName";
            var commandAddrResolver = new CommandAddressResolver("DefaultRouteCommandSBNS", queueName, new Dictionary<string, MsgCustomRoute>
                {
                    {
                        "someRandom",
                        new MsgCustomRoute{
                            CommandQueueNameSuffix = "TestRouteNameCommandSBNS",
                            RequestOrigin = "TestRouteNameRequestOrigin"
                        }
                    }
                });

            //Action
            var addr = commandAddrResolver.Resolve(new MyTestCustomRouteMsg());

            //Assert
            addr.Should().Be(new CommandAddress("DefaultRouteCommandSBNS", queueName));
        }
    }

}

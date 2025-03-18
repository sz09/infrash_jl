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
    public class RequestAddressResolverTests
    {
        [MsgCustomRoute(TestRouteName)]
        public class MyTestCustomRouteMsg : TenancyMsg
        {

        }

        public class MyTestDefaultMsg : TenancyMsg
        {

        }

        public const string TestRouteName = "TESTRouteName";

        [DataRow("MyPath/")]
        [DataRow("MyPath")]
        [TestMethod]
        public async Task TestResolve_ShouldWork_WithCustomRoute(string path)
        {
            //Arrage
            var customRoute = new Dictionary<string, MsgCustomRoute>
                {
                    {
                        TestRouteName,
                        new MsgCustomRoute{
                            CommandQueueNameSuffix = "TestRouteNameCommandSBNS",
                            RequestOrigin = "TestRouteNameRequestOrigin"
                        }
                    }
                };
            var resolver = new RequestAddressResolver("DefaultRouteRequestOrigin", path, customRoute);

            //Action
            var addr = resolver.Resolve(new MyTestCustomRouteMsg());

            //Assert
            addr.Should().Be(new RequestAddress(customRoute[TestRouteName].RequestOrigin, path.TrimEnd('/') + "/" + BaseMicroserviceData.GetSignatureHint(typeof(MyTestCustomRouteMsg))));


        }

        [DataRow("MyPath/")]
        [DataRow("MyPath")]
        [TestMethod]
        public async Task TestResolve_ShouldWork_WithDefaultRoute(string path)
        {
            //Arrage
            var customRoute = new Dictionary<string, MsgCustomRoute>
                {
                    {
                        TestRouteName,
                        new MsgCustomRoute{
                            CommandQueueNameSuffix = "TestRouteNameCommandSBNS",
                            RequestOrigin = "TestRouteNameRequestOrigin"
                        }
                    }
                };

            var resolver = new RequestAddressResolver("DefaultRouteRequestOrigin", path, customRoute);

            //Action
            var addr = resolver.Resolve(new MyTestDefaultMsg());

            //Assert
            addr.Should().Be(new RequestAddress("DefaultRouteRequestOrigin", path.TrimEnd('/') + "/" + BaseMicroserviceData.GetSignatureHint(typeof(MyTestDefaultMsg))));

        }

        [DataRow("MyPath/")]
        [DataRow("MyPath")]
        [TestMethod]
        public async Task TestResolve_ShouldFallBackDefault_IfCustomRouteRequestOriginInvalid(string path)
        {
            //Arrage
            var customRoute = new Dictionary<string, MsgCustomRoute>
                {
                    {
                        TestRouteName,
                        new MsgCustomRoute{
                        }
                    }
                };

            var resolver = new RequestAddressResolver("DefaultRouteRequestOrigin", path, customRoute);

            //Action
            var addr = resolver.Resolve(new MyTestCustomRouteMsg());

            //Assert
            addr.Should().Be(new RequestAddress("DefaultRouteRequestOrigin", path.TrimEnd('/') + "/" + BaseMicroserviceData.GetSignatureHint(typeof(MyTestCustomRouteMsg))));

        }

        [DataRow("MyPath/")]
        [DataRow("MyPath")]
        [TestMethod]
        public async Task TestResolve_ShouldFallBackDefault_IfCustomRouteNULL(string path)
        {
            //Arrage
            var customRoute = new Dictionary<string, MsgCustomRoute>
                {
                    {
                        TestRouteName,
                        null
                    }
                };
            var resolver = new RequestAddressResolver("DefaultRouteRequestOrigin", path, customRoute);

            //Action
            var addr = resolver.Resolve(new MyTestCustomRouteMsg());

            //Assert
            addr.Should().Be(new RequestAddress("DefaultRouteRequestOrigin", path.TrimEnd('/') + "/" + BaseMicroserviceData.GetSignatureHint(typeof(MyTestCustomRouteMsg))));

        }

        [DataRow("MyPath/")]
        [DataRow("MyPath")]
        [TestMethod]
        public async Task TestResolve_ShouldWork_WhenSkipAppendMsgSignatureHintPath(string path)
        {
            //Arrage
            var customRoute = new Dictionary<string, MsgCustomRoute>
                {
                    {
                        TestRouteName,
                        new MsgCustomRoute{
                            CommandQueueNameSuffix = "TestRouteNameCommandSBNS",
                            RequestOrigin = "TestRouteNameRequestOrigin"
                        }
                    }
                };

            var resolver = new RequestAddressResolver("DefaultRouteRequestOrigin", path, customRoute, true);

            //Action
            var addr = resolver.Resolve(new MyTestCustomRouteMsg());

            //Assert
            addr.Should().Be(new RequestAddress(customRoute[TestRouteName].RequestOrigin, path));
        }

        [DataRow("MyPath/")]
        [DataRow("MyPath")]
        [TestMethod]
        public async Task TestResolve_ShouldUsePreferredRoute_ForDefaultMsg(string path)
        {
            //Arrage
            var customRoute = new Dictionary<string, MsgCustomRoute>
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
                };

            var resolver = new RequestAddressResolver("DefaultRouteRequestOrigin", path, customRoute, true);

            //Action
            var addr = resolver.Resolve(new MyTestDefaultMsg());

            //Assert
            addr.Should().Be(new RequestAddress(customRoute[SetupInitializer.PREFERRED_ROUTE_NAME].RequestOrigin, path));
        }


        [DataRow("MyPath/")]
        [DataRow("MyPath")]
        [TestMethod]
        public async Task TestResolve_ShouldUsePreferredRoute_ForCustomRouteMsgWithoutConfig(string path)
        {
            //Arrage
            var customRoute = new Dictionary<string, MsgCustomRoute>
                {
                    {
                        SetupInitializer.PREFERRED_ROUTE_NAME,
                        new MsgCustomRoute{
                            CommandQueueNameSuffix = "MyTestPreferredRouteSBNS",
                            RequestOrigin = "MyTestPreferredRouteRequestOrigin"
                        }
                    }
                };

            var resolver = new RequestAddressResolver("DefaultRouteRequestOrigin", path, customRoute, true);

            //Action
            var addr = resolver.Resolve(new MyTestCustomRouteMsg());

            //Assert
            addr.Should().Be(new RequestAddress(customRoute[SetupInitializer.PREFERRED_ROUTE_NAME].RequestOrigin, path));


        }

        [DataRow("MyPath/")]
        [DataRow("MyPath")]
        [TestMethod]
        public async Task TestResolve_ShouldUseCustomRoute_ForCustomRouteMsgWithConfigOfPreferredRoute(string path)
        {
            //Arrage
            var customRoute = new Dictionary<string, MsgCustomRoute>
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
                };

            var resolver = new RequestAddressResolver("DefaultRouteRequestOrigin", path, customRoute, true);

            //Action
            var addr = resolver.Resolve(new MyTestCustomRouteMsg());

            //Assert
            addr.Should().Be(new RequestAddress(customRoute[TestRouteName].RequestOrigin, path));


        }

        [DataRow("MyPath/")]
        [DataRow("MyPath")]
        [TestMethod]
        public async Task TestResolve_ShouldUseDefaultRoute_WithPreferredRouteEnvironmentVariableButNoConfig(string path)
        {
            //Arrage
            var customRoute = new Dictionary<string, MsgCustomRoute>
                {
                    {
                        "someRandom",
                        new MsgCustomRoute{
                            CommandQueueNameSuffix = "MyTestPreferredRouteSBNS",
                            RequestOrigin = "MyTestPreferredRouteRequestOrigin"
                        }
                    }
                };

            var resolver = new RequestAddressResolver("DefaultRouteRequestOrigin", path, customRoute, true);

            //Action
            var addr = resolver.Resolve(new MyTestCustomRouteMsg());

            //Assert
            addr.Should().Be(new RequestAddress("DefaultRouteRequestOrigin", path));
        }
    }

}

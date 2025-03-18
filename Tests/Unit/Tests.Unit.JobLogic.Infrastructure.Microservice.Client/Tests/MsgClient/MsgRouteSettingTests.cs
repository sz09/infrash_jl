using FluentAssertions;
using JobLogic.Infrastructure.Microservice.Client;
using JobLogic.Infrastructure.UnitTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tests.Unit.JobLogic.Infrastructure.Microservice.Client.Tests
{
    [TestClass]
    public class MsgRouteSettingTests
    {
        [TestMethod]
        public async Task TestGetCustomRouteOrDefault_ShouldWork()
        {
            var routeName = ValueGenerator.String();
            var defaultCommand = ValueGenerator.String();
            var defaultRequest = ValueGenerator.String();
            var routeCommand = ValueGenerator.String();
            var routeRequest = ValueGenerator.String();

            var nullRouteName = ValueGenerator.String();
            var setting = new Dictionary<string, MsgCustomRoute>() {
                    {
                        routeName,
                        new MsgCustomRoute
                        {
                            RequestOrigin = routeRequest,
                            CommandQueueNameSuffix = routeCommand,
                        }
                    },
                    {
                        nullRouteName,
                        null
                    }
                };

            var route = setting.GetCustomRouteOrNULL(routeName);
            route.Should().NotBeNull();
            route.CommandQueueNameSuffix.Should().Be(routeCommand);
            route.RequestOrigin.Should().Be(routeRequest);
            setting.GetCustomRouteOrNULL(ValueGenerator.String()).Should().BeNull();
            setting.GetCustomRouteOrNULL(null).Should().BeNull();
            setting.GetCustomRouteOrNULL(string.Empty).Should().BeNull();
            setting.GetCustomRouteOrNULL(nullRouteName).Should().BeNull();
        }
    }


}

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
    public class EventRouteSettingTests
    {

        [TestMethod]
        public async Task TestGetCustomRouteOrDefault_ShouldWork()
        {
            var routeName = ValueGenerator.String();
            var defaultSBNS = ValueGenerator.String();
            var routeSBNS = ValueGenerator.String();

            var nullRouteName = ValueGenerator.String();
            var setting = new Dictionary<string, EventRoute>() {
                    {
                        routeName,
                        new EventRoute
                        {
                            SubscriptionNotationSuffixDict = new Dictionary<string,string> {{ValueGenerator.String(), routeSBNS } },
                        }
                    },
                    {
                        nullRouteName,
                        null
                    }
                };

            var route = setting.GetCustomRouteOrNULL(routeName);
            route.Should().NotBeNull();
            route.SubscriptionNotationSuffixDict.Should().ContainValue(routeSBNS);
            setting.GetCustomRouteOrNULL(ValueGenerator.String()).Should().BeNull();
            setting.GetCustomRouteOrNULL(null).Should().BeNull();
            setting.GetCustomRouteOrNULL(string.Empty).Should().BeNull();
            setting.GetCustomRouteOrNULL(nullRouteName).Should().BeNull();
        }
    }

}

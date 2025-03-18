using FluentAssertions;
using FluentAssertions.Extensions;
using JobLogic.Infrastructure.Microservice.Client;
using JobLogic.Infrastructure.UnitTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Unit.JobLogic.Infrastructure.Microservice.Client.Tests
{
    [TestClass]
    public class MsgCustomRouteExtensionsTests
    {
        public class NoCustomRouteMsg : TenancyMsg
        {

        }
        [TestMethod]
        public async Task TestGetCustomRouteName_ShouldWithin1Sec_WhenProcess1Million()
        {
            //Arrange
            int count = 1000000;
            var msgs = ValueGenerator.CreateMany<NoCustomRouteMsg>(count);
            async Task ExecTask()
            {
                List<Task> ts = new List<Task>();
                foreach (var msg in msgs)
                {
                    var t = Task.Run(() =>
                    {
                        var routeName = msg.GetCustomRouteName();
                        //routeName.Should().BeNull();
                    });
                    ts.Add(t);
                }

                await Task.WhenAll(ts);
            }

            Func<Task> execTask = () => ExecTask();

            //Action + Assert
            await execTask.Should().CompleteWithinAsync(1.Seconds());
        }

        const string TestRouteName = "TestRouteName";

        [MsgCustomRoute(TestRouteName)]
        public class WithCustomRouteMsg : TenancyMsg
        {

        }
        [TestMethod]
        public async Task TestGetCustomRouteName_ShouldWithin1Sec_WhenProcess1MillionForCustomRouteMsg()
        {
            //Arrange
            int count = 1000000;
            var msgs = ValueGenerator.CreateMany<WithCustomRouteMsg>(count);
            async Task ExecTask()
            {
                List<Task> ts = new List<Task>();
                foreach (var msg in msgs)
                {
                    var t = Task.Run(() =>
                    {
                        var routeName = msg.GetCustomRouteName();
                        //routeName.Should().Be(TestRouteName);
                    });
                    ts.Add(t);
                }

                await Task.WhenAll(ts);
            }

            Func<Task> execTask = () => ExecTask();

            //Action + Assert
            await execTask.Should().CompleteWithinAsync(1.Seconds());
        }
    }
}

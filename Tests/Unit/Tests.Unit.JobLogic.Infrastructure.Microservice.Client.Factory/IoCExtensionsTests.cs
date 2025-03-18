using FluentAssertions;
using JobLogic.Infrastructure.Microservice.Client;
using JobLogic.Infrastructure.Microservice.Client.Factory;
using JobLogic.Infrastructure.UnitTest;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using Polly;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tests.Unit.JobLogic.Infrastructure.Microservice.Client.Factory
{
    [TestClass]    
    public class IoCExtensionsTests
    {
        [TestMethod]
        public async Task TestAddDefaultMsgSenderWithRetryFactory_ShouldWorkWithGetDefaultMsgSenderWithRetryFactory()
        {
            var services = new ServiceCollection();
            services.AddDefaultMsgSenderWithRetryFactory();
            var serviceProvider = services.BuildServiceProvider();
            var ftr = serviceProvider.GetDefaultMsgSenderWithRetryFactory();
            ftr.Should().NotBeNull();
        }

    }
}

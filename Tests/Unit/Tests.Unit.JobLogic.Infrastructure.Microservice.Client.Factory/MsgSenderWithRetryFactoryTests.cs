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
    public class MsgSenderWithRetryFactoryTests
    {
        Mock<HttpMessageHandler> _handlerMock;
        IServiceCollection _services;
        IRequestSender _requestSender;

        [TestMethod]
        [ExpectedException(typeof(MicroserviceClientException))]
        public async Task RequestSenderTest_DefaultPolicy_ServiceUnavailable_ShouldRetry()
        {
            int maxRetry = 5;

            var defaultPolicy = IoCExtensions.GetDefaultRetryPolicy(maxRetry, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt / 2)));

            InitServices(HttpStatusCode.ServiceUnavailable, defaultPolicy);

            await _requestSender.SendAsync(new RequestAddress($"http://{ValueGenerator.String(5)}", ""), null);

            _handlerMock.Protected().Verify(
               "SendAsync",
               Times.Exactly(maxRetry + 1),
               ItExpr.IsAny<HttpRequestMessage>(),
               ItExpr.IsAny<CancellationToken>());
        }

        [TestMethod]
        [ExpectedException(typeof(MicroserviceClientException))]
        public async Task RequestSenderTest_DefaultPolicy_StatusNotFound_ShouldRetry()
        {
            int maxRetry = 5;

            var defaultPolicy = IoCExtensions.GetDefaultRetryPolicy(maxRetry, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt / 2)));

            InitServices(HttpStatusCode.NotFound, defaultPolicy);

            await _requestSender.SendAsync(new RequestAddress($"http://{ValueGenerator.String(5)}", ""), null);

            _handlerMock.Protected().Verify(
               "SendAsync",
               Times.Exactly(maxRetry + 1),
               ItExpr.IsAny<HttpRequestMessage>(),
               ItExpr.IsAny<CancellationToken>());
        }

        [TestMethod]
        [ExpectedException(typeof(MicroserviceClientException))]
        public async Task RequestSenderTest_DefaultPolicy_BadGateway_ShouldRetry()
        {
            int maxRetry = 5;

            var defaultPolicy = IoCExtensions.GetDefaultRetryPolicy(maxRetry, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt / 2)));

            InitServices(HttpStatusCode.BadGateway, defaultPolicy);

            await _requestSender.SendAsync(new RequestAddress($"http://{ValueGenerator.String(5)}", ""), null);

            _handlerMock.Protected().Verify(
               "SendAsync",
               Times.Exactly(maxRetry + 1),
               ItExpr.IsAny<HttpRequestMessage>(),
               ItExpr.IsAny<CancellationToken>());
        }

        [TestMethod]
        public async Task RequestSenderTest_DefaultPolicy_SuccessStatus_ShouldNotRetry()
        {
            int maxRetry = 5;

            var defaultPolicy = IoCExtensions.GetDefaultRetryPolicy(maxRetry, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt / 2)));

            InitServices(HttpStatusCode.OK, defaultPolicy);

            await _requestSender.SendAsync(new RequestAddress($"http://{ValueGenerator.String(5)}", ""), null);

            _handlerMock.Protected().Verify(
               "SendAsync",
               Times.Exactly(1),
               ItExpr.IsAny<HttpRequestMessage>(),
               ItExpr.IsAny<CancellationToken>());
        }

        private void InitServices(HttpStatusCode mockStatusCode, IAsyncPolicy<HttpResponseMessage> policy)
        {
            _handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            _handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = mockStatusCode,
                    Content = new StringContent("test")
                });

            _services = new ServiceCollection();

            _services.AddTransient<DefaultMsgSenderWithRetryFactory>();

            _services.AddHttpClient(DefaultMsgSenderWithRetryFactory.RequestSenderHttpClientFactory.HTTP_CLIENT_NAME, x => { x.Timeout = TimeSpan.FromSeconds(210); })
                .AddPolicyHandler(policy)
                .ConfigurePrimaryHttpMessageHandler(() => _handlerMock.Object);

            var httpClientFactory =
                _services
                    .BuildServiceProvider()
                    .GetRequiredService<IHttpClientFactory>();

            var requestSenderHttpClientFactory = new DefaultMsgSenderWithRetryFactory.RequestSenderHttpClientFactory(httpClientFactory);
            var defaultMsgSenderWithRetryFactory = new DefaultMsgSenderWithRetryFactory(requestSenderHttpClientFactory);


            _requestSender = defaultMsgSenderWithRetryFactory.GetRequestSender();
        }
    }
}

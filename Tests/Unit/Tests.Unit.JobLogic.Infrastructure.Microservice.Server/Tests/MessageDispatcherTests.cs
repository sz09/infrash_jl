using FluentAssertions;
using JobLogic.Infrastructure.Microservice.Client;
using JobLogic.Infrastructure.Microservice.Server;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tests.Unit.JobLogic.Infrastructure.Microservice.Server.Tests;

namespace Tests.Unit.JobLogic.Infrastructure.Microservice.Server
{
    [TestClass]
    public class MessageDispatcherTests : BaseUnitTestsWithServiceCollection
    {
        protected override void RegisterServices(IServiceCollection services)
        {
            services.UseTestServices();
        }

        [TestMethod]
        public async Task TestHandle_OperationInfoShouldBeSameForAll_WhenTenancyOperation()
        {
            var provider = GetServiceProvider();

            List<Task> tasks = new List<Task>();

            for (int i = 0; i < 10000; i++)
            {
                var t = Task.Run(async () =>
                {
                    var payload = MicroservicePayload.Create(OperationInfoFactory.CreateTenancy(Guid.NewGuid()), new TenancyTestMessage { });
                    var dispatcher = GetService<IDispatcher>();
                    var result = await dispatcher.DispatchAsync(payload);

                    var service = result.GetInvocationResponseValue<Service<ITenancyOperationInfo>>();

                    service.OperationInfo.Should().NotBeNull();
                    service.OperationInfo.Should().BeSameAs(service.InnerService1.OperationInfo)
                    .And.BeSameAs(service.InnerService2.OperationInfo)
                    .And.BeSameAs(service.InnerService1.InnerInnerService1.OperationInfo)
                    .And.BeSameAs(service.InnerService1.InnerInnerService2.OperationInfo)
                    .And.BeSameAs(service.InnerService2.InnerInnerService1.OperationInfo)
                    .And.BeSameAs(service.InnerService2.InnerInnerService2.OperationInfo);
                });
                tasks.Add(t);
            }
            await Task.WhenAll(tasks);
        }

        [TestMethod]
        public async Task TestHandle_ScopedServiceShouldBeSameForAll_WhenTenancyOperation()
        {
            var provider = GetServiceProvider();

            List<Task> tasks = new List<Task>();

            for (int i = 0; i < 10000; i++)
            {
                var t = Task.Run(async () =>
                {
                    var payload = MicroservicePayload.Create(OperationInfoFactory.CreateTenancy(Guid.NewGuid()), new TenancyTestMessage { });
                    var dispatcher = GetService<IDispatcher>();
                    var result = await dispatcher.DispatchAsync(payload);

                    var service = result.GetInvocationResponseValue<Service<ITenancyOperationInfo>>();

                    service.ScopedInnerMostService.Should().NotBeNull();
                    service.ScopedInnerMostService.Should().BeSameAs(service.InnerService1.ScopedInnerMostService)
                    .And.BeSameAs(service.InnerService2.ScopedInnerMostService)
                    .And.BeSameAs(service.InnerService1.InnerInnerService1.ScopedInnerMostService)
                    .And.BeSameAs(service.InnerService1.InnerInnerService2.ScopedInnerMostService)
                    .And.BeSameAs(service.InnerService2.InnerInnerService1.ScopedInnerMostService)
                    .And.BeSameAs(service.InnerService2.InnerInnerService2.ScopedInnerMostService);
                });
                tasks.Add(t);
            }
            await Task.WhenAll(tasks);
        }

        [TestMethod]
        public async Task TestHandle_ScopedObjectsShouldBeDifferentForEveryDispatch_WhenTenancyOperation()
        {
            var provider = GetServiceProvider();


            var payload1 = MicroservicePayload.Create(OperationInfoFactory.CreateTenancy(Guid.NewGuid()), new TenancyTestMessage { });
            var dispatcher = GetService<IDispatcher>();
            var result1 = await dispatcher.DispatchAsync(payload1);
            var service1 = result1.GetInvocationResponseValue<Service<ITenancyOperationInfo>>();

            var payload2 = MicroservicePayload.Create(OperationInfoFactory.CreateTenancy(Guid.NewGuid()), new TenancyTestMessage { });
            var result2 = await dispatcher.DispatchAsync(payload2);
            var service2 = result2.GetInvocationResponseValue<Service<ITenancyOperationInfo>>();

            service1.ScopedInnerMostService.Should().NotBeSameAs(service2.ScopedInnerMostService);
            service1.OperationInfo.Should().NotBeSameAs(service2.OperationInfo);
        }


        [TestMethod]
        public async Task TestHandle_OperationInfoShouldBeSameForAll_WhenTenantlessOperation()
        {
            var provider = GetServiceProvider();

            List<Task> tasks = new List<Task>();

            for (int i = 0; i < 1000; i++)
            {
                var t = Task.Run(async () =>
                {
                    var payload = MicroservicePayload.Create(OperationInfoFactory.CreateTenantless(), new TenantlessMessage { });
                    var dispatcher = GetService<IDispatcher>();
                    var result = await dispatcher.DispatchAsync(payload);

                    var service = result.GetInvocationResponseValue<Service<ITenantlessOperationInfo>>();

                    service.OperationInfo.Should().NotBeNull();
                    service.OperationInfo.Should().BeSameAs(service.InnerService1.OperationInfo)
                    .And.BeSameAs(service.InnerService2.OperationInfo)
                    .And.BeSameAs(service.InnerService1.InnerInnerService1.OperationInfo)
                    .And.BeSameAs(service.InnerService1.InnerInnerService2.OperationInfo)
                    .And.BeSameAs(service.InnerService2.InnerInnerService1.OperationInfo)
                    .And.BeSameAs(service.InnerService2.InnerInnerService2.OperationInfo);
                });
                tasks.Add(t);
            }
            await Task.WhenAll(tasks);
        }

        [TestMethod]
        public async Task TestHandle_ScopedServiceShouldBeSameForAll_WhenTenantlessOperation()
        {
            var provider = GetServiceProvider();

            List<Task> tasks = new List<Task>();

            for (int i = 0; i < 1000; i++)
            {
                var t = Task.Run(async () =>
                {
                    var payload = MicroservicePayload.Create(OperationInfoFactory.CreateTenantless(), new TenantlessMessage { });
                    var dispatcher = GetService<IDispatcher>();
                    var result = await dispatcher.DispatchAsync(payload);

                    var service = result.GetInvocationResponseValue<Service<ITenantlessOperationInfo>>();

                    service.ScopedInnerMostService.Should().NotBeNull();
                    service.ScopedInnerMostService.Should().BeSameAs(service.InnerService1.ScopedInnerMostService)
                    .And.BeSameAs(service.InnerService2.ScopedInnerMostService)
                    .And.BeSameAs(service.InnerService1.InnerInnerService1.ScopedInnerMostService)
                    .And.BeSameAs(service.InnerService1.InnerInnerService2.ScopedInnerMostService)
                    .And.BeSameAs(service.InnerService2.InnerInnerService1.ScopedInnerMostService)
                    .And.BeSameAs(service.InnerService2.InnerInnerService2.ScopedInnerMostService);
                });
                tasks.Add(t);
            }
            await Task.WhenAll(tasks);
        }

        [TestMethod]
        public async Task TestHandle_ScopedObjectsShouldBeDifferentForEveryDispatch_WhenTenantlessOperation()
        {
            var provider = GetServiceProvider();


            var payload1 = MicroservicePayload.Create(OperationInfoFactory.CreateTenantless(), new TenantlessMessage { });
            var dispatcher = GetService<IDispatcher>();
            var result1 = await dispatcher.DispatchAsync(payload1);
            var service1 = result1.GetInvocationResponseValue<Service<ITenantlessOperationInfo>>();

            var payload2 = MicroservicePayload.Create(OperationInfoFactory.CreateTenantless(), new TenantlessMessage { });
            var result2 = await dispatcher.DispatchAsync(payload2);
            var service2 = result2.GetInvocationResponseValue<Service<ITenantlessOperationInfo>>();

            service1.ScopedInnerMostService.Should().NotBeSameAs(service2.ScopedInnerMostService);
            service1.OperationInfo.Should().NotBeSameAs(service2.OperationInfo);
        }


        [TestMethod]
        public async Task TestDispatchAsync_InvokeCorrectly_WhenUseSimpleForwardMiddleware()
        {
            _serviceCollection.UseTestServices(x => x.Use<SimpleForwardMiddleware>());

            bool isEventTriggered = false;
            var operationInfo = OperationInfoFactory.CreateTenancy(Guid.NewGuid());

            SimpleForwardMiddleware.OnBeforeInvoke += SimpleForwardMiddleware_OnBeforeInvoke;
            var executeService = new Mock<IExecuteService>();
            var provider = GetServiceProvider((typeof(IExecuteService), executeService.Object));

            
            var payload = MicroservicePayload.Create(operationInfo, new TenancyMessageWithIExecuteService { });
            var dispatcher = GetService<IDispatcher>();
            var result = await dispatcher.DispatchAsync(payload);

            executeService.Verify(x => x.DoIt(), Times.Once);
            isEventTriggered.Should().BeTrue();

            SimpleForwardMiddleware.OnBeforeInvoke -= SimpleForwardMiddleware_OnBeforeInvoke;
            void SimpleForwardMiddleware_OnBeforeInvoke(MiddlewareContext obj)
            {
                var op = obj.ServiceProvider.GetService<ITenancyOperationInfo>();
                op.OperationId.Should().Be(operationInfo.OperationId);
                op.TenantId.Should().Be(operationInfo.TenantId);
                isEventTriggered = true;
            }
        }

        

        [TestMethod]
        public async Task TestDispatchAsync_InvokeFailed_WhenUseFailedInvocationMiddleware()
        {
            _serviceCollection.UseTestServices(x => x.Use<FailedInvocationMiddleware>());

            var executeService = new Mock<IExecuteService>();
            var provider = GetServiceProvider((typeof(IExecuteService), executeService.Object));

            var payload = MicroservicePayload.Create(OperationInfoFactory.CreateTenancy(Guid.NewGuid()), new TenancyMessageWithIExecuteService { });
            var dispatcher = GetService<IDispatcher>();
            var result = await dispatcher.DispatchAsync(payload);
            result.IsSuccess.Should().BeFalse();
            executeService.Verify(x => x.DoIt(), Times.Never);
        }

        [TestMethod]
        public async Task TestDispatchAsync_InvokeCorrectly_WhenUseMultipleSuccessMiddleware()
        {
            string sequence = "";
            _serviceCollection.UseTestServices(x =>
            {
                x.Use<Middleware1>();
                x.Use<Middleware2>();
            });
            Middleware1.OnBeforeInvoke += Middleware1_OnBeforeInvoke;
            Middleware2.OnBeforeInvoke += Middleware2_OnBeforeInvoke;
            Middleware1.OnAfterInvoke += Middleware1_OnAfterInvoke; 
            Middleware2.OnAfterInvoke += Middleware2_OnAfterInvoke; 
            var executeService = new Mock<IExecuteService>();
            executeService.Setup(x => x.DoIt()).Returns(() =>
            {
                sequence += "_";
                return Task.CompletedTask;
            });
            var provider = GetServiceProvider((typeof(IExecuteService), executeService.Object));

            var payload = MicroservicePayload.Create(OperationInfoFactory.CreateTenancy(Guid.NewGuid()), new TenancyMessageWithIExecuteService { });
            var dispatcher = GetService<IDispatcher>();
            var result = await dispatcher.DispatchAsync(payload);
            result.IsSuccess.Should().BeTrue();
            executeService.Verify(x => x.DoIt(), Times.Once);
            sequence.Should().Be("12_34");

            Middleware1.OnBeforeInvoke -= Middleware1_OnBeforeInvoke;
            Middleware2.OnBeforeInvoke -= Middleware2_OnBeforeInvoke;
            Middleware1.OnAfterInvoke -= Middleware1_OnAfterInvoke;
            Middleware2.OnAfterInvoke -= Middleware2_OnAfterInvoke;

            void Middleware1_OnBeforeInvoke()
            {
                sequence += "1";
            }

            void Middleware2_OnBeforeInvoke()
            {
                sequence += "2";
            }

            void Middleware2_OnAfterInvoke()
            {
                sequence += "3";
            }

            void Middleware1_OnAfterInvoke()
            {
                sequence += "4";
            }
        }

        [TestMethod]
        public async Task TestDispatchAsync_InvokeFailed_WhenUseMultipleMiddlewareWith1Failed()
        {
            string sequence = "";
            _serviceCollection.UseTestServices(x =>
            {
                x.Use<Middleware1>();
                x.Use<Middleware2>();
                x.Use<FailedInvocationMiddleware>();
            });
            Middleware1.OnBeforeInvoke += Middleware1_OnBeforeInvoke;
            Middleware2.OnBeforeInvoke += Middleware2_OnBeforeInvoke;
            Middleware1.OnAfterInvoke += Middleware1_OnAfterInvoke;
            Middleware2.OnAfterInvoke += Middleware2_OnAfterInvoke;
            var executeService = new Mock<IExecuteService>();
            executeService.Setup(x => x.DoIt()).Returns(() =>
            {
                sequence += "_";
                return Task.CompletedTask;
            });
            var provider = GetServiceProvider((typeof(IExecuteService), executeService.Object));

            var payload = MicroservicePayload.Create(OperationInfoFactory.CreateTenancy(Guid.NewGuid()), new TenancyMessageWithIExecuteService { });
            var dispatcher = GetService<IDispatcher>();
            var result = await dispatcher.DispatchAsync(payload);
            result.IsSuccess.Should().BeFalse();
            executeService.Verify(x => x.DoIt(), Times.Never);
            sequence.Should().Be("1234");

            Middleware1.OnBeforeInvoke -= Middleware1_OnBeforeInvoke;
            Middleware2.OnBeforeInvoke -= Middleware2_OnBeforeInvoke;
            Middleware1.OnAfterInvoke -= Middleware1_OnAfterInvoke;
            Middleware2.OnAfterInvoke -= Middleware2_OnAfterInvoke;

            void Middleware1_OnBeforeInvoke()
            {
                sequence += "1";
            }

            void Middleware2_OnBeforeInvoke()
            {
                sequence += "2";
            }

            void Middleware2_OnAfterInvoke()
            {
                sequence += "3";
            }

            void Middleware1_OnAfterInvoke()
            {
                sequence += "4";
            }
        }

        [TestMethod]
        public async Task TestDispatchAsync_InvokeFailedAtBegining_WhenUseMultipleMiddlewareWithFailedAtBegining()
        {
            string sequence = "";
            _serviceCollection.UseTestServices(x =>
            {
                x.Use<FailedInvocationMiddleware>();
                x.Use<Middleware1>();
                x.Use<Middleware2>();
            });
            Middleware1.OnBeforeInvoke += Middleware1_OnBeforeInvoke;
            Middleware2.OnBeforeInvoke += Middleware2_OnBeforeInvoke;
            Middleware1.OnAfterInvoke += Middleware1_OnAfterInvoke;
            Middleware2.OnAfterInvoke += Middleware2_OnAfterInvoke;
            var executeService = new Mock<IExecuteService>();
            executeService.Setup(x => x.DoIt()).Returns(() =>
            {
                sequence += "_";
                return Task.CompletedTask;
            });
            var provider = GetServiceProvider((typeof(IExecuteService), executeService.Object));

            var payload = MicroservicePayload.Create(OperationInfoFactory.CreateTenancy(Guid.NewGuid()), new TenancyMessageWithIExecuteService { });
            var dispatcher = GetService<IDispatcher>();
            var result = await dispatcher.DispatchAsync(payload);
            result.IsSuccess.Should().BeFalse();
            executeService.Verify(x => x.DoIt(), Times.Never);
            sequence.Should().Be("");

            Middleware1.OnBeforeInvoke -= Middleware1_OnBeforeInvoke;
            Middleware2.OnBeforeInvoke -= Middleware2_OnBeforeInvoke;
            Middleware1.OnAfterInvoke -= Middleware1_OnAfterInvoke;
            Middleware2.OnAfterInvoke -= Middleware2_OnAfterInvoke;

            void Middleware1_OnBeforeInvoke()
            {
                sequence += "1";
            }

            void Middleware2_OnBeforeInvoke()
            {
                sequence += "2";
            }

            void Middleware2_OnAfterInvoke()
            {
                sequence += "3";
            }

            void Middleware1_OnAfterInvoke()
            {
                sequence += "4";
            }
        }

        #region Tenantless middlware

        [TestMethod]
        public async Task TestDispatchAsync_InvokeCorrectly_WhenUseSimpleForwardMiddlewareForTenantless()
        {
            _serviceCollection.UseTestServices(tenantlessBuildAction: x => x.Use<SimpleForwardMiddleware>());

            bool isEventTriggered = false;
            var operationInfo = OperationInfoFactory.CreateTenantless();

            SimpleForwardMiddleware.OnBeforeInvoke += SimpleForwardMiddleware_OnBeforeInvoke;
            var executeService = new Mock<IExecuteService>();
            var provider = GetServiceProvider((typeof(IExecuteService), executeService.Object));


            var payload = MicroservicePayload.Create(operationInfo, new TenantlessMessageWithIExecuteService { });
            var dispatcher = GetService<IDispatcher>();
            var result = await dispatcher.DispatchAsync(payload);

            executeService.Verify(x => x.DoIt(), Times.Once);
            isEventTriggered.Should().BeTrue();

            SimpleForwardMiddleware.OnBeforeInvoke -= SimpleForwardMiddleware_OnBeforeInvoke;
            void SimpleForwardMiddleware_OnBeforeInvoke(MiddlewareContext obj)
            {
                isEventTriggered = true;
            }
        }



        [TestMethod]
        public async Task TestDispatchAsync_InvokeFailed_WhenUseFailedInvocationMiddlewareForTenantless()
        {
            _serviceCollection.UseTestServices(tenantlessBuildAction: x => x.Use<FailedInvocationMiddleware>());

            var executeService = new Mock<IExecuteService>();
            var provider = GetServiceProvider((typeof(IExecuteService), executeService.Object));

            var payload = MicroservicePayload.Create(OperationInfoFactory.CreateTenantless(), new TenantlessMessageWithIExecuteService { });
            var dispatcher = GetService<IDispatcher>();
            var result = await dispatcher.DispatchAsync(payload);
            result.IsSuccess.Should().BeFalse();
            executeService.Verify(x => x.DoIt(), Times.Never);
        }

        [TestMethod]
        public async Task TestDispatchAsync_InvokeCorrectly_WhenUseMultipleSuccessMiddlewareForTenantless()
        {
            string sequence = "";
            _serviceCollection.UseTestServices(tenantlessBuildAction: x =>
             {
                 x.Use<Middleware1>();
                 x.Use<Middleware2>();
             });
            Middleware1.OnBeforeInvoke += Middleware1_OnBeforeInvoke;
            Middleware2.OnBeforeInvoke += Middleware2_OnBeforeInvoke;
            Middleware1.OnAfterInvoke += Middleware1_OnAfterInvoke;
            Middleware2.OnAfterInvoke += Middleware2_OnAfterInvoke;
            var executeService = new Mock<IExecuteService>();
            executeService.Setup(x => x.DoIt()).Returns(() =>
            {
                sequence += "_";
                return Task.CompletedTask;
            });
            var provider = GetServiceProvider((typeof(IExecuteService), executeService.Object));

            var payload = MicroservicePayload.Create(OperationInfoFactory.CreateTenantless(), new TenantlessMessageWithIExecuteService { });
            var dispatcher = GetService<IDispatcher>();
            var result = await dispatcher.DispatchAsync(payload);
            result.IsSuccess.Should().BeTrue();
            executeService.Verify(x => x.DoIt(), Times.Once);
            sequence.Should().Be("12_34");

            Middleware1.OnBeforeInvoke -= Middleware1_OnBeforeInvoke;
            Middleware2.OnBeforeInvoke -= Middleware2_OnBeforeInvoke;
            Middleware1.OnAfterInvoke -= Middleware1_OnAfterInvoke;
            Middleware2.OnAfterInvoke -= Middleware2_OnAfterInvoke;

            void Middleware1_OnBeforeInvoke()
            {
                sequence += "1";
            }

            void Middleware2_OnBeforeInvoke()
            {
                sequence += "2";
            }

            void Middleware2_OnAfterInvoke()
            {
                sequence += "3";
            }

            void Middleware1_OnAfterInvoke()
            {
                sequence += "4";
            }
        }

        [TestMethod]
        public async Task TestDispatchAsync_InvokeFailed_WhenUseMultipleMiddlewareWith1FailedForTenantless()
        {
            string sequence = "";
            _serviceCollection.UseTestServices(tenantlessBuildAction: x =>
             {
                 x.Use<Middleware1>();
                 x.Use<Middleware2>();
                 x.Use<FailedInvocationMiddleware>();
             });
            Middleware1.OnBeforeInvoke += Middleware1_OnBeforeInvoke;
            Middleware2.OnBeforeInvoke += Middleware2_OnBeforeInvoke;
            Middleware1.OnAfterInvoke += Middleware1_OnAfterInvoke;
            Middleware2.OnAfterInvoke += Middleware2_OnAfterInvoke;
            var executeService = new Mock<IExecuteService>();
            executeService.Setup(x => x.DoIt()).Returns(() =>
            {
                sequence += "_";
                return Task.CompletedTask;
            });
            var provider = GetServiceProvider((typeof(IExecuteService), executeService.Object));

            var payload = MicroservicePayload.Create(OperationInfoFactory.CreateTenantless(), new TenantlessMessageWithIExecuteService { });
            var dispatcher = GetService<IDispatcher>();
            var result = await dispatcher.DispatchAsync(payload);
            result.IsSuccess.Should().BeFalse();
            executeService.Verify(x => x.DoIt(), Times.Never);
            sequence.Should().Be("1234");

            Middleware1.OnBeforeInvoke -= Middleware1_OnBeforeInvoke;
            Middleware2.OnBeforeInvoke -= Middleware2_OnBeforeInvoke;
            Middleware1.OnAfterInvoke -= Middleware1_OnAfterInvoke;
            Middleware2.OnAfterInvoke -= Middleware2_OnAfterInvoke;

            void Middleware1_OnBeforeInvoke()
            {
                sequence += "1";
            }

            void Middleware2_OnBeforeInvoke()
            {
                sequence += "2";
            }

            void Middleware2_OnAfterInvoke()
            {
                sequence += "3";
            }

            void Middleware1_OnAfterInvoke()
            {
                sequence += "4";
            }
        }

        [TestMethod]
        public async Task TestDispatchAsync_InvokeFailedAtBegining_WhenUseMultipleMiddlewareWithFailedAtBeginingForTenantless()
        {
            string sequence = "";
            _serviceCollection.UseTestServices(tenantlessBuildAction: x =>
             {
                 x.Use<FailedInvocationMiddleware>();
                 x.Use<Middleware1>();
                 x.Use<Middleware2>();
             });
            Middleware1.OnBeforeInvoke += Middleware1_OnBeforeInvoke;
            Middleware2.OnBeforeInvoke += Middleware2_OnBeforeInvoke;
            Middleware1.OnAfterInvoke += Middleware1_OnAfterInvoke;
            Middleware2.OnAfterInvoke += Middleware2_OnAfterInvoke;
            var executeService = new Mock<IExecuteService>();
            executeService.Setup(x => x.DoIt()).Returns(() =>
            {
                sequence += "_";
                return Task.CompletedTask;
            });
            var provider = GetServiceProvider((typeof(IExecuteService), executeService.Object));

            var payload = MicroservicePayload.Create(OperationInfoFactory.CreateTenantless(), new TenantlessMessageWithIExecuteService { });
            var dispatcher = GetService<IDispatcher>();
            var result = await dispatcher.DispatchAsync(payload);
            result.IsSuccess.Should().BeFalse();
            executeService.Verify(x => x.DoIt(), Times.Never);
            sequence.Should().Be("");

            Middleware1.OnBeforeInvoke -= Middleware1_OnBeforeInvoke;
            Middleware2.OnBeforeInvoke -= Middleware2_OnBeforeInvoke;
            Middleware1.OnAfterInvoke -= Middleware1_OnAfterInvoke;
            Middleware2.OnAfterInvoke -= Middleware2_OnAfterInvoke;

            void Middleware1_OnBeforeInvoke()
            {
                sequence += "1";
            }

            void Middleware2_OnBeforeInvoke()
            {
                sequence += "2";
            }

            void Middleware2_OnAfterInvoke()
            {
                sequence += "3";
            }

            void Middleware1_OnAfterInvoke()
            {
                sequence += "4";
            }
        }

        #endregion

        [TestMethod]
        public async Task TestDispatchAsync_RunOk_WhenUseTeantlessOnly()
        {
            _serviceCollection.UseTenantlessOnlyTestServices();

            var executeService = new Mock<IExecuteService>();
            var provider = GetServiceProvider((typeof(IExecuteService), executeService.Object));

            var payload = MicroservicePayload.Create(OperationInfoFactory.CreateTenantless(), new TenantlessMessageWithIExecuteService { });
            var dispatcher = GetService<IDispatcher>();
            var result = await dispatcher.DispatchAsync(payload);
            result.IsSuccess.Should().BeTrue();
            executeService.Verify(x => x.DoIt(), Times.Once);
        }

        [TestMethod]
        public async Task TestDispatchAsync_RunOk_WhenUseTenancyOnly()
        {
            _serviceCollection.UseTenancyOnlyTestServices();

            var executeService = new Mock<IExecuteService>();
            var provider = GetServiceProvider((typeof(IExecuteService), executeService.Object));

            var payload = MicroservicePayload.Create(OperationInfoFactory.CreateTenancy(Guid.NewGuid()), new TenancyMessageWithIExecuteService { });
            var dispatcher = GetService<IDispatcher>();
            var result = await dispatcher.DispatchAsync(payload);
            result.IsSuccess.Should().BeTrue();
            executeService.Verify(x => x.DoIt(), Times.Once);
        }
    }
}

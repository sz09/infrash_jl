//using FluentAssertions;
//using JobLogic.Infrastructure.Contract;
//using JobLogic.Infrastructure.Microservice.Server;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Moq;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Threading.Tasks;

//namespace Tests.Unit.JobLogic.Infrastructure.Microservice.Server.Tests
//{
//    [TestClass]
//    public class EventSubscriberTests : BaseUnitTestsWithServiceCollection
//    {
//        protected override void RegisterServices(IServiceCollection services)
//        {
//            services.UseTestServices();
//        }

//        [TestMethod]
//        public async Task TestSubscribe_ShouldInvokeProperly_ForTenancyEvent()
//        {
//            var executeService = new Mock<IExecuteService>();
//            var provider = GetServiceProvider((typeof(IExecuteService), executeService.Object));

//            List<Task> tasks = new List<Task>();

//            var payload = MicroservicePayload.Create(OperationInfoFactory.CreateTenancy(Guid.NewGuid()), new TenancyTestEvent { });
//            var evenSubscriber = GetService<IEventSubscriber>();

//            await evenSubscriber.SubscribeAsync(payload);

//            executeService.Verify(x => x.DoIt(It.IsAny<Service<ITenancyOperationInfo>>()), Times.Once);
//        }

//        [TestMethod]
//        public async Task TestSubscribe_OperationInfoShouldBeSameForAll_WhenTenancyOperation()
//        {
//            var executeService = new Mock<IExecuteService>();
//            executeService.Setup(x => x.DoIt(It.IsAny<Service<ITenancyOperationInfo>>())).Returns<Service<ITenancyOperationInfo>>(service =>
//            {
//                service.OperationInfo.Should().NotBeNull();
//                service.OperationInfo.Should().BeSameAs(service.InnerService1.OperationInfo)
//                .And.BeSameAs(service.InnerService2.OperationInfo)
//                .And.BeSameAs(service.InnerService1.InnerInnerService1.OperationInfo)
//                .And.BeSameAs(service.InnerService1.InnerInnerService2.OperationInfo)
//                .And.BeSameAs(service.InnerService2.InnerInnerService1.OperationInfo)
//                .And.BeSameAs(service.InnerService2.InnerInnerService2.OperationInfo);
//                return Task.CompletedTask;
//            });
//            var provider = GetServiceProvider((typeof(IExecuteService), executeService.Object));

//            List<Task> tasks = new List<Task>();

//            for (int i = 0; i < 10000; i++)
//            {
//                var t = Task.Run(async () =>
//                {
//                    var payload = MicroservicePayload.Create(OperationInfoFactory.CreateTenancy(Guid.NewGuid()), new TenancyTestEvent { });
//                    var dispatcher = GetService<IEventSubscriber>();
//                    await dispatcher.SubscribeAsync(payload);
//                });
//                tasks.Add(t);
//            }
//            await Task.WhenAll(tasks);
//        }


//        [TestMethod]
//        public async Task TestSubscribe_ScopedServiceShouldBeSameForAll_WhenTenancyOperation()
//        {
//            var executeService = new Mock<IExecuteService>();
//            executeService.Setup(x => x.DoIt(It.IsAny<Service<ITenancyOperationInfo>>())).Returns<Service<ITenancyOperationInfo>>(service =>
//            {
//                service.ScopedInnerMostService.Should().NotBeNull();
//                service.ScopedInnerMostService.Should().BeSameAs(service.InnerService1.ScopedInnerMostService)
//                .And.BeSameAs(service.InnerService2.ScopedInnerMostService)
//                .And.BeSameAs(service.InnerService1.InnerInnerService1.ScopedInnerMostService)
//                .And.BeSameAs(service.InnerService1.InnerInnerService2.ScopedInnerMostService)
//                .And.BeSameAs(service.InnerService2.InnerInnerService1.ScopedInnerMostService)
//                .And.BeSameAs(service.InnerService2.InnerInnerService2.ScopedInnerMostService);
//                return Task.CompletedTask;
//            });
//            var provider = GetServiceProvider((typeof(IExecuteService), executeService.Object));

//            List<Task> tasks = new List<Task>();

//            for (int i = 0; i < 10000; i++)
//            {
//                var t = Task.Run(async () =>
//                {
//                    var payload = MicroservicePayload.Create(OperationInfoFactory.CreateTenancy(Guid.NewGuid()), new TenancyTestEvent { });
//                    var dispatcher = GetService<IEventSubscriber>();
//                    await dispatcher.SubscribeAsync(payload);
//                });
//                tasks.Add(t);
//            }
//            await Task.WhenAll(tasks);
//        }


//        [TestMethod]
//        public async Task TestSubscribe_ScopedObjectsShouldBeDifferentForEveryDispatch_WhenTenancyOperation()
//        {
//            List<ITenancyOperationInfo> tenancyOperationInfos = new List<ITenancyOperationInfo>();
//            List<ScopedInnerMostService<ITenancyOperationInfo>> scopedInnerMostServices = new List<ScopedInnerMostService<ITenancyOperationInfo>>();

//            var executeService = new Mock<IExecuteService>();
//            executeService.Setup(x => x.DoIt(It.IsAny<Service<ITenancyOperationInfo>>())).Returns<Service<ITenancyOperationInfo>>(service =>
//            {
//                scopedInnerMostServices.Add(service.ScopedInnerMostService);
//                tenancyOperationInfos.Add(service.OperationInfo);
//                return Task.CompletedTask;
//            });
//            var provider = GetServiceProvider((typeof(IExecuteService), executeService.Object));


//            var payload1 = MicroservicePayload.Create(OperationInfoFactory.CreateTenancy(Guid.NewGuid()), new TenancyTestEvent { });
//            var dispatcher = GetService<IEventSubscriber>();
//            await dispatcher.SubscribeAsync(payload1);

//            var payload2 = MicroservicePayload.Create(OperationInfoFactory.CreateTenancy(Guid.NewGuid()), new TenancyTestEvent { });
//            await dispatcher.SubscribeAsync(payload2);


//            scopedInnerMostServices[0].Should().NotBeSameAs(scopedInnerMostServices[1]);
//            tenancyOperationInfos[0].Should().NotBeSameAs(tenancyOperationInfos[1]);
//        }



//        [TestMethod]
//        public async Task TestSubscribe_ShouldInvokeProperly_ForTenantlessEvent()
//        {
//            var executeService = new Mock<ITenantlessExecuteService>();
//            var provider = GetServiceProvider((typeof(ITenantlessExecuteService), executeService.Object));

//            List<Task> tasks = new List<Task>();

//            var payload = MicroservicePayload.Create(OperationInfoFactory.CreateTenantless(), new TenantlessTestEvent { });
//            var evenSubscriber = GetService<IEventSubscriber>();

//            await evenSubscriber.SubscribeAsync(payload);

//            executeService.Verify(x => x.DoIt(It.IsAny<Service<ITenantlessOperationInfo>>()), Times.Once);
//        }

//        [TestMethod]
//        public async Task TestSubscribe_OperationInfoShouldBeSameForAll_WhenTenantlessOperation()
//        {
//            var executeService = new Mock<ITenantlessExecuteService>();
//            executeService.Setup(x => x.DoIt(It.IsAny<Service<ITenantlessOperationInfo>>())).Returns<Service<ITenantlessOperationInfo>>(service =>
//            {
//                service.OperationInfo.Should().NotBeNull();
//                service.OperationInfo.Should().BeSameAs(service.InnerService1.OperationInfo)
//                .And.BeSameAs(service.InnerService2.OperationInfo)
//                .And.BeSameAs(service.InnerService1.InnerInnerService1.OperationInfo)
//                .And.BeSameAs(service.InnerService1.InnerInnerService2.OperationInfo)
//                .And.BeSameAs(service.InnerService2.InnerInnerService1.OperationInfo)
//                .And.BeSameAs(service.InnerService2.InnerInnerService2.OperationInfo);
//                return Task.CompletedTask;
//            });
//            var provider = GetServiceProvider((typeof(ITenantlessExecuteService), executeService.Object));

//            List<Task> tasks = new List<Task>();

//            for (int i = 0; i < 10000; i++)
//            {
//                var t = Task.Run(async () =>
//                {
//                    var payload = MicroservicePayload.Create(OperationInfoFactory.CreateTenantless(), new TenantlessTestEvent { });
//                    var dispatcher = GetService<IEventSubscriber>();
//                    await dispatcher.SubscribeAsync(payload);
//                });
//                tasks.Add(t);
//            }
//            await Task.WhenAll(tasks);
//        }

//        [TestMethod]
//        public async Task TestSubscribe_ScopedServiceShouldBeSameForAll_WhenTenantlessOperation()
//        {
//            var executeService = new Mock<ITenantlessExecuteService>();
//            executeService.Setup(x => x.DoIt(It.IsAny<Service<ITenantlessOperationInfo>>())).Returns<Service<ITenantlessOperationInfo>>(service =>
//            {
//                service.ScopedInnerMostService.Should().NotBeNull();
//                service.ScopedInnerMostService.Should().BeSameAs(service.InnerService1.ScopedInnerMostService)
//                .And.BeSameAs(service.InnerService2.ScopedInnerMostService)
//                .And.BeSameAs(service.InnerService1.InnerInnerService1.ScopedInnerMostService)
//                .And.BeSameAs(service.InnerService1.InnerInnerService2.ScopedInnerMostService)
//                .And.BeSameAs(service.InnerService2.InnerInnerService1.ScopedInnerMostService)
//                .And.BeSameAs(service.InnerService2.InnerInnerService2.ScopedInnerMostService);
//                return Task.CompletedTask;
//            });
//            var provider = GetServiceProvider((typeof(ITenantlessExecuteService), executeService.Object));

//            List<Task> tasks = new List<Task>();

//            for (int i = 0; i < 10000; i++)
//            {
//                var t = Task.Run(async () =>
//                {
//                    var payload = MicroservicePayload.Create(OperationInfoFactory.CreateTenantless(), new TenantlessTestEvent { });
//                    var dispatcher = GetService<IEventSubscriber>();
//                    await dispatcher.SubscribeAsync(payload);
//                });
//                tasks.Add(t);
//            }
//            await Task.WhenAll(tasks);
//        }


//        [TestMethod]
//        public async Task TestSubscribe_ScopedObjectsShouldBeDifferentForEveryDispatch_WhenTenantlessOperation()
//        {
//            List<ITenantlessOperationInfo> tenancyOperationInfos = new List<ITenantlessOperationInfo>();
//            List<ScopedInnerMostService<ITenantlessOperationInfo>> scopedInnerMostServices = new List<ScopedInnerMostService<ITenantlessOperationInfo>>();

//            var executeService = new Mock<ITenantlessExecuteService>();
//            executeService.Setup(x => x.DoIt(It.IsAny<Service<ITenantlessOperationInfo>>())).Returns<Service<ITenantlessOperationInfo>>(service =>
//            {
//                scopedInnerMostServices.Add(service.ScopedInnerMostService);
//                tenancyOperationInfos.Add(service.OperationInfo);
//                return Task.CompletedTask;
//            });
//            var provider = GetServiceProvider((typeof(ITenantlessExecuteService), executeService.Object));


//            var payload1 = MicroservicePayload.Create(OperationInfoFactory.CreateTenantless(), new TenantlessTestEvent { });
//            var dispatcher = GetService<IEventSubscriber>();
//            await dispatcher.SubscribeAsync(payload1);

//            var payload2 = MicroservicePayload.Create(OperationInfoFactory.CreateTenantless(), new TenantlessTestEvent { });
//            await dispatcher.SubscribeAsync(payload2);


//            scopedInnerMostServices[0].Should().NotBeSameAs(scopedInnerMostServices[1]);
//            tenancyOperationInfos[0].Should().NotBeSameAs(tenancyOperationInfos[1]);
//        }

//        [TestMethod]
//        public async Task TestSubscribe_ShouldInvokeProperlyOnBoth_WhenTeanacyEventIsSubscribedWithTenantlessAdapter()
//        {
//            var executeService = new Mock<IExecuteService>();
//            var tenantlessExecuteService = new Mock<ITenantlessExecuteService>();
//            var provider = GetServiceProvider((typeof(IExecuteService), executeService.Object), (typeof(ITenantlessExecuteService), tenantlessExecuteService.Object));


//            var payload = MicroservicePayload.Create(OperationInfoFactory.CreateTenancy(Guid.NewGuid()), new TenancyAdapterEvent { });
//            var evenSubscriber = GetService<ITenantlessAdaptedEventSubscriber>();

//            await evenSubscriber.SubscribeAsync(payload);

//            tenantlessExecuteService.Verify(x => x.DoIt(It.IsAny<Service<ITenantlessOperationInfo>>()), Times.Once);
//        }

//        [TestMethod]
//        public async Task TestSubscribe_ShouldInvokeProperly_WhenTeanacyEventIsSubscribedWithTenantlessAdapterOnlyAndNoTenancySubcriber()
//        {
//            var executeService = new Mock<IExecuteService>();
//            var tenantlessExecuteService = new Mock<ITenantlessExecuteService>();
//            var provider = GetServiceProvider((typeof(IExecuteService), executeService.Object), (typeof(ITenantlessExecuteService), tenantlessExecuteService.Object));


//            var payload = MicroservicePayload.Create(OperationInfoFactory.CreateTenancy(Guid.NewGuid()), new TenancyAdapterForTenantlessOnlyEvent { });
//            var evenSubscriber = GetService<ITenantlessAdaptedEventSubscriber>();

//            await evenSubscriber.SubscribeAsync(payload);

//            executeService.Verify(x => x.DoIt(It.IsAny<Service<ITenancyOperationInfo>>()), Times.Never);
//            tenantlessExecuteService.Verify(x => x.DoIt(It.IsAny<Service<ITenantlessOperationInfo>>()), Times.Once);
//        }

//        #region tenancy middleware

//        [TestMethod]
//        public async Task TestSubscribe_InvokeCorrectly_WhenUseSimpleForwardMiddleware()
//        {
//            _serviceCollection.UseTestServices(x => x.Use<SimpleForwardMiddleware>());

//            bool isEventTriggered = false;
//            var operationInfo = OperationInfoFactory.CreateTenancy(Guid.NewGuid());

//            SimpleForwardMiddleware.OnBeforeInvoke += SimpleForwardMiddleware_OnBeforeInvoke;
//            var executeService = new Mock<IExecuteService>();
//            var provider = GetServiceProvider((typeof(IExecuteService), executeService.Object));


//            var payload = MicroservicePayload.Create(operationInfo, new TenancyEventWithIExecuteService { });
//            var subscriber = GetService<IEventSubscriber>();
//            var result = await subscriber.SubscribeAsync(payload);

//            executeService.Verify(x => x.DoIt(), Times.Once);
//            isEventTriggered.Should().BeTrue();

//            SimpleForwardMiddleware.OnBeforeInvoke -= SimpleForwardMiddleware_OnBeforeInvoke;
//            void SimpleForwardMiddleware_OnBeforeInvoke(MiddlewareContext obj)
//            {
//                var op = obj.ServiceProvider.GetService<ITenancyOperationInfo>();
//                op.OperationId.Should().Be(operationInfo.OperationId);
//                op.TenantId.Should().Be(operationInfo.TenantId);
//                isEventTriggered = true;
//            }
//        }



//        [TestMethod]
//        public async Task TestSubscribe_InvokeFailed_WhenUseFailedInvocationMiddleware()
//        {
//            _serviceCollection.UseTestServices(x => x.Use<FailedInvocationMiddleware>());

//            var executeService = new Mock<IExecuteService>();
//            var provider = GetServiceProvider((typeof(IExecuteService), executeService.Object));

//            var payload = MicroservicePayload.Create(OperationInfoFactory.CreateTenancy(Guid.NewGuid()), new TenancyEventWithIExecuteService { });
//            var subscriber = GetService<IEventSubscriber>();
//            var result = await subscriber.SubscribeAsync(payload);
//            result.IsSuccess.Should().BeFalse();
//            executeService.Verify(x => x.DoIt(), Times.Never);
//        }

//        [TestMethod]
//        public async Task TestSubscribe_InvokeCorrectly_WhenUseMultipleSuccessMiddleware()
//        {
//            string sequence = "";
//            _serviceCollection.UseTestServices(x =>
//            {
//                x.Use<Middleware1>();
//                x.Use<Middleware2>();
//            });
//            Middleware1.OnBeforeInvoke += Middleware1_OnBeforeInvoke;
//            Middleware2.OnBeforeInvoke += Middleware2_OnBeforeInvoke;
//            Middleware1.OnAfterInvoke += Middleware1_OnAfterInvoke;
//            Middleware2.OnAfterInvoke += Middleware2_OnAfterInvoke;
//            var executeService = new Mock<IExecuteService>();
//            executeService.Setup(x => x.DoIt()).Returns(() =>
//            {
//                sequence += "_";
//                return Task.CompletedTask;
//            });
//            var provider = GetServiceProvider((typeof(IExecuteService), executeService.Object));

//            var payload = MicroservicePayload.Create(OperationInfoFactory.CreateTenancy(Guid.NewGuid()), new TenancyEventWithIExecuteService { });
//            var subscriber = GetService<IEventSubscriber>();
//            var result = await subscriber.SubscribeAsync(payload);
//            result.IsSuccess.Should().BeTrue();
//            executeService.Verify(x => x.DoIt(), Times.Once);
//            sequence.Should().Be("12_34");

//            Middleware1.OnBeforeInvoke -= Middleware1_OnBeforeInvoke;
//            Middleware2.OnBeforeInvoke -= Middleware2_OnBeforeInvoke;
//            Middleware1.OnAfterInvoke -= Middleware1_OnAfterInvoke;
//            Middleware2.OnAfterInvoke -= Middleware2_OnAfterInvoke;

//            void Middleware1_OnBeforeInvoke()
//            {
//                sequence += "1";
//            }

//            void Middleware2_OnBeforeInvoke()
//            {
//                sequence += "2";
//            }

//            void Middleware2_OnAfterInvoke()
//            {
//                sequence += "3";
//            }

//            void Middleware1_OnAfterInvoke()
//            {
//                sequence += "4";
//            }
//        }

//        [TestMethod]
//        public async Task TestSubscribe_InvokeFailed_WhenUseMultipleMiddlewareWith1Failed()
//        {
//            string sequence = "";
//            _serviceCollection.UseTestServices(x =>
//            {
//                x.Use<Middleware1>();
//                x.Use<Middleware2>();
//                x.Use<FailedInvocationMiddleware>();
//            });
//            Middleware1.OnBeforeInvoke += Middleware1_OnBeforeInvoke;
//            Middleware2.OnBeforeInvoke += Middleware2_OnBeforeInvoke;
//            Middleware1.OnAfterInvoke += Middleware1_OnAfterInvoke;
//            Middleware2.OnAfterInvoke += Middleware2_OnAfterInvoke;
//            var executeService = new Mock<IExecuteService>();
//            executeService.Setup(x => x.DoIt()).Returns(() =>
//            {
//                sequence += "_";
//                return Task.CompletedTask;
//            });
//            var provider = GetServiceProvider((typeof(IExecuteService), executeService.Object));

//            var payload = MicroservicePayload.Create(OperationInfoFactory.CreateTenancy(Guid.NewGuid()), new TenancyEventWithIExecuteService { });
//            var subscriber = GetService<IEventSubscriber>();
//            var result = await subscriber.SubscribeAsync(payload);
//            result.IsSuccess.Should().BeFalse();
//            executeService.Verify(x => x.DoIt(), Times.Never);
//            sequence.Should().Be("1234");

//            Middleware1.OnBeforeInvoke -= Middleware1_OnBeforeInvoke;
//            Middleware2.OnBeforeInvoke -= Middleware2_OnBeforeInvoke;
//            Middleware1.OnAfterInvoke -= Middleware1_OnAfterInvoke;
//            Middleware2.OnAfterInvoke -= Middleware2_OnAfterInvoke;

//            void Middleware1_OnBeforeInvoke()
//            {
//                sequence += "1";
//            }

//            void Middleware2_OnBeforeInvoke()
//            {
//                sequence += "2";
//            }

//            void Middleware2_OnAfterInvoke()
//            {
//                sequence += "3";
//            }

//            void Middleware1_OnAfterInvoke()
//            {
//                sequence += "4";
//            }
//        }

//        [TestMethod]
//        public async Task TestSubscribe_InvokeFailedAtBegining_WhenUseMultipleMiddlewareWithFailedAtBegining()
//        {
//            string sequence = "";
//            _serviceCollection.UseTestServices(x =>
//            {
//                x.Use<FailedInvocationMiddleware>();
//                x.Use<Middleware1>();
//                x.Use<Middleware2>();
//            });
//            Middleware1.OnBeforeInvoke += Middleware1_OnBeforeInvoke;
//            Middleware2.OnBeforeInvoke += Middleware2_OnBeforeInvoke;
//            Middleware1.OnAfterInvoke += Middleware1_OnAfterInvoke;
//            Middleware2.OnAfterInvoke += Middleware2_OnAfterInvoke;
//            var executeService = new Mock<IExecuteService>();
//            executeService.Setup(x => x.DoIt()).Returns(() =>
//            {
//                sequence += "_";
//                return Task.CompletedTask;
//            });
//            var provider = GetServiceProvider((typeof(IExecuteService), executeService.Object));

//            var payload = MicroservicePayload.Create(OperationInfoFactory.CreateTenancy(Guid.NewGuid()), new TenancyEventWithIExecuteService { });
//            var subscriber = GetService<IEventSubscriber>();
//            var result = await subscriber.SubscribeAsync(payload);
//            result.IsSuccess.Should().BeFalse();
//            executeService.Verify(x => x.DoIt(), Times.Never);
//            sequence.Should().Be("");

//            Middleware1.OnBeforeInvoke -= Middleware1_OnBeforeInvoke;
//            Middleware2.OnBeforeInvoke -= Middleware2_OnBeforeInvoke;
//            Middleware1.OnAfterInvoke -= Middleware1_OnAfterInvoke;
//            Middleware2.OnAfterInvoke -= Middleware2_OnAfterInvoke;

//            void Middleware1_OnBeforeInvoke()
//            {
//                sequence += "1";
//            }

//            void Middleware2_OnBeforeInvoke()
//            {
//                sequence += "2";
//            }

//            void Middleware2_OnAfterInvoke()
//            {
//                sequence += "3";
//            }

//            void Middleware1_OnAfterInvoke()
//            {
//                sequence += "4";
//            }
//        }

//        #endregion

//        #region Tenantless middlware

//        [TestMethod]
//        public async Task TestSubscribe_InvokeCorrectly_WhenUseSimpleForwardMiddlewareForTenantless()
//        {
//            _serviceCollection.UseTestServices(tenantlessBuildAction: x => x.Use<SimpleForwardMiddleware>());

//            bool isEventTriggered = false;
//            var operationInfo = OperationInfoFactory.CreateTenantless();

//            SimpleForwardMiddleware.OnBeforeInvoke += SimpleForwardMiddleware_OnBeforeInvoke;
//            var executeService = new Mock<IExecuteService>();
//            var provider = GetServiceProvider((typeof(IExecuteService), executeService.Object));


//            var payload = MicroservicePayload.Create(operationInfo, new TenantlessEventWithIExecuteService { });
//            var subscriber = GetService<IEventSubscriber>();
//            var result = await subscriber.SubscribeAsync(payload);

//            executeService.Verify(x => x.DoIt(), Times.Once);
//            isEventTriggered.Should().BeTrue();

//            SimpleForwardMiddleware.OnBeforeInvoke -= SimpleForwardMiddleware_OnBeforeInvoke;
//            void SimpleForwardMiddleware_OnBeforeInvoke(MiddlewareContext obj)
//            {
//                isEventTriggered = true;
//            }
//        }



//        [TestMethod]
//        public async Task TestSubscribe_InvokeFailed_WhenUseFailedInvocationMiddlewareForTenantless()
//        {
//            _serviceCollection.UseTestServices(tenantlessBuildAction: x => x.Use<FailedInvocationMiddleware>());

//            var executeService = new Mock<IExecuteService>();
//            var provider = GetServiceProvider((typeof(IExecuteService), executeService.Object));

//            var payload = MicroservicePayload.Create(OperationInfoFactory.CreateTenantless(), new TenantlessEventWithIExecuteService { });
//            var subscriber = GetService<IEventSubscriber>();
//            var result = await subscriber.SubscribeAsync(payload);
//            result.IsSuccess.Should().BeFalse();
//            executeService.Verify(x => x.DoIt(), Times.Never);
//        }

//        [TestMethod]
//        public async Task TestSubscribe_InvokeCorrectly_WhenUseMultipleSuccessMiddlewareForTenantless()
//        {
//            string sequence = "";
//            _serviceCollection.UseTestServices(tenantlessBuildAction: x =>
//            {
//                x.Use<Middleware1>();
//                x.Use<Middleware2>();
//            });
//            Middleware1.OnBeforeInvoke += Middleware1_OnBeforeInvoke;
//            Middleware2.OnBeforeInvoke += Middleware2_OnBeforeInvoke;
//            Middleware1.OnAfterInvoke += Middleware1_OnAfterInvoke;
//            Middleware2.OnAfterInvoke += Middleware2_OnAfterInvoke;
//            var executeService = new Mock<IExecuteService>();
//            executeService.Setup(x => x.DoIt()).Returns(() =>
//            {
//                sequence += "_";
//                return Task.CompletedTask;
//            });
//            var provider = GetServiceProvider((typeof(IExecuteService), executeService.Object));

//            var payload = MicroservicePayload.Create(OperationInfoFactory.CreateTenantless(), new TenantlessEventWithIExecuteService { });
//            var subscriber = GetService<IEventSubscriber>();
//            var result = await subscriber.SubscribeAsync(payload);
//            result.IsSuccess.Should().BeTrue();
//            executeService.Verify(x => x.DoIt(), Times.Once);
//            sequence.Should().Be("12_34");

//            Middleware1.OnBeforeInvoke -= Middleware1_OnBeforeInvoke;
//            Middleware2.OnBeforeInvoke -= Middleware2_OnBeforeInvoke;
//            Middleware1.OnAfterInvoke -= Middleware1_OnAfterInvoke;
//            Middleware2.OnAfterInvoke -= Middleware2_OnAfterInvoke;

//            void Middleware1_OnBeforeInvoke()
//            {
//                sequence += "1";
//            }

//            void Middleware2_OnBeforeInvoke()
//            {
//                sequence += "2";
//            }

//            void Middleware2_OnAfterInvoke()
//            {
//                sequence += "3";
//            }

//            void Middleware1_OnAfterInvoke()
//            {
//                sequence += "4";
//            }
//        }

//        [TestMethod]
//        public async Task TestSubscribe_InvokeFailed_WhenUseMultipleMiddlewareWith1FailedForTenantless()
//        {
//            string sequence = "";
//            _serviceCollection.UseTestServices(tenantlessBuildAction: x =>
//            {
//                x.Use<Middleware1>();
//                x.Use<Middleware2>();
//                x.Use<FailedInvocationMiddleware>();
//            });
//            Middleware1.OnBeforeInvoke += Middleware1_OnBeforeInvoke;
//            Middleware2.OnBeforeInvoke += Middleware2_OnBeforeInvoke;
//            Middleware1.OnAfterInvoke += Middleware1_OnAfterInvoke;
//            Middleware2.OnAfterInvoke += Middleware2_OnAfterInvoke;
//            var executeService = new Mock<IExecuteService>();
//            executeService.Setup(x => x.DoIt()).Returns(() =>
//            {
//                sequence += "_";
//                return Task.CompletedTask;
//            });
//            var provider = GetServiceProvider((typeof(IExecuteService), executeService.Object));

//            var payload = MicroservicePayload.Create(OperationInfoFactory.CreateTenantless(), new TenantlessEventWithIExecuteService { });
//            var subscriber = GetService<IEventSubscriber>();
//            var result = await subscriber.SubscribeAsync(payload);
//            result.IsSuccess.Should().BeFalse();
//            executeService.Verify(x => x.DoIt(), Times.Never);
//            sequence.Should().Be("1234");

//            Middleware1.OnBeforeInvoke -= Middleware1_OnBeforeInvoke;
//            Middleware2.OnBeforeInvoke -= Middleware2_OnBeforeInvoke;
//            Middleware1.OnAfterInvoke -= Middleware1_OnAfterInvoke;
//            Middleware2.OnAfterInvoke -= Middleware2_OnAfterInvoke;

//            void Middleware1_OnBeforeInvoke()
//            {
//                sequence += "1";
//            }

//            void Middleware2_OnBeforeInvoke()
//            {
//                sequence += "2";
//            }

//            void Middleware2_OnAfterInvoke()
//            {
//                sequence += "3";
//            }

//            void Middleware1_OnAfterInvoke()
//            {
//                sequence += "4";
//            }
//        }

//        [TestMethod]
//        public async Task TestSubscribe_InvokeFailedAtBegining_WhenUseMultipleMiddlewareWithFailedAtBeginingForTenantless()
//        {
//            string sequence = "";
//            _serviceCollection.UseTestServices(tenantlessBuildAction: x =>
//            {
//                x.Use<FailedInvocationMiddleware>();
//                x.Use<Middleware1>();
//                x.Use<Middleware2>();
//            });
//            Middleware1.OnBeforeInvoke += Middleware1_OnBeforeInvoke;
//            Middleware2.OnBeforeInvoke += Middleware2_OnBeforeInvoke;
//            Middleware1.OnAfterInvoke += Middleware1_OnAfterInvoke;
//            Middleware2.OnAfterInvoke += Middleware2_OnAfterInvoke;
//            var executeService = new Mock<IExecuteService>();
//            executeService.Setup(x => x.DoIt()).Returns(() =>
//            {
//                sequence += "_";
//                return Task.CompletedTask;
//            });
//            var provider = GetServiceProvider((typeof(IExecuteService), executeService.Object));

//            var payload = MicroservicePayload.Create(OperationInfoFactory.CreateTenantless(), new TenantlessEventWithIExecuteService { });
//            var subscriber = GetService<IEventSubscriber>();
//            var result = await subscriber.SubscribeAsync(payload);
//            result.IsSuccess.Should().BeFalse();
//            executeService.Verify(x => x.DoIt(), Times.Never);
//            sequence.Should().Be("");

//            Middleware1.OnBeforeInvoke -= Middleware1_OnBeforeInvoke;
//            Middleware2.OnBeforeInvoke -= Middleware2_OnBeforeInvoke;
//            Middleware1.OnAfterInvoke -= Middleware1_OnAfterInvoke;
//            Middleware2.OnAfterInvoke -= Middleware2_OnAfterInvoke;

//            void Middleware1_OnBeforeInvoke()
//            {
//                sequence += "1";
//            }

//            void Middleware2_OnBeforeInvoke()
//            {
//                sequence += "2";
//            }

//            void Middleware2_OnAfterInvoke()
//            {
//                sequence += "3";
//            }

//            void Middleware1_OnAfterInvoke()
//            {
//                sequence += "4";
//            }
//        }

//        #endregion

//        #region tenantlessAdapter middleware
//        [TestMethod]
//        public async Task TestSubscribe_InvokeCorrectly_WhenUseSimpleForwardMiddlewareForTenantlessApdatedEvent()
//        {
//            _serviceCollection.UseTestServices(tenantlessBuildAction: x => x.Use<SimpleForwardMiddleware>());

//            bool isEventTriggered = false;
//            var operationInfo = OperationInfoFactory.CreateTenancy(Guid.NewGuid());

//            SimpleForwardMiddleware.OnBeforeInvoke += SimpleForwardMiddleware_OnBeforeInvoke;
//            var executeService = new Mock<IExecuteService>();
//            var provider = GetServiceProvider((typeof(IExecuteService), executeService.Object));


//            var payload = MicroservicePayload.Create(operationInfo, new TenancyEventWithIExecuteService { });
//            var subscriber = GetService<ITenantlessAdaptedEventSubscriber>();
//            var result = await subscriber.SubscribeAsync(payload);

//            executeService.Verify(x => x.DoIt(), Times.Once);
//            isEventTriggered.Should().BeTrue();

//            SimpleForwardMiddleware.OnBeforeInvoke -= SimpleForwardMiddleware_OnBeforeInvoke;
//            void SimpleForwardMiddleware_OnBeforeInvoke(MiddlewareContext obj)
//            {
//                isEventTriggered = true;
//            }
//        }



//        [TestMethod]
//        public async Task TestSubscribe_InvokeFailed_WhenUseFailedInvocationMiddlewareForTenantlessApdatedEvent()
//        {
//            _serviceCollection.UseTestServices(tenantlessBuildAction: x => x.Use<FailedInvocationMiddleware>());

//            var executeService = new Mock<IExecuteService>();
//            var provider = GetServiceProvider((typeof(IExecuteService), executeService.Object));

//            var payload = MicroservicePayload.Create(OperationInfoFactory.CreateTenancy(Guid.NewGuid()), new TenancyEventWithIExecuteService { });
//            var subscriber = GetService<ITenantlessAdaptedEventSubscriber>();
//            var result = await subscriber.SubscribeAsync(payload);
//            result.IsSuccess.Should().BeFalse();
//            executeService.Verify(x => x.DoIt(), Times.Never);
//        }

//        [TestMethod]
//        public async Task TestSubscribe_InvokeCorrectly_WhenUseMultipleSuccessMiddlewareForTenantlessApdatedEvent()
//        {
//            string sequence = "";
//            _serviceCollection.UseTestServices(tenantlessBuildAction: x =>
//            {
//                x.Use<Middleware1>();
//                x.Use<Middleware2>();
//            });
//            Middleware1.OnBeforeInvoke += Middleware1_OnBeforeInvoke;
//            Middleware2.OnBeforeInvoke += Middleware2_OnBeforeInvoke;
//            Middleware1.OnAfterInvoke += Middleware1_OnAfterInvoke;
//            Middleware2.OnAfterInvoke += Middleware2_OnAfterInvoke;
//            var executeService = new Mock<IExecuteService>();
//            executeService.Setup(x => x.DoIt()).Returns(() =>
//            {
//                sequence += "_";
//                return Task.CompletedTask;
//            });
//            var provider = GetServiceProvider((typeof(IExecuteService), executeService.Object));

//            var payload = MicroservicePayload.Create(OperationInfoFactory.CreateTenancy(Guid.NewGuid()), new TenancyEventWithIExecuteService { });
//            var subscriber = GetService<ITenantlessAdaptedEventSubscriber>();
//            var result = await subscriber.SubscribeAsync(payload);
//            result.IsSuccess.Should().BeTrue();
//            executeService.Verify(x => x.DoIt(), Times.Once);
//            sequence.Should().Be("12_34");

//            Middleware1.OnBeforeInvoke -= Middleware1_OnBeforeInvoke;
//            Middleware2.OnBeforeInvoke -= Middleware2_OnBeforeInvoke;
//            Middleware1.OnAfterInvoke -= Middleware1_OnAfterInvoke;
//            Middleware2.OnAfterInvoke -= Middleware2_OnAfterInvoke;

//            void Middleware1_OnBeforeInvoke()
//            {
//                sequence += "1";
//            }

//            void Middleware2_OnBeforeInvoke()
//            {
//                sequence += "2";
//            }

//            void Middleware2_OnAfterInvoke()
//            {
//                sequence += "3";
//            }

//            void Middleware1_OnAfterInvoke()
//            {
//                sequence += "4";
//            }
//        }

//        [TestMethod]
//        public async Task TestSubscribe_InvokeFailed_WhenUseMultipleMiddlewareWith1FailedForTenantlessApdatedEvent()
//        {
//            string sequence = "";
//            _serviceCollection.UseTestServices(tenantlessBuildAction: x =>
//            {
//                x.Use<Middleware1>();
//                x.Use<Middleware2>();
//                x.Use<FailedInvocationMiddleware>();
//            });
//            Middleware1.OnBeforeInvoke += Middleware1_OnBeforeInvoke;
//            Middleware2.OnBeforeInvoke += Middleware2_OnBeforeInvoke;
//            Middleware1.OnAfterInvoke += Middleware1_OnAfterInvoke;
//            Middleware2.OnAfterInvoke += Middleware2_OnAfterInvoke;
//            var executeService = new Mock<IExecuteService>();
//            executeService.Setup(x => x.DoIt()).Returns(() =>
//            {
//                sequence += "_";
//                return Task.CompletedTask;
//            });
//            var provider = GetServiceProvider((typeof(IExecuteService), executeService.Object));

//            var payload = MicroservicePayload.Create(OperationInfoFactory.CreateTenancy(Guid.NewGuid()), new TenancyEventWithIExecuteService { });
//            var subscriber = GetService<ITenantlessAdaptedEventSubscriber>();
//            var result = await subscriber.SubscribeAsync(payload);
//            result.IsSuccess.Should().BeFalse();
//            executeService.Verify(x => x.DoIt(), Times.Never);
//            sequence.Should().Be("1234");

//            Middleware1.OnBeforeInvoke -= Middleware1_OnBeforeInvoke;
//            Middleware2.OnBeforeInvoke -= Middleware2_OnBeforeInvoke;
//            Middleware1.OnAfterInvoke -= Middleware1_OnAfterInvoke;
//            Middleware2.OnAfterInvoke -= Middleware2_OnAfterInvoke;

//            void Middleware1_OnBeforeInvoke()
//            {
//                sequence += "1";
//            }

//            void Middleware2_OnBeforeInvoke()
//            {
//                sequence += "2";
//            }

//            void Middleware2_OnAfterInvoke()
//            {
//                sequence += "3";
//            }

//            void Middleware1_OnAfterInvoke()
//            {
//                sequence += "4";
//            }
//        }

//        [TestMethod]
//        public async Task TestSubscribe_InvokeFailedAtBegining_WhenUseMultipleMiddlewareWithFailedAtBeginingForTenantlessApdatedEvent()
//        {
//            string sequence = "";
//            _serviceCollection.UseTestServices(tenantlessBuildAction: x =>
//            {
//                x.Use<FailedInvocationMiddleware>();
//                x.Use<Middleware1>();
//                x.Use<Middleware2>();
//            });
//            Middleware1.OnBeforeInvoke += Middleware1_OnBeforeInvoke;
//            Middleware2.OnBeforeInvoke += Middleware2_OnBeforeInvoke;
//            Middleware1.OnAfterInvoke += Middleware1_OnAfterInvoke;
//            Middleware2.OnAfterInvoke += Middleware2_OnAfterInvoke;
//            var executeService = new Mock<IExecuteService>();
//            executeService.Setup(x => x.DoIt()).Returns(() =>
//            {
//                sequence += "_";
//                return Task.CompletedTask;
//            });
//            var provider = GetServiceProvider((typeof(IExecuteService), executeService.Object));

//            var payload = MicroservicePayload.Create(OperationInfoFactory.CreateTenancy(Guid.NewGuid()), new TenancyEventWithIExecuteService { });
//            var subscriber = GetService<ITenantlessAdaptedEventSubscriber>();
//            var result = await subscriber.SubscribeAsync(payload);
//            result.IsSuccess.Should().BeFalse();
//            executeService.Verify(x => x.DoIt(), Times.Never);
//            sequence.Should().Be("");

//            Middleware1.OnBeforeInvoke -= Middleware1_OnBeforeInvoke;
//            Middleware2.OnBeforeInvoke -= Middleware2_OnBeforeInvoke;
//            Middleware1.OnAfterInvoke -= Middleware1_OnAfterInvoke;
//            Middleware2.OnAfterInvoke -= Middleware2_OnAfterInvoke;

//            void Middleware1_OnBeforeInvoke()
//            {
//                sequence += "1";
//            }

//            void Middleware2_OnBeforeInvoke()
//            {
//                sequence += "2";
//            }

//            void Middleware2_OnAfterInvoke()
//            {
//                sequence += "3";
//            }

//            void Middleware1_OnAfterInvoke()
//            {
//                sequence += "4";
//            }
//        }
//        #endregion
//    }
//}

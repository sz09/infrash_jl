namespace Tests.Unit.JobLogic.Infrastructure.Microservice.Server
{
    public class Service<T>
    {
        public T OperationInfo { get; set; }
        public ScopedInnerMostService<T> ScopedInnerMostService { get; set; }
        public InnerService1<T> InnerService1 { get; set; }
        public InnerService2<T> InnerService2 { get; set; }
        public Service(ScopedInnerMostService<T> scopedInnerMostService, T operationInfo, InnerService1<T> innerService1, InnerService2<T> innerService2)
        {
            InnerService1 = innerService1;
            InnerService2 = innerService2;
            ScopedInnerMostService = scopedInnerMostService;
            OperationInfo = operationInfo;
        }
    }

    public class ScopedInnerMostService<T>
    {
        public ScopedInnerMostService()
        {
        }
    }

    public class InnerService2<T>
    {
        public T OperationInfo { get; set; }
        public ScopedInnerMostService<T> ScopedInnerMostService { get; set; }
        public InnerInnerService1<T> InnerInnerService1 { get; set; }
        public InnerInnerService2<T> InnerInnerService2 { get; set; }
        public InnerService2(ScopedInnerMostService<T> scopedInnerMostService, T operationInfo, InnerInnerService1<T> innerInnerService1, InnerInnerService2<T> innerInnerService2)
        {
            InnerInnerService1 = innerInnerService1;
            InnerInnerService2 = innerInnerService2;
            ScopedInnerMostService = scopedInnerMostService;
            OperationInfo = operationInfo;
        }
    }

    public class InnerService1<T>
    {
        public T OperationInfo { get; set; }
        public ScopedInnerMostService<T> ScopedInnerMostService { get; set; }
        public InnerInnerService1<T> InnerInnerService1 { get; set; }
        public InnerInnerService2<T> InnerInnerService2 { get; set; }
        public InnerService1(ScopedInnerMostService<T> scopedInnerMostService, T operationInfo, InnerInnerService1<T> innerInnerService1, InnerInnerService2<T> innerInnerService2)
        {
            InnerInnerService1 = innerInnerService1;
            InnerInnerService2 = innerInnerService2;
            ScopedInnerMostService = scopedInnerMostService;
            OperationInfo = operationInfo;
        }
    }

    public class InnerInnerService2<T>
    {
        public T OperationInfo { get; set; }
        public ScopedInnerMostService<T> ScopedInnerMostService { get; set; }
        public InnerInnerService2(ScopedInnerMostService<T> scopedInnerMostService, T operationInfo)
        {
            OperationInfo = operationInfo;
            ScopedInnerMostService = scopedInnerMostService;
        }
    }

    public class InnerInnerService1<T>
    {
        public T OperationInfo { get; set; }
        public ScopedInnerMostService<T> ScopedInnerMostService { get; set; }
        public InnerInnerService1(ScopedInnerMostService<T> scopedInnerMostService, T operationInfo)
        {
            OperationInfo = operationInfo;
            ScopedInnerMostService = scopedInnerMostService;
        }
    }
}

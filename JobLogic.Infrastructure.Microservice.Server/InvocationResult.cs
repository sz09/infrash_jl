using System;

namespace JobLogic.Infrastructure.Microservice.Server
{

    public sealed class InvocationResult
    {
        public enum ResultState
        {
            SuccessWithRespone,
            Success,
            Cacncelled
        }
        private InvocationResult()
        {
        }

        public ResultState State { get; private set; }
        public object Response { get; private set; }
        public string CancelMessage { get; private set; }

        public bool IsSuccess => State == ResultState.Success || State == ResultState.SuccessWithRespone;

        public object GetResponseOnlyWhenStateIsSuccessWithRespone()
        {
            if(State == ResultState.SuccessWithRespone)
            {
                return Response;
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        public static InvocationResult CreateSuccess(object returnValue)
        {
            return new InvocationResult()
            {
                State = ResultState.SuccessWithRespone,
                Response = returnValue
            };
        }

        public static InvocationResult CreateSuccess()
        {
            return new InvocationResult()
            {
                State = ResultState.Success
            };
        }

        public static InvocationResult CreateCancelled(string cancelMessage)
        {
            return new InvocationResult()
            {
                State = ResultState.Cacncelled,
                CancelMessage = cancelMessage
            };
        }
    }
}

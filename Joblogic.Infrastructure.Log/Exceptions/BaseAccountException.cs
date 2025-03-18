namespace JobLogic.Infrastructure.Log
{
    public abstract class BaseAccountException : BaseException
    {
        public BaseAccountException(string message)
            : base(message)
        {
        }
    }
}

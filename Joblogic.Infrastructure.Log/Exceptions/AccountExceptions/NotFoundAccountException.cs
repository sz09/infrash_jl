namespace JobLogic.Infrastructure.Log
{
    public class NotFoundAccountException : BaseAccountException
    {
        public NotFoundAccountException()
            : base("The email address or password provided is incorrect.")
        {
        }
    }
}

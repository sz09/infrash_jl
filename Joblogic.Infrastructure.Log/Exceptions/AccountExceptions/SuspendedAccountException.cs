namespace JobLogic.Infrastructure.Log
{
    public class SuspendedAccountException : BaseAccountException
    {
        public SuspendedAccountException()
            : base("Your account has been suspended, please contact your administrator.")
        {
        }
    }
}

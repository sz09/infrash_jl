namespace JobLogic.Infrastructure.Log
{
    public class LockedAccountException : BaseAccountException
    {
        public LockedAccountException()
            : base("Your account has been locked, please try again later.")
        {
        }
    }
}

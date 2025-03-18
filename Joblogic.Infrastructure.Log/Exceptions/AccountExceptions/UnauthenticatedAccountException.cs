namespace JobLogic.Infrastructure.Log
{
    public class UnauthenticatedAccountException : BaseAccountException
    {
        public UnauthenticatedAccountException()
            : base("Unable to authenticate the user from server.")
        {
        }
    }
}

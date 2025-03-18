namespace JobLogic.Infrastructure.EmailClient
{
    public sealed class Error
    {
        public Error(string message)
        {
            Message = message;
        }

        public string Message { get; internal set; }
    }
}

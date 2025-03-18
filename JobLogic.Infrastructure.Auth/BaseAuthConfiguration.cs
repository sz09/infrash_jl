namespace JobLogic.Infrastructure.Auth
{
    public abstract class BaseAuthConfiguration
    {
        public abstract string ConsumerKey { get; set; }

        public abstract string ConsumerSecret { get; set; }

        public abstract string UserId { get; set; }
    }
}

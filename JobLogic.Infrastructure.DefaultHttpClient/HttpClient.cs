namespace JobLogic.Infrastructure.DefaultHttpClient
{
    class InternalHttpClient : System.Net.Http.HttpClient, IHttpClient
    {
        public InternalHttpClient()
        {

        }
        public InternalHttpClient(System.Net.Http.HttpMessageHandler handler) : base(handler)
        {

        }
        public InternalHttpClient(System.Net.Http.HttpMessageHandler handler, bool disposeHandler): base (handler , disposeHandler)
        {

        }
    }
}

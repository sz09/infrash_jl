namespace JobLogic.Infrastructure.Microservice.Client
{
    public struct RequestAddress
    {
        public RequestAddress(string origin, string path)
        {
            Origin = origin.TrimEnd('/');
            Path = path.TrimStart('/');
            FullUrl = Origin + "/" + Path;
        }

        public string Origin { get; }
        public string Path { get; }
        public string FullUrl { get; }
    }
}

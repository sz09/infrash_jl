using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OData.UriParser;
using System;
using System.Linq;

namespace JobLogic.Infrastructure.OData.Server
{
    public interface IODataResolver
    {
        ODataQueryOptions ResolveODataQueryOptions(Type edmType, string pathAndQueryString);
    }
    class ODataResolver : IODataResolver
    {
        readonly static ServiceProvider _serviceProvider;
        static ODataResolver()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddOData();
            _serviceProvider = serviceCollection.BuildServiceProvider();

            var routeBuilder = new RouteBuilder(new ApplicationBuilder(_serviceProvider));
            routeBuilder
                .Count()
                .Expand()
                .Filter()
                .MaxTop(null)
            .OrderBy()
            .Select()
            .SkipToken();
            routeBuilder.EnableDependencyInjection();
        }

        IEdmModelProvider _edmModelProvider;
        public ODataResolver(IEdmModelProvider edmModelProvider)
        {
            _edmModelProvider = edmModelProvider;
        }
        public ODataQueryOptions ResolveODataQueryOptions(Type edmType, string pathAndQueryString)
        {
            var context = new ODataQueryContext(_edmModelProvider.EdmModel, edmType, new Microsoft.AspNet.OData.Routing.ODataPath());

            var ctx = new DefaultHttpContext()
            {
                RequestServices = _serviceProvider
            };
            var odataRequest = ctx.Request;
            string acceptedPathFormat = $"/{edmType.Name}?";
            if (!pathAndQueryString.StartsWith(acceptedPathFormat))
            {
                throw new Exception($"path and query string must be in format '{acceptedPathFormat}'");
            }
            odataRequest.Method = HttpMethods.Get;
            odataRequest.Host = new HostString("fakejodatahost");
            odataRequest.Scheme = "http";
            odataRequest.QueryString = new QueryString(pathAndQueryString.Replace(acceptedPathFormat,"?"));

            var option = new ODataQueryOptions(context, odataRequest);

            return option;
        }
    }
}

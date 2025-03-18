using System;
using System.Collections.Generic;

namespace JobLogic.Infrastructure.Microservice.Client
{
    public interface IRequestAddressResolver
    {
        RequestAddress Resolve(BaseMicroserviceMsg microserviceMsg);
    }
    public class RequestAddressResolver : IRequestAddressResolver
    {
        private readonly string _defaultRequestOrigin;
        private readonly string _path;
        private readonly IReadOnlyDictionary<string, MsgCustomRoute> _customRoute;
        private readonly bool _skipAppendMsgSignatureHintPath;
        private readonly static string PREFERRED_ROUTE = Environment.GetEnvironmentVariable(EnvironmentVariableName.JL_PREFERRED_ROUTE);
        public RequestAddressResolver(string defaultRequestOrigin, string path, IReadOnlyDictionary<string, MsgCustomRoute> customRoute, bool skipAppendMsgSignatureHintPath = false)
        {
            _defaultRequestOrigin = defaultRequestOrigin;
            _path = path;
            _customRoute = customRoute;
            _skipAppendMsgSignatureHintPath = skipAppendMsgSignatureHintPath;
        }
        public RequestAddress Resolve(BaseMicroserviceMsg microserviceMsg)
        {
            var routeName = microserviceMsg.GetCustomRouteName();
            var route = _customRoute.GetCustomRouteOrNULL(routeName);
            var preferredRoute = _customRoute.GetCustomRouteOrNULL(PREFERRED_ROUTE);
            var path = _path;
            if (!_skipAppendMsgSignatureHintPath)
            {
                path = _path.TrimEnd('/') + "/" + microserviceMsg.GetSignatureHint();
            }
            var requestOrigin = route?.RequestOrigin ?? preferredRoute?.RequestOrigin ?? _defaultRequestOrigin; //In case custom route doesn't support, just fallback to default
            return new RequestAddress(requestOrigin, path);
        }
    }


}

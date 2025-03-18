using System.Collections.Generic;

namespace JobLogic.Infrastructure.Microservice.Client
{
    public sealed class MsgCustomRoute
    {
        public string RequestOrigin { get; set; }
        public string CommandQueueNameSuffix { get; set; }
    }
}

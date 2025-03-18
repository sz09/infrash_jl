using System;
using System.Linq;

namespace JobLogic.Infrastructure.Microservice.Client
{
    public static class BaseMicroserviceDataExtensions
    {
        public static string GetSignature(this BaseMicroserviceData data)
        {
            return BaseMicroserviceData.GetSignature(data.GetType());
        }
        
        public static string GetSignatureHint(this BaseMicroserviceData data)
        {
            return BaseMicroserviceData.GetSignatureHint(data.GetType());
        }

        

        public static string GetMsgTravelLog(this BaseMicroserviceData data)
        {
            var msgType = data.GetType();
            var msgSignature = BaseMicroserviceData.GetSignature(msgType);
            var msgLogId = msgSignature.GetHashCode() + BaseMicroserviceData.GetSignatureHint(msgType);
            return msgLogId;
        }
    }
}

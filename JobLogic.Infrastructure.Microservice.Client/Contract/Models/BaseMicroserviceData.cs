using System;

namespace JobLogic.Infrastructure.Microservice.Client
{
    public abstract class BaseMicroserviceData
    {
        public static string GetSignature(Type type)
        {
            return type.FullName;
        }
        public static string GetSignatureHint(Type type)
        {
            return type.Name;
        }
        protected internal BaseMicroserviceData()
        {

        }
    }
}

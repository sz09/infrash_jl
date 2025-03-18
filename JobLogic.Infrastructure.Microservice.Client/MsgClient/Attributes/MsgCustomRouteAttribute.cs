using System;

namespace JobLogic.Infrastructure.Microservice.Client
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class MsgCustomRouteAttribute : Attribute
    {
        public MsgCustomRouteAttribute(string customRouteName)
        {
            CustomRouteName = customRouteName;
        }

        public string CustomRouteName { get; }
    }

    
}

using System;
using System.Collections.Generic;
using System.Linq;

namespace JobLogic.Infrastructure.Microservice.Server
{
    public class MiddlewareBuilder
    {
        List<Type> middlewares = new List<Type>();
        public void Use<T>() where T: HandlerMiddleware
        {
            middlewares.Add(typeof(T));
        }
        internal HandlerMiddleware Build(HandlerMiddleware mainMiddleware)
        {
            object mw = mainMiddleware;
            if (middlewares.Any())
            {
                var mwList = new List<Type>(middlewares);
                mwList.Reverse();

                foreach (var i in mwList)
                {
                    mw = Activator.CreateInstance(i, mw);
                }
            }
            return mw as HandlerMiddleware;
        }
    }
}

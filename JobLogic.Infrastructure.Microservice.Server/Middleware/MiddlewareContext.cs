using System;
using System.Collections.Generic;

namespace JobLogic.Infrastructure.Microservice.Server
{
    public interface IMiddlewareContextDataReader
    {
        object GetData(string key);
    }
    public class MiddlewareContext : IMiddlewareContextDataReader
    {
        readonly IDictionary<string, object> _data = new Dictionary<string, object>();

        internal MiddlewareContext(IServiceProvider serviceProvider, HandlerInfo handlerInfo, object message, string messageSignature)
        {
            ServiceProvider = serviceProvider;
            HandlerInfo = handlerInfo;
            Message = message;
            MessageSignature = messageSignature;
        }

        
        internal HandlerInfo HandlerInfo { get; }
        internal object Message { get; }
        public string MessageSignature { get; }

        public IServiceProvider ServiceProvider { get; }
        public object GetData(string key) => _data[key];
        public void SetData(string key, object value)
        {
            _data[key] = value;
        }
    }
}

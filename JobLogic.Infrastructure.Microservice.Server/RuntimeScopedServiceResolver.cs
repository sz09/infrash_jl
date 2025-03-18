namespace JobLogic.Infrastructure.Microservice.Server
{
    class RuntimeScopedServiceResolver<T> where T : class
    {
        private T _value = null;
        public T Value
        {
            get
            {
                if (_value == null)
                    throw new MicroserviceServerException($"RuntimeScopedServiceResolver {typeof(T).FullName}'s value is not found");
                return _value;
            }
            set
            {
                _value = value;
            }
        }
    }
}

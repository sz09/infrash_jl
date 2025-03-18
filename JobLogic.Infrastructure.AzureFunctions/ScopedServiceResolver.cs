using System;

namespace JobLogic.Infrastructure.AzureFunctions
{
    public class ScopedServiceResolver<T> where T : class
    {
        private T _value = null;
        public T Value
        {
            get
            {
                if (_value == null)
                    throw new Exception($"ScopedServiceResolver {typeof(T).FullName}'s value is not found");
                return _value;
            }
            set
            {
                _value = value;
            }
        }
    }
}

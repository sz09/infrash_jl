using System;

namespace JobLogic.Infrastructure.Utilities
{
    public static class TypeUtils
    {
        /// <summary>
        /// Generic method which can return the default value for both value and reference types
        /// </summary>
        public static T GetDefaultValueForType<T>()
        {
            if (typeof(T).IsValueType)
                return default(T);

            return (T)Activator.CreateInstance(typeof(T));
        }
    }
}
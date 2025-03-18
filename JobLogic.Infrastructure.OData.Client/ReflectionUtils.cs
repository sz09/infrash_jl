using System;
using System.Linq;
using System.Reflection;

namespace JobLogic.Infrastructure.OData.Client
{
    static class ReflectionUtils
    {
        static Lazy<MethodInfo> _lzMethodInfoForQueryableTakeIntCount = new Lazy<MethodInfo>(() =>
        {
            var allTakeMethodInfo = typeof(Queryable)
                                .GetMethods(BindingFlags.Public | BindingFlags.Static).Where(x => x.Name == nameof(Queryable.Take)).ToList();
            var takeMethod = allTakeMethodInfo.Where(x => x.GetParameters().Length == 2 && x.GetParameters().ElementAt(1).ParameterType == typeof(int)).Single();
            return takeMethod;
        });

        public static MethodInfo MethodInfoForQueryableTakeIntCount(Type elementType)
        {
            
            return _lzMethodInfoForQueryableTakeIntCount.Value.MakeGenericMethod(elementType);
        }
    }
}

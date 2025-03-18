using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace JobLogic.Infrastructure.Contract.Extensions
{
    public class ChangeRecord<T>
    {
        [JsonConstructor]
        private ChangeRecord()
        {
                
        }
        public ChangeRecord(T originValue, T currentValue)
        {
            OriginValue = originValue;
            CurrentValue = currentValue;
        }
        [JsonProperty]
        public T OriginValue { get; private set; }
        [JsonProperty]
        public T CurrentValue { get; private set; }
    }

    public static class ChangeRecordExtesions
    {
        public static bool IsModified<T>(this ChangeRecord<T> valueChanged)
        {
            if (valueChanged == null) return false;
            return !EqualityComparer<T>.Default.Equals(valueChanged.OriginValue, valueChanged.CurrentValue); ;
        }
    }

}

using JobLogic.Infrastructure.Contract.Extensions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace JobLogic.Infrastructure.ChangeRecorder
{
    public class RecordResult<T>
    {
        readonly Dictionary<string, RecordItem> _changeDict = new Dictionary<string, RecordItem>();

        internal RecordResult(Dictionary<string, RecordItem> changeDict)
        {
            _changeDict = changeDict;
        }

        public ChangeRecord<P> PickChangeRecord<P>(Expression<Func<T, ChangeRecord<P>>> expression)
        {
            var nameKey = expression.GetChangeRecordDictKey();
            if (_changeDict.ContainsKey(nameKey))
            {
                var rec = (RecordItem<P>)_changeDict[nameKey];
                return new ChangeRecord<P>(rec.OriginValue, rec.CurrentValue);
            }
            else
            {
                return null;
            }
        }
    }
}

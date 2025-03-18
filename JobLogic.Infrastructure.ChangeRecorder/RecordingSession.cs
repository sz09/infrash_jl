using JobLogic.Infrastructure.Contract.Extensions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace JobLogic.Infrastructure.ChangeRecorder
{
    public class RecordingSession<T>
    {
        Dictionary<string, RecordItem> _changeDict = new Dictionary<string, RecordItem>();
        public void Record<P>(Expression<Func<T, ChangeRecord<P>>> expression, P val)
        {
            var nameKey = expression.GetChangeRecordDictKey();
            if (_changeDict.ContainsKey(nameKey))
            {
                var rec = (RecordItem<P>)_changeDict[nameKey];
                rec.CurrentValue = val;
            }
            else
            {
                _changeDict[nameKey] = new RecordItem<P>(val);
            }
        }

        public RecordResult<T> FinalizeAndStartNewSession()
        {
            var newChangeBook = new Dictionary<string, RecordItem>();
            foreach (var key in _changeDict.Keys)
            {
                newChangeBook[key] = _changeDict[key].GenerateNewOriginWithCurrentValue();
            }
            var rs = new RecordResult<T>(_changeDict);
            _changeDict = newChangeBook;
            return rs;
        }

    }
}

using System;
using System.Collections.Generic;

namespace JobLogic.Infrastructure.OData.Client
{
    class SingleItemResolver<D>
    {
        public SingleItemResolver(Func<IEnumerable<D>, D> singleItemConverter, int takeTop)
        {
            SingleItemConverter = singleItemConverter;
            TakeTop = takeTop;
        }
        public Func<IEnumerable<D>, D> SingleItemConverter { get; }
        public int TakeTop { get; }
    }
}

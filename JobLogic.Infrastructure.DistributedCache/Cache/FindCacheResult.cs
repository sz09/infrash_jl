using System;

namespace JobLogic.Infrastructure.DistributedCache
{
    public class FindCacheResult<T>
    {
        
        public FindCacheResultState State { get; private set; }
        private T Value { get; set; }
        public T GetValueOnlyWhenStateIsFound()
        {
            if (State == FindCacheResultState.Found)
            {
                return Value;
            }
            throw new InvalidOperationException();
        }

        public static FindCacheResult<T> CreateNotFoundResult()
        {
            return new FindCacheResult<T>()
            {
                State = FindCacheResultState.NotFound,
            };
        }
        public static FindCacheResult<T> CreateFoundResult(T value)
        {
            return new FindCacheResult<T>()
            {
                State = FindCacheResultState.Found,
                Value = value
            };
        }
    }
}

namespace JobLogic.Infrastructure.ChangeRecorder
{
    abstract class RecordItem
    {
        internal abstract RecordItem GenerateNewOriginWithCurrentValue();
    }
    class RecordItem<T> : RecordItem
    {
        public RecordItem(T originValue)
        {
            OriginValue = originValue;
            CurrentValue = originValue;
        }
        public T OriginValue { get; private set; }
        public T CurrentValue { get; set; }

        internal override RecordItem GenerateNewOriginWithCurrentValue()
        {
            return new RecordItem<T>(CurrentValue);
        }
    }
}

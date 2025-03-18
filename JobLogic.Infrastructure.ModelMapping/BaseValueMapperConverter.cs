namespace JobLogic.Infrastructure.ModelMapping
{
    public abstract class BaseValueMapperConverter<TSourceDataType, TDestinationDataType>
    {
        public abstract TDestinationDataType ConvertFrom(TSourceDataType source);

        public abstract TSourceDataType ConvertFrom(TDestinationDataType source);
    }
}

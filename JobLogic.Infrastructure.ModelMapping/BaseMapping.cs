namespace JobLogic.Infrastructure.ModelMapping
{
    public interface IBaseMapping
    {
        TDest Map<TDest, TSource>(TSource source);

        TDest Map<TDest, TSource>(TSource source, TDest destination);
    }

    public abstract class BaseMapping : IBaseMapping
    {
        public abstract TDest Map<TDest, TSource>(TSource source);

        public abstract TDest Map<TDest, TSource>(TSource source, TDest destination);
    }
}

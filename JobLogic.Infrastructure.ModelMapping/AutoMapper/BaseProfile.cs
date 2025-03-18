using AutoMapper;

namespace JobLogic.Infrastructure.ModelMapping
{
    public abstract class BaseProfile : Profile
    {
        public BaseProfile()
        {
            Configure();
        }

        protected abstract void Configure();

    }
}

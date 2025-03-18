using Microsoft.OData.Edm;

namespace JobLogic.Infrastructure.OData.Server
{
    public interface IEdmModelProvider
    {
        IEdmModel EdmModel { get; }
    }
    class EdmModelProvider : IEdmModelProvider
    {
        private readonly IEdmModel _edmModel;

        public EdmModelProvider(IEdmModel edmModel)
        {
            _edmModel = edmModel;
        }

        IEdmModel IEdmModelProvider.EdmModel => _edmModel;

    }
}

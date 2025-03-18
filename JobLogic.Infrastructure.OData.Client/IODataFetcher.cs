using System.Threading.Tasks;

namespace JobLogic.Infrastructure.OData.Client
{
    public interface IODataFetcher
    {
        Task<T[]> FetchData<T>(string queryString);
        Task<long> FetchCount<T>(string queryString);
    }

    public interface IODataFetcher<Tp>
    {
        Task<T[]> FetchData<T>(string queryString, Tp buildParameter);
        Task<long> FetchCount<T>(string queryString, Tp buildParameter);
    }
}

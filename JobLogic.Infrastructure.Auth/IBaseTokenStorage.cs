namespace JobLogic.Infrastructure.Auth
{
    public interface IBaseTokenStorage
    {
        void Add(string companyId, BaseAuthToken authToken);

        void Remove(string companyId);

        BaseAuthToken Find(string companyId);
    }
}

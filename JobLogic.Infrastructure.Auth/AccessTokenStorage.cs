namespace JobLogic.Infrastructure.Auth
{
    public interface IAccessTokenStorage: IBaseTokenStorage
    {
    }

    public class AccessTokenStorage: BaseTokenStorage, IAccessTokenStorage
    {
    }
}

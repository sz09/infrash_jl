namespace JobLogic.Infrastructure.Auth
{
    public interface IRequestTokenStorage: IBaseTokenStorage
    {
    }

    public class RequestTokenStorage: BaseTokenStorage, IRequestTokenStorage
    {
    }
}

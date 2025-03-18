namespace JobLogic.Infrastructure.ConfigurationHelper
{
    public interface IConfigurationHelper
    {
        string GetAppSetting(string key);
        string GetConnectionString(string key);
    }
}

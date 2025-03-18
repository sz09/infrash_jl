namespace JobLogic.Infrastructure.Slack
{
    public sealed class SlackServiceClientSetting
    {
        public SlackServiceClientSetting(string webhookUrl)
        {
            WebhookUrl = webhookUrl;
        }

        public string WebhookUrl { get; set; }
    }
}

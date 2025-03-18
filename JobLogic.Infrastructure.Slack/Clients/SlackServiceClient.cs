using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace JobLogic.Infrastructure.Slack
{
    public interface ISlackServiceClient
    {
        Task<HttpResponseMessage> SendMessage(params string[] messages);
        Task<HttpResponseMessage> SendMessage(SlackMessageModel message);
    }

    public class SlackServiceClient : ISlackServiceClient
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private readonly string _webhookUrl;
        public SlackServiceClient(SlackServiceClientSetting slackServiceClientSetting)
        {
            _webhookUrl = slackServiceClientSetting.WebhookUrl;
        }

        public async Task<HttpResponseMessage> SendMessage(params string[] messages)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var message in messages)
            {
                stringBuilder.AppendLine(message);

                if (stringBuilder.Length >= 2000)
                {
                    var cutSlackMessage = new SlackMessageModel
                    {
                        blocks = new List<SlackMessageModel.Block> { new SlackMessageModel.Block
                            {
                                text = new SlackMessageModel.Text(stringBuilder.ToString())
                            }
                        }
                    };
                    await _httpClient.PostAsJsonAsync(_webhookUrl, cutSlackMessage);
                    stringBuilder = new StringBuilder();
                }
            }
            var slackMessage = new SlackMessageModel
            {
                blocks = new List<SlackMessageModel.Block> { new SlackMessageModel.Block
                {
                    text = new SlackMessageModel.Text(stringBuilder.ToString())
                } }
            };
            return await _httpClient.PostAsJsonAsync(_webhookUrl, slackMessage);
        }

        public Task<HttpResponseMessage> SendMessage(SlackMessageModel message)
        {
            return _httpClient.PostAsJsonAsync(_webhookUrl, message);
        }
    }
}

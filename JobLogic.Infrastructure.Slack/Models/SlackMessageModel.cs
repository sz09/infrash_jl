using System.Collections.Generic;

namespace JobLogic.Infrastructure.Slack
{
    public class SlackMessageModel
    {
        public List<Block> blocks { get; set; }
        public class Block
        {
            public string type { get => "section"; }
            public Text text { get; set; }
        }

        public class Text
        {
            public string type { get => "mrkdwn"; }
            public string text { get; set; }
            public Text()
            {

            }

            public Text(string text)
            {
                this.text = text;
            }
        }
    }
}

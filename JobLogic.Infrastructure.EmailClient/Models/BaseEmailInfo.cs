using System.Collections.Generic;

namespace JobLogic.Infrastructure.EmailClient
{
    public abstract class BaseEmailInfo
    {
        public string EmailSender { get; set; }
        public string EmailTo { get; set; }
        public string Subject { get; set; }
        public string ReplyToAddress { get; set; }
        public string Cc { get; set; }
        public string Bcc { get; set; }
        public IEnumerable<EmailAttachment> Attachments { get; set; }
        public Dictionary<string, string> CustomArgs { get; set; }
    }
}

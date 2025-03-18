using System;
using System.Collections.Generic;

namespace JobLogic.Infrastructure.EmailSendingService
{
    public class EmailMessageModel
    {
        public string From { get; set; }

        public string To { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public string ReplyTo { get; set; }

        public string Cc { get; set; }

        public string Bcc { get; set; }

        public Dictionary<string, string> Images { get; set; }

        public EmailAttachment[] Attachments { get; set; }

        public string Template { get; set; }

        public Dictionary<string, object> Data { get; set; }

        public Guid? CommunicationLogId { get; set; }

        public Guid? TenantId { get; set; }

        public EmailMessageModel() { }

        public EmailMessageModel(string from, string to, string subject, string body, string replyTo = null, string cc = null, string bcc = null,
            Dictionary<string, string> images = null, EmailAttachment[] attachments = null, string templateId = null)
        {
            From = from;
            To = to;
            Subject = subject;
            Body = body;
            ReplyTo = replyTo;
            Cc = cc;
            Bcc = bcc;
            Images = images;
            Attachments = attachments;
        }

        public EmailMessageModel(string from, string to, string subject, string templateId, Dictionary<string, object> data, string replyTo = null, string cc = null, string bcc = null,
            Dictionary<string, string> images = null, EmailAttachment[] attachments = null)
        {
            From = from;
            To = to;
            Subject = subject;
            Template = templateId;
            Data = data;
            ReplyTo = replyTo;
            Cc = cc;
            Bcc = bcc;
            Images = images;
            Attachments = attachments;
        }
    }
}

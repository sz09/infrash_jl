using JobLogic.Infrastructure.Utilities;
using Newtonsoft.Json.Linq;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace JobLogic.Infrastructure.EmailClient
{
    public interface IEmailClient
    {
        Task<Error> SendMessageAsync(HtmlEmail emailMsg);
        Task<Error> SendMessageAsync(TemplatedEmail emailMsg);
    }
    public class EmailClient : IEmailClient
    {
        private readonly ISendGridClient _sendGridClient;

        public EmailClient(ISendGridClient sendGridClient)
        {
            _sendGridClient = sendGridClient;
        }
        public async Task<Error> SendMessageAsync(HtmlEmail emailMsg)
        {
            var msg = ConvertToSendGridMessage(emailMsg);
            return await SendMessge(msg);
        }

        public async Task<Error> SendMessageAsync(TemplatedEmail emailMsg)
        {
            var msg = ConvertToSendGridMessage(emailMsg);
            return await SendMessge(msg);
        }

        Attachment ConvertToAttachment(EmailAttachment emailAttachment)
        {
            if (emailAttachment.IsInlineAttachment)
            {
                return new Attachment
                {
                    Content = emailAttachment.Base64Content,
                    Disposition = "inline",
                    Filename = emailAttachment.FileName,
                    ContentId = emailAttachment.InlineContentId
                };
            }
            else
            {
                return new Attachment
                {
                    Content = emailAttachment.Base64Content,
                    Filename = emailAttachment.FileName,
                    Disposition = "attachment"
                };
            }
        }

        private void CreateEmailAddressesForSendGrid(SendGridMessage message, List<EmailAddress> emailsTo, string emailAdresses)
        {
            if (!string.IsNullOrWhiteSpace(emailAdresses))
            {
                emailAdresses = emailAdresses.Replace(",", ";");
                var emailList = emailAdresses.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var email in emailList)
                {
                    if (EmailNonNullAndUnique(message, emailsTo, email) && email.isEmail())
                    {
                        var unescapeUnicode = Regex.Unescape(email.Trim());
                        emailsTo.Add(new EmailAddress(unescapeUnicode));
                    }
                }
            }
        }

        private bool EmailNonNullAndUnique(SendGridMessage message, List<EmailAddress> list, string email)
        {
            if (!string.IsNullOrWhiteSpace(email))
            {
                email = email.Trim();
                if (!list.Any(e => e.Email.Equals(email, StringComparison.InvariantCultureIgnoreCase)))
                {
                    if (message.Personalizations != null)
                    {
                        var personalization = message.Personalizations.FirstOrDefault();

                        return
                            (personalization.Tos == null || !personalization.Tos.Any(e => e.Email.Equals(email, StringComparison.InvariantCultureIgnoreCase))) &&
                            (personalization.Ccs == null || !personalization.Ccs.Any(e => e.Email.Equals(email, StringComparison.InvariantCultureIgnoreCase))) &&
                            (personalization.Bccs == null || !personalization.Bccs.Any(e => e.Email.Equals(email, StringComparison.InvariantCultureIgnoreCase)));
                    }

                    return true;
                }
            }

            return false;
        }

        private bool IsValidPersonalizations(SendGridMessage mailMessage)
        {
            return mailMessage.Personalizations != null && mailMessage.Personalizations.All(p => p.Tos.Any());
        }

        private async Task<Error> SendMessge(SendGridMessage sendGridMessage)
        {
            if (!IsValidPersonalizations(sendGridMessage))
                return new Error($"Invalid email message");

            var response = await _sendGridClient.SendEmailAsync(sendGridMessage);
            if (response.StatusCode != HttpStatusCode.Accepted)
                return new Error($"{response.StatusCode} - SendGrid responsed errors: { await response.Body?.ReadAsStringAsync() }");

            return null;
        }

        SendGridMessage ConvertCommonEmailInfoToSendGridMessage(BaseEmailInfo message)
        {
            var personalization = new Personalization();
            var msg = new SendGridMessage
            {
                From = new EmailAddress(message.EmailSender),
                Subject = message.Subject
            };

            if (message.CustomArgs != null)
            {
                personalization.CustomArgs = message.CustomArgs;
            }
            msg.SetClickTracking(true, false);
            msg.SetOpenTracking(true);
            msg.SetSubscriptionTracking(true);


            #region [To, ReplyTo, CC, BCC]
            personalization.Tos = new List<EmailAddress>();
            var emailsTo = new List<EmailAddress>();
            CreateEmailAddressesForSendGrid(msg, emailsTo, message.EmailTo);
            msg.AddTos(emailsTo, personalization: personalization);

            if (!string.IsNullOrWhiteSpace(message.ReplyToAddress) && message.ReplyToAddress.isEmail())
                msg.SetReplyTo(new EmailAddress(message.ReplyToAddress));

            if (!string.IsNullOrWhiteSpace(message.Cc))
            {
                personalization.Ccs = new List<EmailAddress>();
                var cc = new List<EmailAddress>();
                CreateEmailAddressesForSendGrid(msg, cc, message.Cc);
                msg.AddCcs(cc, personalization: personalization);
            }

            if (!string.IsNullOrWhiteSpace(message.Bcc))
            {
                personalization.Bccs = new List<EmailAddress>();
                var bcc = new List<EmailAddress>();
                CreateEmailAddressesForSendGrid(msg, bcc, message.Bcc);
                msg.AddBccs(bcc, personalization: personalization);
            }
            #endregion

            IEnumerable<Attachment> messageAttachments = new List<Attachment>();
            if (message.Attachments != null)
            {
                messageAttachments = message.Attachments.Select(x => ConvertToAttachment(x));
            }

            if (messageAttachments.Any())
                msg.AddAttachments(messageAttachments);

            return msg;
        }

        SendGridMessage ConvertToSendGridMessage(HtmlEmail message)
        {
            var msg = ConvertCommonEmailInfoToSendGridMessage(message);
            msg.HtmlContent = message.Body;
            return msg;
        }
        SendGridMessage ConvertToSendGridMessage(TemplatedEmail message)
        {
            var msg = ConvertCommonEmailInfoToSendGridMessage(message);


            string templateId = message.TemplateId;

            if (!string.IsNullOrWhiteSpace(templateId))
            {
                msg.SetTemplateId(templateId);

                if (message.TemplateData != null)
                {
                    var templateData = new Dictionary<string, object>();
                    foreach (var obj in message.TemplateData)
                    {
                        // Fix JOB-14279 - JArray failed to converted into expected object (Newtonsoft 9 defect ???)
                        if (obj.Value is JArray)
                        {
                            var jarrayValue = obj.Value as JArray;
                            var dictValue = jarrayValue.ToObject<Dictionary<string, object>[]>();
                            templateData.Add(obj.Key, dictValue);
                        }
                        else
                        {
                            templateData.Add(obj.Key, obj.Value);
                        }
                    }
                    msg.SetTemplateData(templateData);
                }
            }

            return msg;
        }
    }
}

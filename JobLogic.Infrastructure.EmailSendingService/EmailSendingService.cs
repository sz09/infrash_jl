using JobLogic.Infrastructure.ImageHelper;
using JobLogic.Infrastructure.ServiceResponders;
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

namespace JobLogic.Infrastructure.EmailSendingService
{
    public interface IEmailSendingService
    {
        /// <summary>
        /// Sends non-template based email
        /// </summary>
        /// <param name="emailFrom"></param>
        /// <param name="emailTo"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="replyToAddress"></param>
        /// <param name="ccEmailAddress"></param>
        /// <param name="bccEmailAddress"></param>
        /// <param name="images"></param>
        /// <param name="attachments"></param>
        /// <returns></returns>
        Task<JobLogic.Infrastructure.ServiceResponders.Response> SendMessageAsync(
            string emailFrom, string emailTo, string subject, string body,
            string replyToAddress = null, string ccEmailAddress = null, string bccEmailAddress = null,
            Dictionary<string, string> images = null,
            Guid? tenantId = null, Guid? communicationLogId = null, params EmailAttachment[] attachments);

        /// <summary>
        /// Sends template-based email
        /// </summary>
        /// <param name="emailFrom"></param>
        /// <param name="emailTo"></param>
        /// <param name="subject"></param>
        /// <param name="templateId"></param>
        /// <param name="data"></param>
        /// <param name="replyToAddress"></param>
        /// <param name="ccEmailAddress"></param>
        /// <param name="bccEmailAddress"></param>
        /// <param name="images"></param>
        /// <param name="attachments"></param>
        /// <returns></returns>
        Task<JobLogic.Infrastructure.ServiceResponders.Response> SendMessageAsync(
            string emailFrom, string emailTo, string subject,
            string templateId, Dictionary<string, object> data,
            string replyToAddress = null, string ccEmailAddress = null, string bccEmailAddress = null,
            Dictionary<string, string> images = null,
            Guid? tenantId = null, Guid? communicationLogId = null, params EmailAttachment[] attachments);
    }

    public class EmailSendingService : IEmailSendingService
    {
        private readonly ISendGridClient _sendGridClient;

        public EmailSendingService(ISendGridClient sendGridClient)
        {
            _sendGridClient = sendGridClient;
        }

        public Task<JobLogic.Infrastructure.ServiceResponders.Response> SendMessageAsync(
            string emailFrom, string emailTo, string subject,
            string body,
            string replyToAddress = null, string ccEmailAddress = null, string bccEmailAddress = null,
            Dictionary<string, string> images = null,
            Guid? tenantId = null, Guid? communicationLogId = null, params EmailAttachment[] attachments)
        {
            var message = new EmailMessageModel(emailFrom, emailTo, subject, body, replyToAddress, ccEmailAddress, bccEmailAddress, images, attachments);
            message.CommunicationLogId = communicationLogId;
            message.TenantId = tenantId;

            var sendGridMessage = ToSendGridMessage(message, new Personalization());
            return SendMessge(sendGridMessage);
        }

        public Task<ServiceResponders.Response> SendMessageAsync(
            string emailFrom, string emailTo, string subject,
            string templateId, Dictionary<string, object> data,
            string replyToAddress = null, string ccEmailAddress = null, string bccEmailAddress = null,
            Dictionary<string, string> images = null,
            Guid? tenantId = null, Guid? communicationLogId = null, params EmailAttachment[] attachments)
        {
            var message = new EmailMessageModel(emailFrom, emailTo, subject, templateId, data, replyToAddress, ccEmailAddress, bccEmailAddress, images, attachments);
            message.CommunicationLogId = communicationLogId;
            message.TenantId = tenantId;

            var sendGridMessage = ToSendGridMessage(message, new Personalization());
            return SendMessge(sendGridMessage);
        }

        private SendGridMessage ToSendGridMessage(EmailMessageModel message, Personalization personalization)
        {
            var msg = new SendGridMessage
            {
                From = new EmailAddress(message.From),
                Subject = message.Subject,
                HtmlContent = message.Body
            };

            if (message.TenantId.HasValue && message.CommunicationLogId.HasValue)
            {
                var customArgs = new Dictionary<string, string>
                {
                    { "TenantId", message.TenantId.Value.ToString() },
                    { "CommunicationLogId", message.CommunicationLogId.Value.ToString() }
                };
                if (personalization != null)
                {
                    personalization.CustomArgs = customArgs;
                }
                else
                {
                    msg.AddCustomArgs(customArgs, personalization: personalization);
                }
                msg.SetClickTracking(true, false);
                msg.SetOpenTracking(true);
                msg.SetSubscriptionTracking(true);
            }

            #region [To, ReplyTo, CC, BCC]
            personalization.Tos = new List<EmailAddress>();
            var emailsTo = new List<EmailAddress>();
            CreateEmailAddressesForSendGrid(msg, emailsTo, message.To);
            msg.AddTos(emailsTo, personalization: personalization);

            if (!string.IsNullOrWhiteSpace(message.ReplyTo) && message.ReplyTo.isEmail())
                msg.SetReplyTo(new EmailAddress(message.ReplyTo));

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

            var messageAttachments = new List<Attachment>();
            if (message.Attachments != null)
            {
                foreach (var attachmentByteArray in message.Attachments)
                {
                    if (attachmentByteArray.Attachment != null)
                    {
                        var attachmentBase64 = ImageUtils.ConvertImageToBase64(
                            attachmentByteArray.AttachmentBytes).RemovePrefix(ImageUtils.ImageBase64Prefix);
                        messageAttachments.Add(new Attachment
                        {
                            Content = attachmentBase64,
                            Filename = attachmentByteArray.FileName,
                            Disposition = "attachment"
                        });
                    }
                }
            }

            if (message.Images != null)
            {
                foreach (var image in message.Images)
                {
                    if (!string.IsNullOrWhiteSpace(image.Value))
                    {
                        messageAttachments.Add(new Attachment
                        {
                            Content = image.Value.RemovePrefix(ImageUtils.ImageBase64Prefix),
                            Disposition = "inline",
                            Filename = "companyLogo.jpg",
                            ContentId = image.Key
                        });
                    }
                }
            }

            if (messageAttachments.Any())
                msg.AddAttachments(messageAttachments);

            if (!string.IsNullOrWhiteSpace(message.Template))
            {
                msg.SetTemplateId(message.Template);

                if (message.Data != null && personalization != null)
                {
                    var templateData = new Dictionary<string, object>();
                    foreach (var obj in message.Data)
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

        private async Task<ServiceResponders.Response> SendMessge(SendGridMessage sendGridMessage)
        {
            if (!IsValidPersonalizations(sendGridMessage))
                return ResponseFactory.ReturnWithError($"Invalid email message");

            var response = await _sendGridClient.SendEmailAsync(sendGridMessage);
            if (response.StatusCode != HttpStatusCode.Accepted)
                return ResponseFactory.ReturnWithError($"{response.StatusCode} - SendGrid responsed errors: { await response.Body?.ReadAsStringAsync() }");

            return ResponseFactory.Return();
        }
    }
}
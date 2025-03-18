using JobLogic.Infrastructure.EmailSendingService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Test.Integration.Joblogic.Infrastructure.EmailSendingServices
{
    [TestClass]
    public class EmailSendingServiceTests
    {
        const string RMG_Job_Creation_Notification_TemplateId = "d-c869586116f9454e834ec6f28fc57e6c";
        const string RMG_Job_Completion_Notification_TemplateId = "d-4da2e95f74ef4f018b0dfc7d29db37fe";
        const string RMG_Job_Appointment_Change_Notification_TemplateId = "d-d01bcaa81be04b2294c3f3b8360c27fc";
        const string RMG_Job_Cancellation_Notification_TemplateId = "d-fc216f4fe5e24187bebbf9ce4a7f663a";
        const string JobLogic_ContactUs_TemplateId = "d-d404ea66ccf449979cb67500efa8714c";

        const string RMG_London_ImageUrl = "https://marketing-image-production.s3.amazonaws.com/uploads/3511b3e71cfa4f144932c9d73cf3a21b5062898656be0022bc2be2a5f4c001c1b15ff313d4052b693e99e8797b7456d6da0a26617a8b67b6f26966f612cc6694.PNG";
        const string RMG_Scotland_ImageUrl = "https://marketing-image-production.s3.amazonaws.com/uploads/908ccf7a17cb1d6dc6452043a3d77ce12870030d33de0e57cc3246b05121f2ab75aacc6328ea1e6fef390fccf4847bade2b34864ff23adb8af2632b60a1b8f1b.png";
        const string RMG_Other_ImageUrl = "https://marketing-image-production.s3.amazonaws.com/uploads/5daa39cd53f5d53d3b5aab9fbd62f59d8be25f9fb136338c61c579cbdec7948602f7a41f084bc5d0f6029f51fae8a2542ab23fdf587ae881ea245c031bce93be.jpg";

        string emailFrom = "noreply@joblogic.com";
        string emailTo = string.Empty;

        EmailSendingService emailSendingService;

        [TestInitialize]
        public void TestInitialize()
        {
            emailSendingService = new EmailSendingService(
                new SendGrid.SendGridClient("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"));
        }

        [TestMethod]
        public async Task SendMessage_BySendGridClient_WithAttachments_ShouldReturnSuccess()
        {
            var subject = "SendMessage_BySendGridClient_WithAttachments_ShouldReturnSuccess";
            var data = new Dictionary<string, object>()
            {
                { "companyName", "JobLogic" },
                { "companyTelephone", "0123456789" },
                { "userName", "haih"},
                { "userEmailAddress", "haih@joblogic.com" },
                { "subject", "JobLogic Contact Us" },
                { "message", "Contact Us Message" }
            };
            var attachments = await GetEmailAttachments();

            var response = await emailSendingService.SendMessageAsync(
                emailFrom, emailTo, subject, JobLogic_ContactUs_TemplateId, data: data, attachments: attachments.ToArray());
            Assert.IsTrue(response.Success);
        }

        [TestMethod]
        public async Task SendMessage_ShouldReturnSuccess()
        {
            string subject = "SendMessage_ShouldReturnSuccess";
            string body = "Testing from unit test project";
            string replyToAddress = null;// "salma+replyto@joblogic.com";
            string ccEmailAddress = null;// "salma+cc@joblogic.com";
            string bccEmailAddress = null;//  "abdullahi+bcc@joblogic.com";
            Dictionary<string, string> images = null;
            EmailAttachment[] attachments = null;

            var response = await emailSendingService.SendMessageAsync(emailFrom, emailTo, subject, body, replyToAddress, ccEmailAddress, bccEmailAddress);
            Assert.IsTrue(response.Success);
        }


        [TestMethod]
        public async Task SendMessageWithRmgLondonTemplateForJobCreation_ShouldReturnSuccess()
        {
            var subject = "SendMessageWithRmgLondonTemplateForJobCreation_ShouldReturnSuccess";
            var templateId = RMG_Job_Creation_Notification_TemplateId;
            var data = new Dictionary<string, object>();

            data.Add("rmgJobId", "OrderNoLONDON123");
            data.Add("ContractorName", "Bob London Ltd");
            data.Add("rmgdate", "02/10/2018");
            data.Add("rmgBrandTelephone", "0207 598 1600");
            data.Add("rmgBrandEmailaddress", "customer.service@rmguk.london");
            data.Add("rmgBrandImage", RMG_London_ImageUrl);
            data.Add("subject", subject);

            var response = await emailSendingService.SendMessageAsync(emailFrom, emailTo, subject, templateId, data);
            Assert.IsTrue(response.Success);
        }

        [TestMethod]
        public async Task SendMessageWithRmgScotlandTemplateForJobCreation_ShouldReturnSuccess()
        {
            var subject = "SendMessageWithRmgScotlandTemplateForJobCreation_ShouldReturnSuccess";
            var templateId = RMG_Job_Creation_Notification_TemplateId;
            var data = new Dictionary<string, object>();

            data.Add("rmgJobId", "OrderNoSCOTLAND123");
            data.Add("ContractorName", "Bob Scotland Ltd");
            data.Add("rmgdate", "02/11/2018");
            data.Add("rmgBrandTelephone", "0345 002 4499");
            data.Add("rmgBrandEmailaddress", "customerservice@rmgscotland.com");
            data.Add("rmgBrandImage", RMG_Scotland_ImageUrl);
            data.Add("subject", subject);

            var response = await emailSendingService.SendMessageAsync(emailFrom, emailTo, subject, templateId, data);
            Assert.IsTrue(response.Success);
        }

        [TestMethod]
        public async Task SendMessageWithRmgOtherTemplateForJobCreation_ShouldReturnSuccess()
        {
            var subject = "SendMessageWithRmgOtherTemplateForJobCreation_ShouldReturnSuccess";
            var templateId = RMG_Job_Creation_Notification_TemplateId;
            var data = new Dictionary<string, object>();

            data.Add("rmgJobId", "OrderNo123");
            data.Add("ContractorName", "Bob Ltd");
            data.Add("rmgdate", "02/12/2018");
            data.Add("rmgBrandTelephone", "0345 002 4444");
            data.Add("rmgBrandEmailaddress", "customer.service@rmguk.com");
            data.Add("rmgBrandImage", RMG_Other_ImageUrl);
            data.Add("subject", subject);

            var response = await emailSendingService.SendMessageAsync(emailFrom, emailTo, subject, templateId, data);
            Assert.IsTrue(response.Success);
        }


        [TestMethod]
        public async Task SendMessageWithRmgLondonTemplateForJobCompletion_ShouldReturnSuccess()
        {
            var subject = "SendMessageWithRmgLondonTemplateForJobCompletion_ShouldReturnSuccess";
            var templateId = RMG_Job_Completion_Notification_TemplateId;
            var data = new Dictionary<string, object>();

            data.Add("rmgJobId", "OrderNoLONDON123");
            data.Add("ContractorName", "Bob London Ltd");
            data.Add("rmgdate", "02/10/2018");
            data.Add("rmgBrandTelephone", "0207 598 1600");
            data.Add("rmgBrandEmailaddress", "customer.service@rmguk.london");
            data.Add("rmgBrandImage", RMG_London_ImageUrl);
            data.Add("subject", subject);

            var response = await emailSendingService.SendMessageAsync(emailFrom, emailTo, subject, templateId, data);
            Assert.IsTrue(response.Success);
        }

        [TestMethod]
        public async Task SendMessageWithRmgScotlandTemplateForJobCompletion_ShouldReturnSuccess()
        {
            var subject = "SendMessageWithRmgScotlandTemplateForJobCompletion_ShouldReturnSuccess";
            var templateId = RMG_Job_Completion_Notification_TemplateId;
            var data = new Dictionary<string, object>();

            data.Add("rmgJobId", "OrderNoSCOTLAND123");
            data.Add("ContractorName", "Bob Scotland Ltd");
            data.Add("rmgdate", "02/11/2018");
            data.Add("rmgBrandTelephone", "0345 002 4499");
            data.Add("rmgBrandEmailaddress", "customerservice@rmgscotland.com");
            data.Add("rmgBrandImage", RMG_Scotland_ImageUrl);
            data.Add("subject", subject);

            var response = await emailSendingService.SendMessageAsync(emailFrom, emailTo, subject, templateId, data);
            Assert.IsTrue(response.Success);
        }

        [TestMethod,]
        public async Task SendMessageWithRmgOtherTemplateForJobCompletion_ShouldReturnSuccess()
        {
            var subject = "SendMessageWithRmgOtherTemplateForJobCompletion_ShouldReturnSuccess";
            var templateId = RMG_Job_Completion_Notification_TemplateId;
            var data = new Dictionary<string, object>();

            data.Add("rmgJobId", "OrderNo123");
            data.Add("ContractorName", "Bob Ltd");
            data.Add("rmgdate", "02/12/2018");
            data.Add("rmgBrandTelephone", "0345 002 4444");
            data.Add("rmgBrandEmailaddress", "customer.service@rmguk.com");
            data.Add("rmgBrandImage", RMG_Other_ImageUrl);
            data.Add("subject", subject);

            var response = await emailSendingService.SendMessageAsync(emailFrom, emailTo, subject, templateId, data);
            Assert.IsTrue(response.Success);
        }


        [TestMethod]
        public async Task SendMessageWithRmgLondonTemplateForJobAppointmentChange_ShouldReturnSuccess()
        {
            var subject = "SendMessageWithRmgLondonTemplateForJobAppointmentChange_ShouldReturnSuccess";
            var templateId = RMG_Job_Appointment_Change_Notification_TemplateId;
            var data = new Dictionary<string, object>();

            data.Add("rmgJobId", "OrderNoLONDON123");
            data.Add("ContractorName", "Bob London Ltd");
            data.Add("rmgdate", "02/10/2018");
            data.Add("rmgBrandTelephone", "0207 598 1600");
            data.Add("rmgBrandEmailaddress", "customer.service@rmguk.london");
            data.Add("rmgBrandImage", RMG_London_ImageUrl);
            data.Add("subject", subject);

            var response = await emailSendingService.SendMessageAsync(emailFrom, emailTo, subject, templateId, data);
            Assert.IsTrue(response.Success);
        }

        [TestMethod]
        public async Task SendMessageWithRmgScotlandTemplateForJobAppointmentChange_ShouldReturnSuccess()
        {
            var subject = "SendMessageWithRmgScotlandTemplateForJobAppointmentChange_ShouldReturnSuccess";
            var templateId = RMG_Job_Appointment_Change_Notification_TemplateId;
            var data = new Dictionary<string, object>();

            data.Add("rmgJobId", "OrderNoSCOTLAND123");
            data.Add("ContractorName", "Bob Scotland Ltd");
            data.Add("rmgdate", "02/11/2018");
            data.Add("rmgBrandTelephone", "0345 002 4499");
            data.Add("rmgBrandEmailaddress", "customerservice@rmgscotland.com");
            data.Add("rmgBrandImage", RMG_Scotland_ImageUrl);
            data.Add("subject", subject);

            var response = await emailSendingService.SendMessageAsync(emailFrom, emailTo, subject, templateId, data);
            Assert.IsTrue(response.Success);
        }

        [TestMethod]
        public async Task SendMessageWithRmgOtherTemplateJobAppointmentChange_ShouldReturnSuccess()
        {
            var subject = "SendMessageWithRmgOtherTemplateJobAppointmentChange_ShouldReturnSuccess";
            var templateId = RMG_Job_Appointment_Change_Notification_TemplateId;
            var data = new Dictionary<string, object>();

            data.Add("rmgJobId", "OrderNo123");
            data.Add("ContractorName", "Bob Ltd");
            data.Add("rmgdate", "02/12/2018");
            data.Add("rmgBrandTelephone", "0345 002 4444");
            data.Add("rmgBrandEmailaddress", "customer.service@rmguk.com");
            data.Add("rmgBrandImage", RMG_Other_ImageUrl);
            data.Add("subject", subject);

            var response = await emailSendingService.SendMessageAsync(emailFrom, emailTo, subject, templateId, data);
            Assert.IsTrue(response.Success);
        }


        [TestMethod]
        public async Task SendMessageWithRmgOtherTemplateJobCancellation_ShouldReturnSuccess()
        {
            var rmgJobId = "OrderNo123";
            var subject = "SendMessageWithRmgOtherTemplateJobCancellation_ShouldReturnSuccess - " + rmgJobId;
            var templateId = RMG_Job_Cancellation_Notification_TemplateId;
            var data = new Dictionary<string, object>();

            data.Add("rmgJobId", rmgJobId);
            data.Add("ContractorName", "Bob Ltd");
            data.Add("rmgReason", "Access Issues");
            data.Add("rmgComments", "No access");
            data.Add("subject", subject);

            var response = await emailSendingService.SendMessageAsync(emailFrom, emailTo, subject, templateId, data);
            Assert.IsTrue(response.Success);
        }

        private async Task<IEnumerable<EmailAttachment>> GetEmailAttachments()
        {
            var attachmens = new List<EmailAttachment>();
            var paths = Directory.GetFiles("AttachmentFiles");

            foreach (var path in paths)
            {
                var fileInfo = new FileInfo(path);
                var bytes = await File.ReadAllBytesAsync(path);

                attachmens.Add(new EmailAttachment(bytes, fileInfo.Name));
            }

            return attachmens;
        }
    }
}

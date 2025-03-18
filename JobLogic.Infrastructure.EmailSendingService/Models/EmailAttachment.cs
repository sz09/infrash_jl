using System.IO;
using System.Net.Mail;

namespace JobLogic.Infrastructure.EmailSendingService
{
    public class EmailAttachment
    {
        public EmailAttachment(byte[] attachmentBytes, string fileName)
        {
            AttachmentBytes = attachmentBytes;
            FileName = fileName;
        }

        public byte[] AttachmentBytes { get; set; }
        public string FileName { get; set; }

        public Attachment Attachment
        {
            get
            {
                if (AttachmentBytes == null) return null;

                return new Attachment(new MemoryStream(AttachmentBytes), FileName);
            }
        }
    }
}

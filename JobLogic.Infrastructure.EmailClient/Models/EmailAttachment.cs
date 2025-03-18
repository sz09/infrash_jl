namespace JobLogic.Infrastructure.EmailClient
{
    public sealed class EmailAttachment
    {
        public string FileName { get; set; }
        public string Base64Content { get; set; }
        public string InlineContentId { get; set; }
        public bool IsInlineAttachment => !string.IsNullOrEmpty(InlineContentId);
    }
}

using System.IO;

namespace JobLogic.Infrastructure.ImageHelper
{
    public class ImageResizeRequest
    {
        public Stream Stream { get; }
        public string OriginalFileName { get; set; }
        public int MaxWidth { get; set; }
        public int MaxHeight { get; set; }
        public string Format { get; set; }
      
        public ImageResizeRequest(Stream imageStream, string format = "jpg")
        {
            imageStream.Seek(0, SeekOrigin.Begin);

            Stream = new MemoryStream();
            imageStream.CopyTo(Stream);
            Format = format;

            imageStream.Seek(0, SeekOrigin.Begin);
            Stream.Seek(0, SeekOrigin.Begin);
        }
    }
}

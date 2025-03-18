using System.IO;

namespace JobLogic.Infrastructure.ImageHelper
{
    public class ImageResizeResponse
    {
        public Stream Stream { get; private set; }
        public string SuggestedFileName { get; private set; }

        public ImageResizeResponse(Stream stream, string suggestedFileName)
        {
            Stream = stream;
            SuggestedFileName = suggestedFileName;
        }
    }
}

using System;
using System.Threading;
using System.Threading.Tasks;

namespace JobLogic.Infrastructure.PDF
{
    public class HtmlToPdfConvertion: IPdfConvertion
    {
        [Obsolete("Should use Async version instead")]
        public byte[] WriteToByteArray(string html)
        {
            return new HtmlToPdfConversionService().WritePDFAsByteArray(html);
        }

        public Task<byte[]> WriteToByteArrayAsync(string html, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return new HtmlToPdfConversionService().WritePDFAsByteArrayAsync(html);
        }
    }
}

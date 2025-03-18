using System;
using System.Threading;
using System.Threading.Tasks;

namespace JobLogic.Infrastructure.PDF
{
    public interface IPdfConvertion
    {
        [Obsolete("Should use Async version instead")]
        byte[] WriteToByteArray(string HTML);

        Task<byte[]> WriteToByteArrayAsync(string HTML, CancellationToken cancellationToken = default);
    }
}

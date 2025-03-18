using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace JobLogic.Infrastructure.PDF
{
    public interface IHtmlToPdfConversionService
    {
        string GetPdfExeLocation();

        [Obsolete("Should use Async version instead")]
        byte[] WritePDFAsByteArray(string HTML);

        Task<byte[]> WritePDFAsByteArrayAsync(string HTML, CancellationToken cancellationToken = default);

        [Obsolete("Should use Async version instead")]
        MemoryStream WritePDFToStream(string HTML, string outputPath = null);

        Task<MemoryStream> WritePDFToStreamAsync(string HTML, string outputPath = null, CancellationToken cancellationToken = default);
    }

    public class HtmlToPdfConversionService : IHtmlToPdfConversionService
    {
        public string GetPdfExeLocation()
        {
            var fileName = string.Empty;
            var exeFilePath = ConfigurationManager.AppSettings["HTMLToPdfExeFilePath"];
            if (HttpContext.Current != null)
            {
                fileName = HttpContext.Current.Server.MapPath(exeFilePath);
            }
            else
            {
                fileName = Directory.GetCurrentDirectory() + exeFilePath;
            }
            var t = File.Exists(fileName);
            return fileName;
        }

        [Obsolete("Should use Async version instead")]
        public byte[] WritePDFAsByteArray(string HTML)
        {
            var pdfFileStream = WritePDFToStream(HTML);
            var byteArray = pdfFileStream.ToArray();
            pdfFileStream.Dispose();
            return byteArray;
        }

        public async Task<byte[]> WritePDFAsByteArrayAsync(string HTML, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var pdfFileStream = await WritePDFToStreamAsync(HTML);
            var byteArray = pdfFileStream.ToArray();
            pdfFileStream.Dispose();
            return byteArray;
        }

        [Obsolete("Should use Async version instead")]
        public MemoryStream WritePDFToStream(string HTML, string outputPath = null)
        {

            var processStartInfo = new ProcessStartInfo()
            {
                UseShellExecute = false,
                FileName = GetPdfExeLocation(),
                CreateNoWindow = true,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                Arguments = "-s A4 -q -n --disable-smart-shrinking " + " - -"
            };

            var process = Process.Start(processStartInfo);
            try
            {
                var streamInput = process.StandardInput;
                streamInput.AutoFlush = true;
                streamInput.Write(HTML);
                streamInput.Dispose();
                streamInput.Close();

                var pdf = new MemoryStream();
                process.StandardOutput.BaseStream.CopyTo(pdf);
                process.StandardOutput.Close();
                pdf.Position = 0;
                if (process.WaitForExit(60000))
                {
                    // NOTE: the application hangs when we use WriteFile (due to the Delete below?); this works
                    return pdf;
                }
            }
            finally
            {
                process.Close();
                process.Dispose();
            }

            return null;
        }

        public async Task<MemoryStream> WritePDFToStreamAsync(string HTML, string outputPath = null, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var processStartInfo = new ProcessStartInfo()
            {
                UseShellExecute = false,
                FileName = GetPdfExeLocation(),
                CreateNoWindow = true,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                Arguments = "-s A4 -q -n --disable-smart-shrinking " + " - -"
            };

            var process = Process.Start(processStartInfo);
            try
            {
                var streamInput = process.StandardInput;
                streamInput.AutoFlush = true;
                await streamInput.WriteAsync(HTML);
                streamInput.Dispose();
                streamInput.Close();

                var pdf = new MemoryStream();
                await process.StandardOutput.BaseStream.CopyToAsync(pdf);
                process.StandardOutput.Close();
                pdf.Position = 0;
                if (process.WaitForExit(60000))
                {
                    // NOTE: the application hangs when we use WriteFile (due to the Delete below?); this works
                    return pdf;
                }
            }
            finally
            {
                process.Close();
                process.Dispose();
            }

            return null;
        }
    }
}

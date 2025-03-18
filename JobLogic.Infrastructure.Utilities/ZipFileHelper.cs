using System.IO;
using System.IO.Compression;

namespace JobLogic.Infrastructure.Utilities
{
    public static class ZipFileHelper
    {
        public static Stream CompressStream(this byte[] dataByte)
        {
            using (MemoryStream compressedMemoryStream = new MemoryStream())
            {
                using (var compressionStream = new GZipStream(compressedMemoryStream, CompressionMode.Compress, true))
                {
                    compressionStream.Write(dataByte, 0, dataByte.Length);
                }
                return new MemoryStream(compressedMemoryStream.ToArray());
            }
        }

        public static Stream DeCompressStream(this Stream compressedStream)
        {
            var output = new MemoryStream();
            using (var zipStream = new GZipStream(compressedStream, CompressionMode.Decompress, true))
            {
                try
                {
                    zipStream.CopyTo(output);
                    output.Position = 0;
                    compressedStream.Close();
                    return output;
                }
                catch
                {
                    //return original stream if can't decompressed
                    compressedStream.Position = 0;
                    return compressedStream;
                }
            }
        }

        public static Stream DeCompressStream(this byte[] compressedByte)
        {
            return DeCompressStream(new MemoryStream(compressedByte));
        }
    }
}

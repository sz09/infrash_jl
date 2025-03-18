using MimeTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace JobLogic.Infrastructure.Utilities
{
    public static class FileUtils
    {
        [Obsolete("Should use Async version instead")]
        public static byte[] GetFile(string filePath)
        {
            try
            {
                byte[] fileBytes = File.ReadAllBytes(filePath);
                return fileBytes;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static async Task<byte[]> GetFileAsync(string filePath, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            byte[] fileBytes;
            using (var stream = File.OpenRead(filePath))
            {
                fileBytes = new byte[stream.Length];
                await stream.ReadAsync(fileBytes, 0, (int)stream.Length, cancellationToken);
            }
            return fileBytes;
        }

        public static string ValidateFileName(this string fileName)
        {
            var r = new Regex(string.Format("[{0}]", Regex.Escape(new string(Path.GetInvalidFileNameChars()))));
            return r.Replace(fileName, "");
        }

        public static string MimeType(this string fileExtension)
        {
            return string.IsNullOrWhiteSpace(fileExtension)
                    ? "application/unknown"
                    : MimeTypeMap.GetMimeType(fileExtension);
        }

        public static string GenerateFileName(string prefixPart, string middlePart, string suffixPart = null, string fileType = "pdf")
        {
            if (string.IsNullOrWhiteSpace(prefixPart) && string.IsNullOrWhiteSpace(middlePart) && string.IsNullOrWhiteSpace(suffixPart))
            {
                throw new ArgumentNullException();
            }

            var parts = new List<string> { prefixPart, middlePart, suffixPart };

            var fileName = string.Format("{0}.{1}", string.Join(" - ", parts.Where(x => !string.IsNullOrEmpty(x))), fileType);
            return fileName.ValidateFileName();
        }

        public static string GetExtension(this string fileName, bool withDot = true)
        {
            if (!string.IsNullOrWhiteSpace(fileName))
            {
                var fileExtPos = fileName.LastIndexOf(".");
                if (fileExtPos >= 0)
                {
                    return fileName.Substring(fileExtPos + (withDot ? 0 : 1));
                }
            }

            return string.Empty;
        }

        public static bool IsValidUpload(string fileName, string whiteList)
        {
            var extension = GetExtension(fileName, false);
            if (!string.IsNullOrEmpty(extension) &&
                !string.IsNullOrEmpty(whiteList))
            {
                var extensionTypes = whiteList.ToLower().Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                extension = extension.Replace(".", "").ToLower();
                return extensionTypes.Any(x => x.ToLower() == extension);
            }
            return false;
        }

        [Obsolete("Should use Async version instead")]
        public static byte[] StreamToByteArray(Stream stream)
        {
            if (stream is MemoryStream)
            {
                return ((MemoryStream)stream).ToArray();
            }
            else
            {
                // Jon Skeet's accepted answer 
                return ReadFully(stream);
            }
        }

        public static async Task<byte[]> StreamToByteArrayAsync(Stream stream, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (stream is MemoryStream)
            {
                return ((MemoryStream)stream).ToArray();
            }
            else
            {
                return await ReadFullyAsync(stream, cancellationToken);
            }
        }

        [Obsolete("Should use Async version instead")]
        public static byte[] ReadFully(Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                input.Position = 0;
                return ms.ToArray();
            }
        }

        public static async Task<byte[]> ReadFullyAsync(Stream input, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var ms = new MemoryStream())
            {
                await input.CopyToAsync(ms);
                input.Position = 0;
                return ms.ToArray();
            }
        }

        public static T Deserialize<T>(this XmlReader xmlReader)
        {
            var serializer = new XmlSerializer(typeof(T));
            return (T)serializer.Deserialize(xmlReader);
        }

		public static byte[] SaveToCsv<T>(List<T> reportData)
        {
            Func<T, string, string> GetRowValue = (row, fields) =>
            {
                return string.Join(",", fields.Split(',')
                .Select(a =>
                {
                    var propertyInfo = row.GetType().GetProperty(a);
                    var columnValue = propertyInfo.GetValue(row, null);
                    if (propertyInfo.PropertyType == typeof(string) && columnValue != null)
                    {
                        return StringUtils.AsDelimiteredString(columnValue.ToString());
                    }

                    return columnValue;
                }));
            };
            var lines = new List<string>();
            IEnumerable<PropertyDescriptor> props = TypeDescriptor.GetProperties(typeof(T)).OfType<PropertyDescriptor>();
            var header = string.Join(",", props.Select(x => x.DisplayName));
            var fieldNames = string.Join(",", props.Select(x => x.Name));
            lines.Add(header);
            var valueLines = reportData.Select(row => GetRowValue(row, fieldNames));
            lines.AddRange(valueLines);
            return lines.SelectMany(s => Encoding.UTF8.GetBytes(s + Environment.NewLine)).ToArray();
        }
    }

    public class BlobStorage
    {
        public BlobStorage(string container, string fileName, string blobReference)
        {
            Container = container;
            FileName = fileName;
            BlobReference = blobReference;
        }
        public string Container { get; set; }
        public string FileName { get; set; }
        public string BlobReference { get; set; }
    }
}

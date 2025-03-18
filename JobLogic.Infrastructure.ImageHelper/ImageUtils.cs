using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace JobLogic.Infrastructure.ImageHelper
{
    public static class ImageUtils
    {
        public const string ImageBase64Prefix = "data:image/jpeg;base64,";
        private const int OrientationKey = 0x0112;
        private const int NotSpecified = 0;
        private const int NormalOrientation = 1;
        private const int MirrorHorizontal = 2;
        private const int UpsideDown = 3;
        private const int MirrorVertical = 4;
        private const int MirrorHorizontalAndRotateRight = 5;
        private const int RotateLeft = 6;
        private const int MirorHorizontalAndRotateLeft = 7;
        private const int RotateRight = 8;

        public static string ConvertImageToBase64(this string filelocation)
        {
            var byteArray = File.ReadAllBytes(filelocation);
            return ConvertImageToBase64(byteArray);
        }

        public static string ConvertImageToBase64(this Stream stream)
        {
            if (stream == null) return string.Empty;

            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                var byteArray = memoryStream.ToArray();

                return ConvertImageToBase64(byteArray);
            }
        }

        public static string ConvertImageToBase64(this byte[] data)
        {
            if (data == null) return string.Empty;
            var base64 = Convert.ToBase64String(data);
            return string.Format("{0}{1}", ImageBase64Prefix, base64);
        }

        public static bool IsImage(this Stream inputStream)
        {
            try
            {
                using (var bitmap = new Bitmap(inputStream))
                {
                    return !bitmap.Size.IsEmpty;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool IsImage(string contentType, string fileName, Stream inputStream)
        {
            // Check Content Type
            contentType = contentType.ToLower();
            if (contentType == "image/jpg" ||
                        contentType == "image/jpeg" ||
                        contentType == "image/png")
            {
                return inputStream.IsImage();
            }

            // Check Extension
            return fileName.IsImage() && inputStream.IsImage();
        }

        public static bool IsImage(this string filePath)
        {
            var extension = GetExtension(filePath);
            if (extension != null)
            {
                var fileName = extension.ToLower();
                if (fileName == ".jpg" ||
                    fileName == ".png" ||
                    fileName == ".jpeg" ||
                    fileName == ".bmp")
                {
                    return true;
                }
            }
            return false;
        }

        public static Bitmap Resize(this Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.Clear(Color.White);
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                graphics.DrawImage(image, destRect);

                // Fix orientation if needed.
                //https://stackoverflow.com/questions/33310562/some-images-are-being-rotated-when-resized
                if (image.PropertyIdList.Contains(OrientationKey))
                {
                    var orientation = (int)image.GetPropertyItem(OrientationKey).Value[0];
                    switch (orientation)
                    {
                        case NotSpecified: // Assume it is good.
                        case NormalOrientation:
                            // No rotation required.
                            break;
                        case MirrorHorizontal:
                            destImage.RotateFlip(RotateFlipType.RotateNoneFlipX);
                            break;
                        case UpsideDown:
                            destImage.RotateFlip(RotateFlipType.Rotate180FlipNone);
                            break;
                        case MirrorVertical:
                            destImage.RotateFlip(RotateFlipType.Rotate180FlipX);
                            break;
                        case MirrorHorizontalAndRotateRight:
                            destImage.RotateFlip(RotateFlipType.Rotate90FlipX);
                            break;
                        case RotateLeft:
                            destImage.RotateFlip(RotateFlipType.Rotate90FlipNone);
                            break;
                        case MirorHorizontalAndRotateLeft:
                            destImage.RotateFlip(RotateFlipType.Rotate270FlipX);
                            break;
                        case RotateRight:
                            destImage.RotateFlip(RotateFlipType.Rotate270FlipNone);
                            break;
                        default:
                            //throw new NotImplementedException("An orientation of " + orientation + " isn't implemented.");
                            break;
                    }
                }
            }

            return destImage;
        }

        public static void GetImageSize(this Image image, out int height, out int width, int maxWidth = 400)
        {
            height = image.Height;
            width = image.Width;

            GetImageSize(ref width, ref height, maxWidth);
        }

        public static void GetImageSizeByHeight(this Image image, out int height, out int width, int maxHeight = 100)
        {
            height = image.Height;
            width = image.Width;

            GetImageSize(ref height, ref width, maxHeight);
        }

        public static void GetImageSize(ref int dimension1, ref int dimension2, int maxDimension1)
        {
            if (dimension1 > maxDimension1)
            {
                int oldDimension1 = dimension1;
                dimension1 = maxDimension1;

                // Calculate what new height is as a percentage of original height
                var newWidthAsPercentageOfOriginal = (maxDimension1 * 100.0) / oldDimension1;

                // Use height percentage to recalculate width (keeping aspect ratio)
                dimension2 = (int)((newWidthAsPercentageOfOriginal * dimension2) / 100.0);
            }
        }

        public static ImageResizeResponse Resize(this ImageResizeRequest request)
        {
            ImageResizeResponse result = null;
            int height = 0;
            int width = 0;

            var suggestedFileName = string.Empty;
            if (request.OriginalFileName != null)
                suggestedFileName = GetSuggestedResizeFileName(request.OriginalFileName, request.MaxWidth, request.MaxHeight, request.Format);

            using (var img = Image.FromStream(request.Stream))
            {
                img.GetImageSize(out height, out width, request.MaxWidth);
                using (var bitmap = img.Resize(width, height))
                {
                    result = new ImageResizeResponse(bitmap.ToStream(ImageFormat.Jpeg), suggestedFileName);
                }
            }

            return result;
        }

        public static Stream ToStream(this Image image, ImageFormat format)
        {
            var stream = new MemoryStream();
            image.Save(stream, format);
            stream.Position = 0;
            return stream;
        }

        public static string GetSuggestedResizeFileName(this string originalFileName, int width, int height, string format = "jpg")
        {
            var fileName = Path.GetFileNameWithoutExtension(originalFileName);

            return $"{width}x{height}_{fileName}.{format}";
        }

        public static Color HexStringToColor(string hex)
        {
            if ((hex != null) && (hex != ""))
            {
                hex = hex.Replace("#", "");

                if (hex.Length != 8)
                    throw new Exception(hex +
                        " is not a valid 6-place hexadecimal color code.");

                string r, g, b, a;

                a = hex.Substring(0, 2);
                b = hex.Substring(2, 2);
                g = hex.Substring(4, 2);
                r = hex.Substring(6, 2);

                return Color.FromArgb(
                                                    HexStringToBase10Int(r),
                                                     HexStringToBase10Int(g),
                                                     HexStringToBase10Int(b));
            }
            return Color.Gray;
        }

        public static int HexStringToBase10Int(string hex)
        {
            if ((hex != null) && (hex != ""))
            {
                int base10value = 0;

                try { base10value = System.Convert.ToInt32(hex, 16); }
                catch { base10value = 0; }

                return base10value;
            }
            return 0;
        }

        private static string GetExtension(this string fileName, bool withDot = true)
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
    }
}

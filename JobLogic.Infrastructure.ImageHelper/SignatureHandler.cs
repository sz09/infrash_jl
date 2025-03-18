using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace JobLogic.Infrastructure.ImageHelper
{
    public static class SignatureHandler
    {
        public static byte[] ConvertSignatureToBytes(string signatureData, int width = 240, int height = 104)
        {
            if (string.IsNullOrEmpty(signatureData)) return null;
            try
            {
                if (SignatureIsBase64(signatureData))
                {
                    return SplitBase64StringAndConvertToBytes(signatureData);
                }

                byte[] result = null;
                using (var bitmap = ConvertCoordinatesToImage(signatureData, width, height))
                {
                    result = ConvertImageToBytes(bitmap);
                }

                return result;
            }
            catch
            {
                return null;
            }
        }

        private static byte[] SplitBase64StringAndConvertToBytes(string signatureData)
        {
            // We convert SVG signatures to base-64 in MobileV2 -> TaskProcessors: Complete and LeftSite
            var base64Split = signatureData.Split(',');
            return Convert.FromBase64String(base64Split[1]);
        }

        /// <summary>
        /// Joblogic default signature default with 240 and default height is 104
        /// </summary>
        /// <param name="signatureCords"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        private static Bitmap ConvertCoordinatesToImage(string signatureCords, int width = 240, int height = 104)
        {
            var Sstr = signatureCords;
            var objMyBitMap = new Bitmap(width, height);
            using (var objGraphics = Graphics.FromImage(objMyBitMap))
            {
                objGraphics.Clear(Color.White);
                using (var myPen = new Pen(Color.Black, 1))
                {
                    var lines = Sstr.Split('*');
                    for (var i = 0; i <= lines.Length - 1; i++)
                    {
                        var points = lines[i].Split(',');
                        for (int j = 0; j <= points.Length - 2; j++)
                        {
                            var xCord = (float)Convert.ToDouble((points[j].Split('.'))[0]);
                            var yCord = (float)Convert.ToDouble((points[j].Split('.'))[1]);

                            var xCord1 = (float)Convert.ToDouble((points[j + 1].Split('.'))[0]);
                            var yCord1 = (float)Convert.ToDouble((points[j + 1].Split('.'))[1]);

                            objGraphics.DrawLine(myPen, xCord, yCord, xCord1, yCord1);
                        }
                    }
                }
            }

            return objMyBitMap;
        }

        private static byte[] ConvertImageToBytes(Bitmap img)
        {
            var byteArray = new byte[0];
            using (var stream = new MemoryStream())
            {
                img.Save(stream, ImageFormat.Jpeg);
                byteArray = stream.ToArray();
            }

            return byteArray;
        }

        private static bool SignatureIsBase64(string signatureData) => signatureData.Contains("base64,");
    }
}
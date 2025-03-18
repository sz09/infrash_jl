using FluentAssertions;
using JobLogic.Infrastructure.UnitTest;
using JobLogic.Infrastructure.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Text;

namespace Tests.Unit.Joblogic.Infrastructure.Utilites
{
    [TestClass]
    public class FileUtilsTests : BaseUnitTest
    {
        [TestMethod]
        public void ValidateFileName_WithAllValidCharacters_Should_Return_Same_Value()
        {
            // Arrange
            var expectedValue = "FunnyPicOfCats.jpg";

            // Action
            var result = expectedValue.ValidateFileName();

            // Assert
            result.Should().Be(expectedValue);
        }

        [TestMethod]
        public void ValidateFileName_WithInvalidCharacters_Should_Remove_Invalid_Characters()
        {
            // Arrange
            var invalidValue = $"FunnyPicOfCats{GetNInvalidFileNameChars(5)}.jpg";
            var expectedValue = "FunnyPicOfCats.jpg";

            // Action
            var result = invalidValue.ValidateFileName();

            // Assert
            result.Should().Be(expectedValue);
        }

        private static string GetNInvalidFileNameChars(int n)
        {
            var randomNumberGen = new Random();
            var invalidFileNameChars = Path.GetInvalidFileNameChars();
            var suffix = new StringBuilder();
            for (var i = 0; i < n; i++)
            {
                suffix = suffix.Append(invalidFileNameChars[randomNumberGen.Next(0, invalidFileNameChars.Length - 1)]);
            }
            return suffix.ToString();
        }


        [TestMethod]
        public void MimeType_With_Null_FileExtension_Should_Return_ApplicationUnknown()
        {
            // Arrange
            var expectedValue = "application/unknown";

            // Action
            var result = FileUtils.MimeType(null);

            // Assert
            result.Should().Be(expectedValue);
        }

        [TestMethod]
        public void MimeType_With_Empty_FileExtension_Should_Return_ApplicationUnknown()
        {
            // Arrange
            var expectedValue = "application/unknown";

            // Action
            var result = FileUtils.MimeType(string.Empty);

            // Assert
            result.Should().Be(expectedValue);
        }

        [TestMethod]
        public void MimeType_With_Whitespace_FileExtension_Should_Return_ApplicationUnknown()
        {
            // Arrange
            var expectedValue = "application/unknown";

            // Action
            var result = FileUtils.MimeType("       ");

            // Assert
            result.Should().Be(expectedValue);
        }

        [DataRow(".jpg", "image/jpeg")]
        [DataRow(".jpeg", "image/jpeg")]
        [DataRow(".png", "image/png")]
        [DataRow(".gif", "image/gif")]
        [DataRow(".pdf", "application/pdf")]
        [DataRow(".doc", "application/msword")]
        [DataRow(".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document")]
        [DataRow(".ppt", "application/vnd.ms-powerpoint")]
        [DataRow(".pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation")]
        [DataRow(".pps", "application/vnd.ms-powerpoint")]
        [DataRow(".ppsx", "application/vnd.openxmlformats-officedocument.presentationml.slideshow")]
        [DataRow(".odt", "application/vnd.oasis.opendocument.text")]
        [DataRow(".xls", "application/vnd.ms-excel")]
        [DataRow(".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")]
        [DataRow(".mp3", "audio/mpeg")]
        [DataRow(".m4a", "audio/m4a")]
        [DataRow(".ogg", "audio/ogg")]
        [DataRow(".wav", "audio/wav")]
        [DataRow(".mp4", "video/mp4")]
        [DataRow(".m4v", "video/x-m4v")]
        [DataRow(".mov", "video/quicktime")]
        [DataRow(".wmv", "video/x-ms-wmv")]
        [DataRow(".avi", "video/x-msvideo")]
        [DataRow(".mpg", "video/mpeg")]
        [DataRow(".ogv", "video/ogg")]
        [DataRow(".3gp", "video/3gpp")]
        [DataRow(".3g2", "video/3gpp2")]
        [DataRow(".txt", "text/plain")]
        [DataRow(".zip", "application/zip")]
        [DataRow(".csv", "text/csv")]
        [DataRow(".eml", "message/rfc822")]
        [DataRow(".msg", "application/vnd.ms-outlook")]
        [DataRow(".tif", "image/tiff")]
        [DataRow(".tiff", "image/tiff")]
        [DataRow(".heic", "application/octet-stream")]
        [DataRow(".bmp", "image/bmp")]
        [DataRow(".xml", "text/xml")]
        [DataRow(".ods", "application/vnd.oasis.opendocument.spreadsheet")]
        [DataRow(".xlsm", "application/vnd.ms-excel.sheet.macroEnabled.12")]
        [DataRow(".odg", "application/vnd.oasis.opendocument.graphics")]
        [TestMethod]
        public void MimeType_With_Allowed_FileExtension_Should_Return_ApplicationUnknown(string fileExtension, string expectedValue)
        {
            // Arrange
            // expectedValue passed in via DataRow attribute
            // Items are based on extensions we allowed via our whitelist as of 17/12/2020

            // Action
            var result = fileExtension.MimeType();

            // Assert
            result.Should().Be(expectedValue);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GenerateFileName_When_All_Arguments_Are_Null_Should_Throw_ArgumentNullException()
        {
            // Arrange
            // Nothing to arrange

            // Action
            _ = FileUtils.GenerateFileName(string.Empty, string.Empty, string.Empty);

            // Assert
            // Passes based on ExpectedException attribute
        }

        [TestMethod]

        [DataRow("a", null, null, "pdf", "a.pdf")]
        [DataRow("a", "b", null, "pdf", "a - b.pdf")]
        [DataRow("a", null, "c", "pdf", "a - c.pdf")]
        [DataRow(null, "b", "c", "pdf", "b - c.pdf")]
        [DataRow("a", "b", "c", "pdf", "a - b - c.pdf")]
        public void GenerateFileName_Should_Return_Correct_Value(string prefixPart, string middlePart, string suffixPart, string fileType, string expectedValue)
        {
            // Arrange
            // Nothing to arrange

            // Action
            var result = FileUtils.GenerateFileName(prefixPart, middlePart, suffixPart, fileType);

            // Assert
            result.Should().Be(expectedValue);
        }
    }
}

using FluentAssertions;
using JobLogic.Infrastructure.DeviceDetector;
using JobLogic.Infrastructure.UnitTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Unit.Joblogic.Infrastructure.DeviceDetector
{
    [TestCategory("Unit")]
    [TestClass]
    public class UAParseUtilsTests : BaseTest
    {
        [TestMethod]
        public void Parse_ReturnCorrectly_IfUAStringDesktopValid()
        {
            // Arrange
            string ua = "Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:47.0) Gecko/20100101 Firefox/47.0";
            // Action
            var result = UAParseUtils.Parse(ua);

            // Assert
            result.Browser.Should().NotBeNullOrEmpty().And.Contain("Firefox");
            result.DeviceName.Should().NotBeNullOrEmpty().And.Contain("desktop");
            result.OS.Should().NotBeNullOrEmpty().And.Contain("Windows");
        }

        [TestMethod]
        public void Parse_ReturnCorrectly_IfUAStringSamsungValid()
        {
            // Arrange
            string ua = "Mozilla/5.0 (Linux; Android 6.0.1; SAMSUNG SM-G900F Build/MMB29M) AppleWebKit/537.36 (KHTML, like Gecko) SamsungBrowser/4.0 Chrome/44.0.2403.133 Mobile Safari/537.36";
            // Action
            var result = UAParseUtils.Parse(ua);

            // Assert
            result.Browser.Should().NotBeNullOrEmpty().And.Contain("Samsung");
            result.DeviceName.Should().NotBeNullOrEmpty().And.Contain("S5").And.Contain("Samsung");
            result.OS.Should().NotBeNullOrEmpty().And.Contain("Android");
        }

        [TestMethod]
        public void Parse_ReturnNullInfo_IfUAStringNull()
        {
            // Arrange
            string ua = null;
            // Action
            var result = UAParseUtils.Parse(ua);

            // Assert
            result.Browser.Should().BeNull();
            result.DeviceName.Should().BeNull();
            result.OS.Should().BeNull();
        }

        [TestMethod]
        public void Parse_ReturnNullInfo_IfUAStringEmpty()
        {
            // Arrange
            string ua = string.Empty;
            // Action
            var result = UAParseUtils.Parse(ua);

            // Assert
            result.Browser.Should().BeNull();
            result.DeviceName.Should().BeNull();
            result.OS.Should().BeNull();
        }

        [TestMethod]
        public void Parse_ReturnNullInfo_IfUAStringInvalid()
        {
            // Arrange
            string ua = GenerateUniqueString();
            // Action
            var result = UAParseUtils.Parse(ua);

            // Assert
            result.Browser.Should().BeNull();
            result.DeviceName.Should().BeNull();
            result.OS.Should().BeNull();
        }
    }
}

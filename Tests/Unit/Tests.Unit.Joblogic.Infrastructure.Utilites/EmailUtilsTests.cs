using JobLogic.Infrastructure.UnitTest;
using JobLogic.Infrastructure.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Unit.Joblogic.Infrastructure.Utilites
{
    [TestClass]
    public class EmailUtilsTests : BaseUnitTest
    {
        [TestMethod]
        public void IsValidEmail_ReturnsTrue_WhenValidEmail()
        {
            // Arrange
            var emailaddress = "test@gmail.com";

            // Action
            var result = EmailUtils.IsValidEmail(emailaddress);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsValidEmailWithFullStop_ReturnsTrue_WhenValidEmail()
        {
            // Arrange
            var emailaddress = "test.tesing@gmail.com";

            // Action
            var result = EmailUtils.IsValidEmail(emailaddress);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsValidEmail_ReturnsFalse_WhenInValidEmail()
        {
            // Arrange
            var emailaddress = "test@";

            // Action
            var result = EmailUtils.IsValidEmail(emailaddress);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsValidEmailAddress_ReturnsTrue_WhenValidEmail()
        {
            // Arrange
            var emailaddress = "test@gmail.com";

            // Action
            var result = EmailUtils.IsValidEmailAddress(emailaddress);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsValidEmailAddressWithFullStop_ReturnsTrue_WhenValidEmail()
        {
            // Arrange
            var emailaddress = "test.tesing@gmail.com";

            // Action
            var result = EmailUtils.IsValidEmailAddress(emailaddress);

            // Assert
            Assert.IsTrue(result);
        }

        [DataTestMethod]
        [DataRow("foo-bar@")]
        [DataRow("foo-bar@example.com'")]
        [DataRow("foo-bar@example.com\"")]
        [DataRow("foo-bar@example.com;")]
        [DataRow("foo-bar@example.com)")]
        [DataRow("foo-bar@example.com AND 1=1 -- ")]
        [DataRow("foo-bar@example.com' AND '1'='1' -- ")]
        [DataRow("foo-bar@example.com\" AND \"1\"=\"1\" -- ")]
        [DataRow("foo-bar@example.com AND 1=1")]
        [DataRow("foo-bar@example.com' AND '1'='1")]
        [DataRow("foo-bar@example.com\" AND \"1\"=\"1")]
        [DataRow("foo-bar@example.com UNION ALL select NULL -- ")]
        [DataRow("foo-bar@example.com' UNION ALL select NULL -- ")]
        [DataRow("foo-bar@example.com\" UNION ALL select NULL -- ")]
        [DataRow("foo-bar@example.com) UNION ALL select NULL -- ")]
        [DataRow("foo-bar@example.com') UNION ALL select NULL -- ")]
        [DataRow("${@print(chr(122).chr(97).chr(112).chr(95).chr(116).chr(111).chr(107).chr(101).chr(110))}")]
        [DataRow("${@print(chr(122).chr(97).chr(112).chr(95).chr(116).chr(111).chr(107).chr(101).chr(110))}\"")]
        [DataRow("foo-bar@example.com&cat /etc/passwd&")]
        [DataRow("foo-bar@example.com;cat /etc/passwd;")]
        [DataRow("foo-bar@example.com\"&cat /etc/passwd&\"")]
        [DataRow("foo-bar@example.com\";cat /etc/passwd;\"")]
        [DataRow("foo-bar@example.com'&cat /etc/passwd&'")]
        [DataRow("foo-bar@example.com';cat /etc/passwd;'")]
        [DataRow("foo-bar@example.com&sleep 15&")]
        [DataRow("foo-bar@example.com;sleep 15;")]
        [DataRow("foo-bar@example.com\"&sleep 15&\"")]
        [DataRow("foo-bar@example.com\";sleep 15;\"")]
        [DataRow("foo-bar@example.com'&sleep 15&'")]
        [DataRow("foo-bar@example.com';sleep 15;'")]
        [DataRow("foo-bar@example.com&type %SYSTEMROOT%\\win.ini")]
        [DataRow("foo-bar@example.com|type %SYSTEMROOT%\\win.ini")]
        [DataRow("foo-bar@example.com\"&type %SYSTEMROOT%\\win.ini&\"")]
        [DataRow("foo-bar@example.com\"|type %SYSTEMROOT%\\win.ini")]
        [DataRow("foo-bar@example.com'&type %SYSTEMROOT%\\win.ini&'")]
        [DataRow("foo-bar@example.com'|type %SYSTEMROOT%\\win.ini")]
        [DataRow("foo-bar@example.com&timeout /T 15")]
        [DataRow("foo-bar@example.com|timeout /T 15")]
        [DataRow("foo-bar@example.com\"&timeout /T 15&\"")]
        [DataRow("foo-bar@example.com\"|timeout /T 15")]
        [DataRow("foo-bar@example.com'&timeout /T 15&'")]
        [DataRow("foo-bar@example.com'|timeout /T 15")]
        [DataRow("foo-bar@example.com;get-help")]
        [DataRow("foo-bar@example.com\";get-help")]
        [DataRow("foo-bar@example.com';get-help")]
        [DataRow("foo-bar@example.com;get-help #")]
        [DataRow("foo-bar@example.com;start-sleep -s 15")]
        [DataRow("foo-bar@example.com\";start-sleep -s 15")]
        [DataRow("foo-bar@example.com';start-sleep -s 15")]
        [DataRow("foo-bar@example.com;start-sleep -s 15 #")]
        [DataRow("@")]
        public void IsValidEmailAddressEmail_ReturnsFalse_WhenInValidEmail(string emailaddress)
        {
            // Action
            var result = EmailUtils.IsValidEmailAddress(emailaddress);

            // Assert
            Assert.IsFalse(result);
        }
    }
}

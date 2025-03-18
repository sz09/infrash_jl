using JobLogic.Infrastructure.SMS;
using JobLogic.Infrastructure.UnitTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;

namespace Tests.Unit.Joblogic.Infrastructure.SMS
{
    [TestClass]
    public class SMSNotificationTest : BaseUnitTest
    {
        ISMSNotification _smsNotification;

        [TestInitialize]
        public void Init()
        {
            _smsNotification = new TwilioSMSNotification("AC4348a5f1948208dfb060ab513069a313", "5464e200aecf0c21f84aec9637e0454e", "+15005550006");
        }

        [TestMethod]
        public async Task SendSMS_WithCorrectGeoNumberTo_Success()
        {
            var response = await _smsNotification.SendSMSAsync("+447769759897", "Hello From JobLogic");
            Assert.IsTrue(response.Success);
        }

        [TestMethod]
        public async Task SendSMS_With_WrongGeoTo_Fail()
        {
            var response = await _smsNotification.SendSMSAsync("07855023546", "Hello From JobLogic");
            Assert.IsFalse(response.Success);
            Assert.AreEqual("Error: 21211 - Status: 400 - Message: The 'To' number 07855023546 is not a valid phone number.",
                response.ErrorMessages.First());
        }

        [TestMethod]
        public async Task SendSMS_With_UnavailableNumberFrom_Fail()
        {
            var response = await _smsNotification.SendSMSAsync("+447855023546", "Hello From JobLogic", "+15005550001");
            Assert.IsFalse(response.Success);
            Assert.AreEqual("Error: 21212 - Status: 400 - Message: The 'From' number +15005550001 is not a valid phone number, shortcode, or alphanumeric sender ID.",
                response.ErrorMessages.First());
        }

        [TestMethod]
        public async Task SendSMS_With_InvalidNumberFrom_Fail()
        {
            var response = await _smsNotification.SendSMSAsync("+447855023546", "Hello From JobLogic", "+15005550000");
            Assert.IsFalse(response.Success);
            Assert.AreEqual("Error: 21606 - Status: 400 - Message: The From phone number +15005550000 is not a valid, SMS-capable inbound phone number or short code for your account.",
                response.ErrorMessages.First());
        }
    }
}

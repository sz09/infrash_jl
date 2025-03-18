using JobLogic.Infrastructure.SMS;
using JobLogic.Infrastructure.UnitTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;

namespace Tests.Unit.Joblogic.Infrastructure.SMS
{
    [TestClass]
    public class SMSNotificationWithApiKeyTest : BaseUnitTest
    {
        ISMSNotification _smsNotification;

        [TestInitialize]
        public void Init()
        {
            _smsNotification = new TwilioSMSNotification("ACdfba9413c32de6edeb45e6b180821114", "SK71ddb7d25f32508ea8ff860ba340853b", "RKRGuKZim6b3Atpqd6wpyNMvqCxTAyhG", "+442476101409");
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
            var response = await _smsNotification.SendSMSAsync("078550235426", "Hello From JobLogic");
            Assert.IsFalse(response.Success);
            Assert.AreEqual("Error: 21211 - Status: 400 - Message: The 'To' number 078550235426 is not a valid phone number.",
                response.ErrorMessages.First());
        }

        [TestMethod]
        public async Task SendSMS_With_UnavailableNumberFrom_Fail()
        {
            var response = await _smsNotification.SendSMSAsync("+447855023546", "Hello From JobLogic", "+15005550001");
            Assert.IsFalse(response.Success);
            Assert.AreEqual("Error: 21606 - Status: 400 - Message: The From phone number +15005550001 is not a valid, SMS-capable inbound phone number or short code for your account.",
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

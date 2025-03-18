using JobLogic.Infrastructure.SMS;
using JobLogic.Infrastructure.UnitTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;

namespace Tests.Unit.Joblogic.Infrastructure.SMS
{
    [TestClass]
    public class ValidatePhoneNumberWithApiKeyTest : BaseUnitTest
    {
        ISMSNotification _smsNotification;

        [TestInitialize]
        public void Init()
        {
            _smsNotification = new TwilioSMSNotification("ACdfba9413c32de6edeb45e6b180821114", "SKff799d9687a07544fa62d9d66d57c8d1", "ZOyAoLgwODbwjoblSNblQJQpFqLtSTyE",  "+12133944558");
        }

        [TestMethod]
        public void ValidatePhoneNumber_WithInvalidNumber_Fail()
        {
            var response = _smsNotification.ValidatePhoneNumber("?");
            Assert.IsFalse(response.Success);
        }

        [TestMethod]
        public void ValidatePhoneNumber_WithInvalidNumber_Fail2()
        {
            var response = _smsNotification.ValidatePhoneNumber("111");
            Assert.IsFalse(response.Success);
        }

        // This auth token not available in test account.
        [TestMethod]
        public void ValidatePhoneNumber_WithValidNumber_Success()
        {
            var response = _smsNotification.ValidatePhoneNumber("+4407855023546");
            Assert.IsTrue(response.Success);
        }
    }
}

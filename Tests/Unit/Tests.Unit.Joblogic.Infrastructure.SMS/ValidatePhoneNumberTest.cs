using JobLogic.Infrastructure.SMS;
using JobLogic.Infrastructure.UnitTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;

namespace Tests.Unit.Joblogic.Infrastructure.SMS
{
    [TestClass]
    public class ValidatePhoneNumberTest : BaseUnitTest
    {
        ISMSNotification _smsNotification;

        [TestInitialize]
        public void Init()
        {
            _smsNotification = new TwilioSMSNotification("AC4348a5f1948208dfb060ab513069a313", "5464e200aecf0c21f84aec9637e0454e", "+12133944558");
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
    }
}

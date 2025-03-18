using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using JobLogic.Infrastructure.SMS;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Unit.Joblogic.Infrastructure.SMS
{
    [TestClass]
    public class PhoneNumberUtilsTest
    {

        [DataTestMethod]
        [DataRow("07497087780","GB","", "+447497087780")]
        [DataRow("07497087780","", "United Kingdom", "+447497087780")]
        [DataRow("07497087780","GB", "United Kingdom", "+447497087780")]
        [DataRow("+447497087780","", "United Kingdom", "+447497087780")]
        [DataRow("00447497087780",null, "United Kingdom", "+447497087780")]
        [DataRow("00447497087780", "GB", null, "+447497087780")]
        public async Task FormatE164PhoneNumber_GoodCases_ReturnCorrectFormat(string input, string countryCode, string countryName, string expectedOutput)
        {
            // Action
            var actualOutput = PhoneNumberUtils.FormatE164PhoneNumber(input, countryCode,countryName);
            // Assert
            Assert.AreEqual(expectedOutput, actualOutput);
        }

        [DataTestMethod]
        [DataRow("0749708778", "GB", "", "0749708778")]
        [DataRow("07497087780", null, null, "07497087780")]
        [DataRow("+447497087780", "US", null, "+447497087780")]
        [DataRow("00447497087780", null, "US", "+447497087780")]
        [DataRow("+447497087780", null, "Non-Existing Country", "+447497087780")]
        [DataRow("07497087780", null, "Non-Existing Country", "07497087780")]
        public async Task FormatE164PhoneNumber_BadCases_ReturnOriginalNumber(string input, string countryCode, string countryName, string expectedOutput)
        {
            // Action
            var actualOutput = PhoneNumberUtils.FormatE164PhoneNumber(input, countryCode, countryName);
            // Assert
            Assert.AreEqual(expectedOutput, actualOutput);
        }
    }
}

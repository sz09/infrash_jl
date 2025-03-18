using FluentAssertions;
using JobLogic.Infrastructure.UnitTest;
using JobLogic.Infrastructure.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SqlTypes;

namespace Tests.Unit.Joblogic.Infrastructure.Utilites
{
    [TestClass]
    public class ValidationUtilsTests : BaseUnitTest
    {
        private static string GetRandomStringOfLength(int length) => ValueGenerator.StringWithPattern($"^(.{{{length}}})$");

        #region isMobileNumber

        [TestMethod]
        public void isMobileNumber_With_Required_But_Null_Value_Should_Return_False()
        {
            // Arrange
            // Nothing to arrange

            // Action
            var result = ValidationUtils.isMobileNumber(null, required: true);

            // Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void isMobileNumber_With_Required_But_Empty_Value_Should_Return_False()
        {
            // Arrange
            // Nothing to arrange

            // Action
            var result = ValidationUtils.isMobileNumber(string.Empty, required: true);

            // Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void isMobileNumber_With_NotRequired_But_Null_Value_Should_Return_True()
        {
            // Arrange
            // Nothing to arrange

            // Action
            var result = ValidationUtils.isMobileNumber(null, required: false);

            // Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void isMobileNumber_With_NotRequired_But_Empty_Value_Should_Return_True()
        {
            // Arrange
            // Nothing to arrange

            // Action
            var result = ValidationUtils.isMobileNumber(string.Empty, required: false);

            // Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        [DataRow(100)]
        public void isMobileNumber_With_Length_Exceed_Max_Should_Return_False(int maxLength)
        {
            // Arrange
            var value = GetRandomStringOfLength(maxLength + 1);

            // Action
            var result = ValidationUtils.isMobileNumber(value, maxLength);

            // Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        [DataRow("B9 4AA")]
        [DataRow("My name is bob")]
        public void isMobileNumber_Where_Required_And_Doesnt_Match_Regex_Should_Return_False(string invalidMobileNumber)
        {
            // Arrange
            // Nothing to arrange

            // Action
            var result = ValidationUtils.isMobileNumber(invalidMobileNumber, required: true);

            // Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        [DataRow("+44 7911 123456")]
        [DataRow("+447911123456")]
        [DataRow("+447911 123456")]
        [DataRow("+44 7911123456")]
        public void isMobileNumber_Where_Required_And_Does_Match_Regex_Should_Return_True(string validMobileNumber)
        {
            // Arrange
            // Nothing to arrange

            // Action
            var result = ValidationUtils.isMobileNumber(validMobileNumber, required: true);

            // Assert
            result.Should().BeTrue();
        }

        #endregion

        #region isLandlineNumber

        [TestMethod]
        public void isLandlineNumber_With_Required_But_Null_Value_Should_Return_False()
        {
            // Arrange
            // Nothing to arrange

            // Action
            var result = ValidationUtils.isLandlineNumber(null, required: true);

            // Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void isLandlineNumber_With_Required_But_Empty_Value_Should_Return_False()
        {
            // Arrange
            // Nothing to arrange

            // Action
            var result = ValidationUtils.isLandlineNumber(string.Empty, required: true);

            // Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void isLandlineNumber_With_NotRequired_But_Null_Value_Should_Return_True()
        {
            // Arrange
            // Nothing to arrange

            // Action
            var result = ValidationUtils.isLandlineNumber(null, required: false);

            // Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void isLandlineNumber_With_NotRequired_But_Empty_Value_Should_Return_True()
        {
            // Arrange
            // Nothing to arrange

            // Action
            var result = ValidationUtils.isLandlineNumber(string.Empty, required: false);

            // Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        [DataRow(100)]
        public void isLandlineNumber_With_Length_Exceed_Max_Should_Return_False(int maxLength)
        {
            // Arrange
            var value = GetRandomStringOfLength(maxLength + 1);

            // Action
            var result = ValidationUtils.isLandlineNumber(value, maxLength);

            // Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        [DataRow("B9 4AA")]
        [DataRow("My name is bob")]
        public void isLandlineNumber_Where_Required_And_Doesnt_Match_Regex_Should_Return_False(string invalidLandlineNumber)
        {
            // Arrange
            // Nothing to arrange

            // Action
            var result = ValidationUtils.isLandlineNumber(invalidLandlineNumber, required: true);

            // Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        [DataRow("(020) 1234 5678")]
        [DataRow("(0121) 458 1476")]
        [DataRow("(01865) 12 6785")]
        public void isLandlineNumber_Where_Required_And_Does_Match_Regex_Should_Return_True(string validLandlineNumber)
        {
            // Arrange
            // Nothing to arrange

            // Action
            var result = ValidationUtils.isLandlineNumber(validLandlineNumber, required: true);

            // Assert
            result.Should().BeTrue();
        }

        #endregion

        #region isEmail

        [TestMethod]
        public void isEmail_With_Required_But_Null_Value_Should_Return_False()
        {
            // Arrange
            // Nothing to arrange

            // Action
            var result = ValidationUtils.isEmail(null, required: true);

            // Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void isEmail_With_Required_But_Empty_Value_Should_Return_False()
        {
            // Arrange
            // Nothing to arrange

            // Action
            var result = ValidationUtils.isEmail(string.Empty, required: true);

            // Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void isEmail_With_NotRequired_But_Null_Value_Should_Return_True()
        {
            // Arrange
            // Nothing to arrange

            // Action
            var result = ValidationUtils.isEmail(null, required: false);

            // Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void isEmail_With_NotRequired_But_Empty_Value_Should_Return_True()
        {
            // Arrange
            // Nothing to arrange

            // Action
            var result = ValidationUtils.isEmail(string.Empty, required: false);

            // Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        [DataRow(100)]
        public void isEmail_With_Length_Exceed_Max_Should_Return_False(int maxLength)
        {
            // Arrange
            var value = GetRandomStringOfLength(maxLength + 1);

            // Action
            var result = ValidationUtils.isEmail(value, maxLength);

            // Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        [DataRow("B9 4AA")]
        [DataRow("My name is bob")]
        public void isEmail_Where_Required_And_Doesnt_Match_Regex_Should_Return_False(string invalidEmailAddress)
        {
            // Arrange
            // Nothing to arrange

            // Action
            var result = ValidationUtils.isEmail(invalidEmailAddress, required: true);

            // Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        [DataRow("test@joblogic.com")]
        [DataRow("test.dot@joblogic.com")]
        [DataRow("reallylongemail@joblogic-test.com")]
        [DataRow("nice.dotted.email@joblogic-test.com")]
        public void isEmail_Where_Required_And_Does_Match_Regex_Should_Return_True(string validEmailAddress)
        {
            // Arrange
            // Nothing to arrange

            // Action
            var result = ValidationUtils.isEmail(validEmailAddress, required: true);

            // Assert
            result.Should().BeTrue();
        }

        #endregion

        #region isPostcode

        [TestMethod]
        public void isPostcode_With_Required_But_Null_Value_Should_Return_False()
        {
            // Arrange
            // Nothing to arrange

            // Action
            var result = ValidationUtils.isPostcode(null, required: true);

            // Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void isPostcode_With_Required_But_Empty_Value_Should_Return_False()
        {
            // Arrange
            // Nothing to arrange

            // Action
            var result = ValidationUtils.isPostcode(string.Empty, required: true);

            // Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void isPostcode_With_NotRequired_But_Null_Value_Should_Return_True()
        {
            // Arrange
            // Nothing to arrange

            // Action
            var result = ValidationUtils.isPostcode(null, required: false);

            // Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void isPostcode_With_NotRequired_But_Empty_Value_Should_Return_True()
        {
            // Arrange
            // Nothing to arrange

            // Action
            var result = ValidationUtils.isPostcode(string.Empty, required: false);

            // Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        [DataRow(100)]
        public void isPostcode_With_Length_Exceed_Max_Should_Return_False(int maxLength)
        {
            // Arrange
            var value = GetRandomStringOfLength(maxLength + 1);

            // Action
            var result = ValidationUtils.isPostcode(value, maxLength);

            // Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        [DataRow("test@joblogic.com")]
        [DataRow("My name is bob")]
        public void isPostcode_Where_Required_And_Doesnt_Match_Regex_Should_Return_False(string invalidPostcode)
        {
            // Arrange
            // Nothing to arrange

            // Action
            var result = ValidationUtils.isPostcode(invalidPostcode, required: true);

            // Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        [DataRow("B9 4AA")]
        [DataRow("WD25 8DX")]
        [DataRow("CH65 9JJ")]
        [DataRow("NN12 8TN")]
        public void isPostcode_Where_Required_And_Does_Match_Regex_Should_Return_True(string validPostcode)
        {
            // Arrange
            // Nothing to arrange

            // Action
            var result = ValidationUtils.isPostcode(validPostcode, required: true);

            // Assert
            result.Should().BeTrue();
        }

        #endregion

        #region StripHTML

        [TestMethod]
        public void StripHTML_With_Null_Value_Should_Return_Null()
        {
            // Arrange
            // Nothing to arrange

            // Action
            var result = ValidationUtils.StripHTML(null);

            // Assert
            result.Should().BeNull();
        }

        [TestMethod]
        public void StripHTML_With_Empty_Value_Should_Return_Null()
        {
            // Arrange
            // Nothing to arrange

            // Action
            var result = ValidationUtils.StripHTML(string.Empty);

            // Assert
            result.Should().BeNull();
        }

        [TestMethod]
        public void StripHTML_With_No_HTML_Should_Return_Same_Value()
        {
            // Arrange
            const string value = "Hello there I include no HTML";

            // Action
            var result = ValidationUtils.StripHTML(value);

            // Assert
            result.Should().BeEquivalentTo(value);
        }

        [TestMethod]
        public void StripHTML_With_HTML_Should_Return_With_Removed()
        {
            // Arrange
            const string valueNoHtml = "Hello there I include no HTML";
            const string valueWithHtml = "<p>" + valueNoHtml + "</p>";

            // Action
            var result = ValidationUtils.StripHTML(valueWithHtml);

            // Assert
            result.Should().BeEquivalentTo(valueNoHtml);
        }

        #endregion

        #region isValidDate

        [TestMethod]
        public void isValidDate_With_LessThan_Min_SqlDateTime_Should_Return_False()
        {
            // Arrange
            // Nothing to arrange
            var value = SqlDateTime.MinValue.Value.AddDays(-1);

            // Action
            var result = ValidationUtils.isValidDate(value);

            // Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void isValidDate_With_Equal_Min_SqlDateTime_Should_Return_False()
        {
            // Arrange
            // Nothing to arrange
            var value = SqlDateTime.MinValue.Value;

            // Action
            var result = ValidationUtils.isValidDate(value);

            // Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void isValidDate_With_Equal_Max_SqlDateTime_Should_Return_True()
        {
            // Arrange
            var value = SqlDateTime.MaxValue.Value;

            // Action
            var result = ValidationUtils.isValidDate(value);

            // Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void isValidDate_With_LessThan_Max_SqlDateTime_Should_Return_True()
        {
            // Arrange
            var value = SqlDateTime.MaxValue.Value.AddYears(-1);

            // Action
            var result = ValidationUtils.isValidDate(value);

            // Assert
            result.Should().BeTrue();
        }

        #endregion

        #region isValidDecimal

        [TestMethod]
        public void isValidDecimal_With_Zero_Value_Should_Return_False()
        {
            // Arrange
            // Nothing to arrange

            // Action
            var result = ValidationUtils.isValidDecimal(0);

            // Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void isValidDecimal_With_GreaterThan_Zero_Value_Should_Return_True()
        {
            // Arrange
            // Nothing to arrange

            // Action
            var result = ValidationUtils.isValidDecimal(1);

            // Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void isValidDecimal_With_LessThan_Zero_Value_Should_Return_False()
        {
            // Arrange
            // Nothing to arrange

            // Action
            var result = ValidationUtils.isValidDecimal(-1);

            // Assert
            result.Should().BeFalse();
        }

        #endregion

        #region isValidCost

        [TestMethod]
        public void isValidCost_With_Zero_Value_Should_Return_True()
        {
            // Arrange
            // Nothing to arrange

            // Action
            var result = ValidationUtils.isValidCost(0);

            // Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void isValidCost_With_GreaterThan_Zero_Value_Should_Return_True()
        {
            // Arrange
            // Nothing to arrange

            // Action
            var result = ValidationUtils.isValidCost(1);

            // Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void isValidCost_With_LessThan_Zero_Value_Should_Return_False()
        {
            // Arrange
            // Nothing to arrange

            // Action
            var result = ValidationUtils.isValidCost(-1);

            // Assert
            result.Should().BeFalse();
        }

        #endregion

        #region isTransient

        [TestMethod]
        public void isTransient_With_Null_Value_Should_Return_True()
        {
            // Arrange
            // Nothing to arrange

            // Action
            var result = ValidationUtils.isTransient(null);

            // Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void isTransient_With_Zero_Value_Should_Return_True()
        {
            // Arrange
            // Nothing to arrange

            // Action
            var result = ValidationUtils.isTransient(0);

            // Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void isTransient_With_Non_Zero_Value_Should_Return_False()
        {
            // Arrange
            // Nothing to arrange

            // Action
            var result = ValidationUtils.isTransient(1);

            // Assert
            result.Should().BeFalse();
        }

        #endregion

        #region isValidStringLength

        [TestMethod]
        public void isValidStringLength_With_Null_Value_Should_Return_True()
        {
            // Arrange
            // Nothing to arrange

            // Action
            var result = ValidationUtils.isValidStringLength(null, 0);

            // Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void isValidStringLength_With_Empty_Value_Should_Return_True()
        {
            // Arrange
            // Nothing to arrange

            // Action
            var result = ValidationUtils.isValidStringLength(string.Empty, 0);

            // Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        [DataRow(100)]
        public void isValidStringLength_With_Length_Below_Max_Should_Return_True(int maxLength)
        {
            // Arrange
            var value = GetRandomStringOfLength(maxLength - 1);

            // Action
            var result = ValidationUtils.isValidStringLength(value, maxLength);

            // Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        [DataRow(100)]
        public void isValidStringLength_With_Length_Equal_Max_Should_Return_True(int maxLength)
        {
            // Arrange
            var value = GetRandomStringOfLength(maxLength);

            // Action
            var result = ValidationUtils.isValidStringLength(value, maxLength);

            // Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        [DataRow(100)]
        public void isValidStringLength_With_Length_Exceed_Max_Should_Return_False(int maxLength)
        {
            // Arrange
            var value = GetRandomStringOfLength(maxLength + 1);

            // Action
            var result = ValidationUtils.isValidStringLength(value, maxLength);

            // Assert
            result.Should().BeFalse();
        }

        #endregion
    }
}

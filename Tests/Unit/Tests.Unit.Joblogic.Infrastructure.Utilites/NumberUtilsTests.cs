using FluentAssertions;
using JobLogic.Infrastructure.UnitTest;
using JobLogic.Infrastructure.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Tests.Unit.Joblogic.Infrastructure.Utilites
{
    [TestClass]
    public class NumberUtilsTests : BaseUnitTest
    {
        [DataRow("10.56432", "10.564")]
        [DataRow("3", "3.000")]
        [DataRow("-1", "-1.000")]
        [DataRow("0", "0.000")]
        [DataRow("8.8888888888", "8.889")]
        [DataRow("1.123", "1.123")]
        [TestMethod]
        public void FormatToDecimalPlaces_WithNonNullableDecimal_And_DefaultNumbericScale_Should_Return_3dp_String(string inputValueAsStr, string expectedValue)
        {
            // Arrange
            var inputValue = Convert.ToDecimal(inputValueAsStr);

            // Action
            var result = inputValue.FormatToDecimalPlaces();

            // Assert
            result.Should().Be(expectedValue);
        }

        [DataRow("3", "1", "3.0")]
        [DataRow("3.1", "2", "3.10")]
        [DataRow("3.1234", "2", "3.12")]
        [DataRow("3.1234", "6", "3.123400")]
        [TestMethod]
        public void FormatToDecimalPlaces_WithNonNullableDecimal_And_CustomNumbericScale_Should_Return_Ndp_String(string inputValueAsStr, string decimalPlacesAsStr, string expectedValue)
        {
            // Arrange
            var inputValue = Convert.ToDecimal(inputValueAsStr);
            var decimalPlaces = Convert.ToInt32(decimalPlacesAsStr);

            // Action
            var result = inputValue.FormatToDecimalPlaces(decimalPlaces);

            // Assert
            result.Should().Be(expectedValue);
        }

        [TestMethod]
        public void FormatToDecimalPlaces_WithNullableDecimal_And_Null_Value_Should_Return_EmptyString()
        {
            // Arrange
            decimal? inputValue = null;

            // Action
            var result = inputValue.FormatToDecimalPlaces();

            // Assert
            result.Should().BeEmpty();
        }

        [TestMethod]
        public void FormatToDecimalPlaces_WithNullableDecimal_Should_Return_Correct_Value()
        {
            // Arrange
            decimal? inputValue = 3.123M;

            // Action
            var result = inputValue.FormatToDecimalPlaces(1);

            // Assert
            result.Should().Be("3.1");
        }

        [TestMethod]
        public void FormatToDecimalPlaces_WithNullableDouble_And_Null_Value_Should_Return_EmptyString()
        {
            // Arrange
            double? inputValue = null;

            // Action
            var result = inputValue.FormatToDecimalPlaces();

            // Assert
            result.Should().BeEmpty();
        }

        [DataRow(3, 0, "3")]
        [DataRow(3, 1, "3")]
        [DataRow(3, 2, "03")]
        [DataRow(77, 2, "77")]
        [DataRow(77, 6, "000077")]
        [TestMethod]
        public void PadLeft_WithDefaultCharacter_Should_Return_Correct_Value(int inputValue, int padLength, string expectedValue)
        {
            // Arrange

            // Action
            var result = inputValue.PadLeft(padLength);

            // Assert
            result.Should().Be(expectedValue);
        }

        [DataRow(56, 10, 'j', "jjjjjjjj56")]
        [DataRow(77, 3, 'w', "w77")]
        [DataRow(77, 6, 'a', "aaaa77")]
        [TestMethod]
        public void PadLeft_WithCustomCharacter_Should_Return_Correct_Value(int inputValue, int padLength, char character, string expectedValue)
        {
            // Arrange

            // Action
            var result = inputValue.PadLeft(padLength, character);

            // Assert
            result.Should().Be(expectedValue);
        }

        [TestMethod]
        public void PadLeft_WithEmptyCharacter_Should_Return_Correct_Value()
        {
            // Arrange
            var inputValue = 77;

            // Action
            var result = inputValue.PadLeft(4, ' ');

            // Assert
            result.Should().Be("  77");
        }

        [DataRow(3, 0, "3")]
        [DataRow(3, 1, "3")]
        [DataRow(3, 2, "03")]
        [DataRow(77, 2, "77")]
        [DataRow(77, 6, "000077")]
        [TestMethod]
        public void PadLeft_ForLong_WithDefaultCharacter_Should_Return_Correct_Value(long inputValue, int padLength, string expectedValue)
        {
            // Arrange

            // Action
            var result = inputValue.PadLeft(padLength);

            // Assert
            result.Should().Be(expectedValue);
        }

        [DataRow(56, 10, 'j', "jjjjjjjj56")]
        [DataRow(77, 3, 'w', "w77")]
        [DataRow(77, 6, 'a', "aaaa77")]
        [TestMethod]
        public void PadLeft_ForLong_WithCustomCharacter_Should_Return_Correct_Value(long inputValue, int padLength, char character, string expectedValue)
        {
            // Arrange

            // Action
            var result = inputValue.PadLeft(padLength, character);

            // Assert
            result.Should().Be(expectedValue);
        }

        [TestMethod]
        public void PadLeft_ForLong_WithEmptyCharacter_Should_Return_Correct_Value()
        {
            // Arrange
            long inputValue = 77;

            // Action
            var result = inputValue.PadLeft(4, ' ');

            // Assert
            result.Should().Be("  77");
        }
    }
}

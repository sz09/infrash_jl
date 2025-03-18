using FluentAssertions;
using JobLogic.Infrastructure.UnitTest;
using JobLogic.Infrastructure.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Tests.Unit.Joblogic.Infrastructure.Utilites
{
    [TestClass]
    public class StringUtilsTests : BaseUnitTest
    {
        [TestMethod]
        public void AsDelimiteredStringForCSV_With_Null_IEnumerable_ShouldReturn_EmptyString()
        {
            // Arrange         
            var expectedResult = string.Empty;
            var testArgument = (IEnumerable<string>)null;

            // Act
            var actualResult = StringUtils.AsDelimiteredStringForCSV(testArgument);

            // Assert
            actualResult.Should().BeEquivalentTo(expectedResult);
        }

        [TestMethod]
        public void AsDelimiteredStringForCSV_With_Empty_IEnumerable_ShouldReturn_EmptyString()
        {
            // Arrange         
            var expectedResult = string.Empty;
            var testArgument = new List<string>();

            // Act
            var actualResult = StringUtils.AsDelimiteredStringForCSV(testArgument);

            // Assert
            actualResult.Should().BeEquivalentTo(expectedResult);
        }

        [TestMethod]
        public void AsDelimiteredStringForCSV_With_OneItem_IEnumerable_ShouldReturn_CorrectCSV()
        {
            // Arrange         
            var expectedResult = "\"Final\"";
            var testArgument = new List<string>() { "Final" };

            // Act
            var actualResult = StringUtils.AsDelimiteredStringForCSV(testArgument);

            // Assert
            actualResult.Should().BeEquivalentTo(expectedResult);
        }

        [TestMethod]
        public void AsDelimiteredStringForCSV_With_TwoItem_IEnumerable_ShouldReturn_CorrectCSV()
        {
            // Arrange         
            var expectedResult = "\"Final\",\"Fantasy\"";
            var testArgument = new List<string>() { "Final", "Fantasy" };

            // Act
            var actualResult = StringUtils.AsDelimiteredStringForCSV(testArgument);

            // Assert
            actualResult.Should().BeEquivalentTo(expectedResult);
        }

        [TestMethod]
        public void AsDelimiteredStringForCSV_With_MultipleItem_IEnumerable_ShouldReturn_CorrectCSV()
        {
            // Arrange         
            var expectedResult = "\"Final\",\"Fantasy\",\"VII\",\"VIII\",\"IX\",\"X\",\"XIV\"";
            var testArgument = new List<string>() { "Final", "Fantasy", "VII", "VIII", "IX", "X", "XIV" };

            // Act
            var actualResult = StringUtils.AsDelimiteredStringForCSV(testArgument);

            // Assert
            actualResult.Should().BeEquivalentTo(expectedResult);
        }

        [TestMethod]
        public void AsDelimiteredStringForCSV_Where_StringContains_QuotationMarks_ShouldChange_To_DoubleQuotationMarks()
        {
            // Arrange         
            var expectedResult = "\"Obi-Wan said \"\"Hello There\"\" and General Grievous responded \"\"General Kenobi!\"\"\"";
            var testArgument = new List<string>() { "Obi-Wan said \"Hello There\" and General Grievous responded \"General Kenobi!\"" };

            // Act
            var actualResult = StringUtils.AsDelimiteredStringForCSV(testArgument);

            // Assert
            actualResult.Should().BeEquivalentTo(expectedResult);
        }

        [TestMethod]
        public void AsDelimiteredStringForCSV_With_Null_Array_ShouldReturn_EmptyString()
        {
            // Arrange         
            var expectedResult = string.Empty;
            var testArgument = (string[])null;

            // Act
            var actualResult = StringUtils.AsDelimiteredStringForCSV(testArgument);

            // Assert
            actualResult.Should().BeEquivalentTo(expectedResult);
        }

        [TestMethod]
        public void AsDelimiteredStringForCSV_With_Empty_Array_ShouldReturn_EmptyString()
        {
            // Arrange         
            var expectedResult = string.Empty;
            var testArgument = new string[0];

            // Act
            var actualResult = StringUtils.AsDelimiteredStringForCSV(testArgument);

            // Assert
            actualResult.Should().BeEquivalentTo(expectedResult);
        }
    }
}

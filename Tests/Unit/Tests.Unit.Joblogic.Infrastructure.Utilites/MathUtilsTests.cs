using FluentAssertions;
using JobLogic.Infrastructure.UnitTest;
using JobLogic.Infrastructure.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Tests.Unit.Joblogic.Infrastructure.Utilites
{
    [TestClass]
    public class MathUtilsTests : BaseUnitTest
    {
        [TestMethod]
        public void CalculateSell_With_Positive_CostPerUnit_And_Zero_Uplift_Should_Return_CostPerUnit()
        {
            // Arrange
            const decimal costPerUnit = 77.0M;
            const double uplift = 0D;

            // Action
            var result = MathUtils.CalculateSell(costPerUnit, uplift);

            // Assert
            result.Should().Be(costPerUnit);
        }

        [TestMethod]
        public void CalculateSell_With_Zero_CostPerUnit_And_Positive_Uplift_Should_Return_Zero()
        {
            // Arrange
            const decimal costPerUnit = 0M;
            const double uplift = 10D;

            // Action
            var result = MathUtils.CalculateSell(costPerUnit, uplift);

            // Assert
            result.Should().Be(0M);
        }

        [DataRow("10", "10", "11")]
        [DataRow("100", "10", "110")]
        [DataRow("10.50", "10", "11.55")]
        [DataRow("7777.77", "77", "13766.65")]
        [DataRow("7777.77", "80", "13999.99")]
        [TestMethod]
        public void CalculateSell_With_CostPerUnit_And_Uplift_Should_Return_Correct_Value(string costPerUnitStr, string upliftStr, string expectedResultStr)
        {
            // Arrange
            var costPerUnit = Convert.ToDecimal(costPerUnitStr);
            var uplift = Convert.ToDouble(upliftStr);
            var expectedResult = Convert.ToDecimal(expectedResultStr);

            // Nothing to arrange

            // Action
            var result = MathUtils.CalculateSell(costPerUnit, uplift);

            // Assert
            result.Should().Be(expectedResult);
        }
    }
}

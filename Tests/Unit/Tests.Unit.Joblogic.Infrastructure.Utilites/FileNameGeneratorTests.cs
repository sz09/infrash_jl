using FluentAssertions;
using JobLogic.Infrastructure.UnitTest;
using JobLogic.Infrastructure.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Tests.Unit.Joblogic.Infrastructure.Utilites
{
    [TestClass]
    public class FileNameGeneratorTests : BaseUnitTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Generate_When_All_Arguments_Are_Null_Should_Throw_ArgumentNullException()
        {
            // Arrange
            var fileNameGenerator = new FileNameGenerator();

            // Action
            _ = fileNameGenerator.Generate(string.Empty, string.Empty, string.Empty);

            // Assert
            // Passes based on ExpectedException attribute
        }

        [TestMethod]

        [DataRow("a", null, null, "pdf", "a.pdf")]
        [DataRow("a", "b", null, "pdf", "a - b.pdf")]
        [DataRow("a", null, "c", "pdf", "a - c.pdf")]
        [DataRow(null, "b", "c", "pdf", "b - c.pdf")]
        [DataRow("a", "b", "c", "pdf", "a - b - c.pdf")]
        public void Generate_Should_Return_Correct_Value(string prefixPart, string middlePart, string suffixPart, string fileType, string expectedValue)
        {
            // Arrange
            var fileNameGenerator = new FileNameGenerator();

            // Action
            var result = fileNameGenerator.Generate(prefixPart, middlePart, suffixPart, fileType);

            // Assert
            result.Should().Be(expectedValue);
        }
    }
}

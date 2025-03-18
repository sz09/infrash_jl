using FluentAssertions;
using JobLogic.Infrastructure.UnitTest;
using JobLogic.Infrastructure.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Tests.Unit.Joblogic.Infrastructure.Utilites
{
    [TestClass]
    public class SecurityUtilsTests : BaseUnitTest
    {
        [TestMethod]
        public void EncodedPassword_With_Empty_Password_Should_Return_Empty_String()
        {
            // Arrange
            var timestamp = new DateTime(2021, 1, 1, 7, 30, 15, 0);

            // Action
            var result = SecurityUtils.EncodedPassword(string.Empty, timestamp);

            // Assert
            result.Should().BeEmpty();
        }

        [TestMethod]
        public void EncodedPassword_With_Should_Return_Correct_Hash()
        {
            // Arrange
            const string password = "blob";
            var timestamp = new DateTime(2021, 1, 1, 7, 30, 15, 0);

            // Action
            var result = SecurityUtils.EncodedPassword(password, timestamp);

            // Assert
            result.Should().Be("pstonHdy4w6PiRY3maEwOcE0IPk=");
        }
    }
}

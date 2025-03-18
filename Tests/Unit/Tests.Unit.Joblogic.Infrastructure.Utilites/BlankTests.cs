using FluentAssertions;
using JobLogic.Infrastructure.UnitTest;
using JobLogic.Infrastructure.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Tests.Unit.Joblogic.Infrastructure.Utilites
{
    [TestClass]
    public class BlankTests : BaseUnitTest
    {
        [TestMethod]
        public void ExampleTest()
        {
            // Arrange
            List<int> enumerable = null;

            // Action
            var result = enumerable.IsNullOrEmpty();

            // Assert
            result.Should().BeTrue();
        }
    }
}

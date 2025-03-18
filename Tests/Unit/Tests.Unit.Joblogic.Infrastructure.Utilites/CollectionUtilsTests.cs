using FluentAssertions;
using JobLogic.Infrastructure.UnitTest;
using JobLogic.Infrastructure.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Tests.Unit.Joblogic.Infrastructure.Utilites
{
    [TestClass]
    public class CollectionUtilsTests : BaseUnitTest
    {
        [TestMethod]
        public void IsNullOrEmpty_With_NullList_ShouldReturnTrue()
        {
            // Arrange
            List<int> enumerable = null;

            // Action
            var result = enumerable.IsNullOrEmpty();

            // Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void IsNullOrEmpty_With_NullArray_ShouldReturnTrue()
        {
            // Arrange
            int[] enumerable = null;

            // Action
            var result = enumerable.IsNullOrEmpty();

            // Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void IsNullOrEmpty_With_EmptyList_ShouldReturnTrue()
        {
            // Arrange
            var enumerable = new List<int>();

            // Action
            var result = enumerable.IsNullOrEmpty();

            // Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void IsNullOrEmpty_With_EmptyArray_ShouldReturnTrue()
        {
            // Arrange
            var enumerable = new int[0];

            // Action
            var result = enumerable.IsNullOrEmpty();

            // Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void IsNullOrEmpty_With_OneItemList_ShouldReturnFalse()
        {
            // Arrange
            var enumerable = new List<int>() { 77 };

            // Action
            var result = enumerable.IsNullOrEmpty();

            // Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void IsNullOrEmpty_With_OneItemArray_ShouldReturnFalse()
        {
            // Arrange
            var enumerable = new int[] { 77 };

            // Action
            var result = enumerable.IsNullOrEmpty();

            // Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void IsNullOrEmpty_With_MultipleItemList_ShouldReturnFalse()
        {
            // Arrange
            var enumerable = new List<int>() { 7, 77, 777, 7777, 77777, 777777, 7777777 };

            // Action
            var result = enumerable.IsNullOrEmpty();

            // Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void IsNullOrEmpty_With_MultipleItemArray_ShouldReturnFalse()
        {
            // Arrange
            var enumerable = new int[] { 7, 77, 777, 7777, 77777, 777777, 7777777 };

            // Action
            var result = enumerable.IsNullOrEmpty();

            // Assert
            result.Should().BeFalse();
        }
    }
}

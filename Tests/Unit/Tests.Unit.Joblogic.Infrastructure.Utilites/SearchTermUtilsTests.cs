using FluentAssertions;
using JobLogic.Infrastructure.UnitTest;
using JobLogic.Infrastructure.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Tests.Unit.Joblogic.Infrastructure.Utilites
{
    [TestClass]
    public class SearchTermUtilsTests : BaseUnitTest
    {
        [TestMethod]
        public void Build_With_Null_SearchTerm_Should_Return_List_Of_String()
        {
            // Arrange
            // Nothing to arrange

            // Action
            var result = SearchTermUtils.Instance.Build(null);

            // Assert
            result.Should().BeOfType<List<string>>();
        }

        [TestMethod]
        public void Build_With_Null_SearchTerm_Should_Return_EmptyList()
        {
            // Arrange
            // Nothing to arrange

            // Action
            var result = SearchTermUtils.Instance.Build(null);

            // Assert
            result.Should().BeEmpty();
        }

        [TestMethod]
        public void Build_With_Empty_SearchTerm_Should_Return_EmptyList()
        {
            // Arrange
            // Nothing to arrange

            // Action
            var result = SearchTermUtils.Instance.Build(string.Empty);

            // Assert
            result.Should().BeEmpty();
        }

        [DataRow("item1", 1)]
        [DataRow("item1 item2 item3", 3)]
        [DataRow("item1 item2 item3 item4 item5 item6 item7", 7)]
        [TestMethod]
        public void Build_With_SearchTerm_And_SearchExact_Should_Return_Correct_Item_Count(string searchTerm, int expectedCount)
        {
            // Arrange
            // Nothing to arrange

            // Action
            var result = SearchTermUtils.Instance.Build(searchTerm);

            // Assert
            result.Should().HaveCount(expectedCount);
        }
    }
}

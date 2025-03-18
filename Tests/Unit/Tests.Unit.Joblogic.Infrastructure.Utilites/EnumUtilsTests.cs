using FluentAssertions;
using JobLogic.Infrastructure.UnitTest;
using JobLogic.Infrastructure.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Unit.Joblogic.Infrastructure.Utilites
{
    [TestClass]
    public class EnumUtilsTests : BaseUnitTest
    {
        [TestMethod]
        public void GetDescription_Pass_Null_Should_Return_Empty_String()
        {
            // Arrange

            // Action
            var result = EnumUtils.GetDescription(null);

            // Assert
            result.Should().Be(string.Empty);
        }

        [TestMethod]
        public void GetDescription_Pass_Not_Existing_Enum_Should_Return_Empty_String()
        {
            // Arrange

            // Action
            var result = EnumUtils.GetDescription((EnumUtilsTests_FakeEnum)77);

            // Assert
            result.Should().Be(string.Empty);
        }

        [TestMethod]
        public void GetDescription_Pass_Enum_With_Description_Should_Return_Description()
        {
            // Arrange

            // Action
            var result = EnumUtils.GetDescription(EnumUtilsTests_FakeEnum.Item1);

            // Assert
            result.Should().Be("Item 1 - Description");
        }

        [TestMethod]
        public void GetDescription_Pass_Enum_No_Description_Should_Return_Name()
        {
            // Arrange

            // Action
            var result = EnumUtils.GetDescription(EnumUtilsTests_FakeEnum.NoDescription);

            // Assert
            result.Should().Be("NoDescription");
        }


        [TestMethod]
        public void GetDisplayName_Pass_Null_Should_Return_Empty_String()
        {
            // Arrange

            // Action
            var result = EnumUtils.GetDisplayName(null);

            // Assert
            result.Should().Be(string.Empty);
        }

        [TestMethod]
        public void GetDisplayName_Pass_Enum_With_DisplayName_Should_Return_DisplayName()
        {
            // Arrange

            // Action
            var result = EnumUtils.GetDisplayName(EnumUtilsTests_FakeEnum.Item1);

            // Assert
            result.Should().Be("Item 1 - Display Name");
        }

        [TestMethod]
        public void GetDisplayName_Pass_Enum_No_DisplayName_Should_Return_Name()
        {
            // Arrange

            // Action
            var result = EnumUtils.GetDisplayName(EnumUtilsTests_FakeEnum.NoDescription);

            // Assert
            result.Should().Be("NoDescription");
        }


        [TestMethod]
        public void TryParseEnum_Pass_Null_Should_Return_Null()
        {
            // Arrange

            // Action
            var result = EnumUtils.TryParseEnum<EnumUtilsTests_FakeEnum>(null);

            // Assert
            result.Should().Be(null);
        }

        [TestMethod]
        public void TryParseEnum_Pass_Enum_Should_Parse_Return_Correct_Value()
        {
            // Arrange

            // Action
            var result = EnumUtils.TryParseEnum<EnumUtilsTests_FakeEnum>("Item3");

            // Assert
            result.Should().Be(EnumUtilsTests_FakeEnum.Item3);
        }


        [TestMethod]
        public void IsOneOf_With_Null_Params_Should_Return_False()
        {
            // Action
            var result = EnumUtilsTests_FakeEnum.Item1.IsOneOf(null);

            // Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void IsOneOf_With_Empty_Params_Should_Return_False()
        {
            // Action
            var result = EnumUtilsTests_FakeEnum.Item1.IsOneOf(new EnumUtilsTests_FakeEnum[] { });

            // Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void IsOneOf_When_Not_Matching_Should_Return_False()
        {
            // Action
            var result = EnumUtilsTests_FakeEnum.Item1.IsOneOf(
                EnumUtilsTests_FakeEnum.Item2,
                EnumUtilsTests_FakeEnum.Item3,
                EnumUtilsTests_FakeEnum.NoDescription);

            // Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void IsOneOf_When_Matching_Should_Return_True()
        {
            // Action
            var result = EnumUtilsTests_FakeEnum.Item1.IsOneOf(
                EnumUtilsTests_FakeEnum.Item1,
                EnumUtilsTests_FakeEnum.Item2,
                EnumUtilsTests_FakeEnum.Item3,
                EnumUtilsTests_FakeEnum.NoDescription);

            // Assert
            result.Should().BeTrue();
        }
    }
}

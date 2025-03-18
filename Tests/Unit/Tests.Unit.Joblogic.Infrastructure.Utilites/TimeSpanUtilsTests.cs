using FluentAssertions;
using JobLogic.Infrastructure.UnitTest;
using JobLogic.Infrastructure.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Unit.Joblogic.Infrastructure.Utilites
{
    [TestClass]
    public class TimeSpanUtilsTests : BaseUnitTest
    {
        [TestMethod]
        [DataRow(0, 0, 0)]
        [DataRow(15, 0, 15)]
        [DataRow(59, 0, 59)]
        [DataRow(60, 1, 0)]
        [DataRow(90, 1, 30)]
        [DataRow(420, 7, 0)]
        public void ConvertToTimeSpanWithHours_Should_Return_Correct_Values(int totalMinutes, int expectedTotalHours, int expectedMinutes)
        {
            // Action
            var result = TimeSpanUtils.ConvertToTimeSpanWithHours(totalMinutes);

            // Assert
            result.Should().BeEquivalentTo(new TimeSpanWithHours_JobLogic
            {
                TotalHours = expectedTotalHours,
                Minutes = expectedMinutes
            });
        }

        [TestMethod]
        [DataRow(0, 0, 0, 0)]
        [DataRow(15, 0, 0, 15)]
        [DataRow(59, 0, 0, 59)]
        [DataRow(60, 0, 1, 0)]
        [DataRow(90, 0, 1, 30)]
        [DataRow(420, 0, 7, 0)]
        [DataRow(1530, 1, 1, 30)]
        [DataRow(3065, 2, 3, 5)]
        public void ConvertToTimeSpan_Should_Return_Correct_Values(int totalMinutes, int expectedDays, int expectedTotalHours, int expectedMinutes)
        {
            // Action
            var result = TimeSpanUtils.ConvertToTimeSpan(totalMinutes);

            // Assert
            result.Should().BeEquivalentTo(new TimeSpan_JobLogic
            {
                TotalDays = expectedDays,
                TotalHours = expectedTotalHours,
                TotalMinutes = expectedMinutes
            });
        }

        [TestMethod]
        [DataRow(0, 0, 0, 0)]
        [DataRow(0, 0, 77, 77)]
        [DataRow(0, 77, 77, 4697)]
        [DataRow(77, 77, 77, 115577)]
        public void GetTotalMinutesFromTimeSpan_Should_Return_Correct_Values(int totalDays, int totalHours, int totalMinutes, int expectedMinutes)
        {
            // Action
            var result = TimeSpanUtils.GetTotalMinutesFromTimeSpan(totalDays, totalHours, totalMinutes);

            // Assert
            result.Should().Be(expectedMinutes);
        }

        [TestMethod]
        [DataRow(0, "0d 0h 0m")]
        [DataRow(59, "0d 0h 59m ")]
        [DataRow(60, "0d 1h 0m ")]
        [DataRow(1439, "0d 23h 59m ")]
        [DataRow(1440, "1d 0h 0m ")]
        [DataRow(1530, "1d 1h 30m ")]
        public void GetTimeSpanFormatted_Should_Return_Correct_Values(int totalMinutes, string expectedTimeSpan)
        {
            // Action
            var result = TimeSpanUtils.GetTimeSpanFormatted(totalMinutes);

            // Assert
            result.Should().Be(expectedTimeSpan);
        }

        [TestMethod]
        public void GetTimeSpanFormatted_If_Pass_0_Has_No_Whitespace_At_End()
        {
            // Action
            var result = TimeSpanUtils.GetTimeSpanFormatted(0);

            // Assert

            var lastCharacter = StringUtils.Right(result, 1);

            lastCharacter.Should().Be("m");
        }

        [TestMethod]
        public void GetTimeSpanFormatted_If_Pass_GreaterThan_0_Has_Whitespace_At_End()
        {
            // Action
            for (int i = 10; i < 50; i += 10)
            {
                var result = TimeSpanUtils.GetTimeSpanFormatted(i);

                // Assert
                var lastCharacter = StringUtils.Right(result, 1);

                lastCharacter.Should().Be(" ");
            }
        }
    }
}

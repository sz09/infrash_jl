using FluentAssertions;
using JobLogic.Infrastructure.UnitTest;
using JobLogic.Infrastructure.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Tests.Unit.Joblogic.Infrastructure.Utilites
{
    [TestClass]
    public class DateUtilsTest : BaseUnitTest
    {
        [TestMethod]
        public void GetBeginningOfDay_With_DateTime_TodaysDate_Should_Return_Correct_Value()
        {
            // Arrange
            var date = DateTime.Now;
            var expectedValue = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);

            // Action
            var result = date.GetBeginningOfDay();

            // Assert
            result.Should().Be(expectedValue);
        }

        [TestMethod]
        public void GetBeginningOfDay_With_DateTimeOffset_TodaysDate_Should_Return_Correct_Value()
        {
            // Arrange
            var date = DateTimeOffset.Now;
            var expectedValue = new DateTimeOffset(date.Year, date.Month, date.Day, 0, 0, 0, date.Offset);

            // Action
            var result = date.GetBeginningOfDay();

            // Assert
            result.Should().Be(expectedValue);
        }

        [TestMethod]
        public void GetBeginningOfDay_With_Nullable_DateTime_Null_Should_Return_Null()
        {
            // Arrange
            DateTime? date = null;

            // Action
            var result = date.GetBeginningOfDay();

            // Assert
            result.Should().BeNull();
        }

        [TestMethod]
        public void GetBeginningOfDay_With_Nullable_DateTime_TodaysDate_Should_Return_Correct_Value()
        {
            // Arrange
            DateTime? date = DateTime.Now;
            var expectedValue = new DateTime(date.Value.Year, date.Value.Month, date.Value.Day, 0, 0, 0);

            // Action
            var result = date.GetBeginningOfDay();

            // Assert
            result.Should().Be(expectedValue);
        }

        [TestMethod]
        public void GetEndOfDay_With_DateTime_TodaysDate_Should_Return_Correct_Value()
        {
            // Arrange
            var date = DateTime.Now;
            var expectedValue = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);

            // Action
            var result = date.GetEndOfDay();

            // Assert
            result.Should().Be(expectedValue);
        }

        [TestMethod]
        public void GetEndOfDay_With_Nullable_DateTime_Null_Should_Return_Null()
        {
            // Arrange
            DateTime? date = null;

            // Action
            var result = date.GetEndOfDay();

            // Assert
            result.Should().BeNull();
        }

        [TestMethod]
        public void GetEndOfDay_With_Nullable_DateTime_TodaysDate_Should_Return_Correct_Value()
        {
            // Arrange
            DateTime? date = DateTime.Now;
            var expectedValue = new DateTime(date.Value.Year, date.Value.Month, date.Value.Day, 23, 59, 59);

            // Action
            var result = date.GetEndOfDay();

            // Assert
            result.Should().Be(expectedValue);
        }

        [TestMethod]
        public void GetFirstDayOfYear_With_DateTime_TodaysDate_Should_Return_Correct_Value()
        {
            // Arrange
            var date = DateTime.Now;
            var expectedValue = new DateTime(date.Year, 1, 1);

            // Action
            var result = date.GetFirstDayOfYear();

            // Assert
            result.Should().Be(expectedValue);
        }

        [TestMethod]
        public void GetFirstDayOfYear_With_Nullable_DateTime_Null_Should_Return_Null()
        {
            // Arrange
            DateTime? date = null;

            // Action
            var result = date.GetFirstDayOfYear();

            // Assert
            result.Should().BeNull();
        }

        [TestMethod]
        public void GetFirstDayOfYear_With_Nullable_DateTime_TodaysDate_Should_Return_Correct_Value()
        {
            // Arrange
            DateTime? date = DateTime.Now;
            var expectedValue = new DateTime(date.Value.Year, 1, 1);

            // Action
            var result = date.GetFirstDayOfYear();

            // Assert
            result.Should().Be(expectedValue);
        }

        [TestMethod]
        public void GetLastDayOfYear_With_DateTime_TodaysDate_Should_Return_Correct_Value()
        {
            // Arrange
            var date = DateTime.Now;
            var expectedValue = new DateTime(date.Year, 12, 31);

            // Action
            var result = date.GetLastDayOfYear();

            // Assert
            result.Should().Be(expectedValue);
        }

        [TestMethod]
        public void GetLastDayOfYear_With_Nullable_DateTime_Null_Should_Return_Null()
        {
            // Arrange
            DateTime? date = null;

            // Action
            var result = date.GetLastDayOfYear();

            // Assert
            result.Should().BeNull();
        }

        [TestMethod]
        public void GetLastDayOfYear_With_Nullable_DateTime_TodaysDate_Should_Return_Correct_Value()
        {
            // Arrange
            DateTime? date = DateTime.Now;
            var expectedValue = new DateTime(date.Value.Year, 12, 31);

            // Action
            var result = date.GetLastDayOfYear();

            // Assert
            result.Should().Be(expectedValue);
        }

        [TestMethod]
        public void GetFirstDayOfMonth_With_DateTime_TodaysDate_Should_Return_Correct_Value()
        {
            // Arrange
            var date = DateTime.Now;
            var expectedValue = new DateTime(date.Year, date.Month, 1);

            // Action
            var result = date.GetFirstDayOfMonth();

            // Assert
            result.Should().Be(expectedValue);
        }

        [TestMethod]
        public void GetFirstDayOfMonth_With_Nullable_DateTime_Null_Should_Return_Null()
        {
            // Arrange
            DateTime? date = null;

            // Action
            var result = date.GetFirstDayOfMonth();

            // Assert
            result.Should().BeNull();
        }

        [TestMethod]
        public void GetFirstDayOfMonth_With_Nullable_DateTime_TodaysDate_Should_Return_Correct_Value()
        {
            // Arrange
            DateTime? date = DateTime.Now;
            var expectedValue = new DateTime(date.Value.Year, date.Value.Month, 1);

            // Action
            var result = date.GetFirstDayOfMonth();

            // Assert
            result.Should().Be(expectedValue);
        }

        [TestMethod]
        [DataRow(2020, 12, 31)]
        [DataRow(2021, 02, 28)]
        [DataRow(2020, 02, 29)] // 2020 = Leap year in UK
        public void GetLastDayOfMonth_With_DateTime_TodaysDate_Should_Return_Correct_Value(int year, int month, int expectedDay)
        {
            // Arrange
            var date = new DateTime(year, month, 1);
            var expectedValue = new DateTime(year, month, expectedDay);

            // Action
            var result = date.GetLastDayOfMonth();

            // Assert
            result.Should().Be(expectedValue);
        }

        [TestMethod]
        public void GetLastDayOfMonth_With_Nullable_DateTime_Null_Should_Return_Null()
        {
            // Arrange
            DateTime? date = null;

            // Action
            var result = date.GetLastDayOfMonth();

            // Assert
            result.Should().BeNull();
        }

        [TestMethod]
        [DataRow(2020, 12, 31)]
        [DataRow(2021, 02, 28)]
        [DataRow(2020, 02, 29)] // 2020 = Leap year in UK
        public void GetLastDayOfMonth_With_Nullable_DateTime_TodaysDate_Should_Return_Correct_Value(int year, int month, int expectedDay)
        {
            // Arrange
            DateTime? date = new DateTime(year, month, 1);
            var expectedValue = new DateTime(year, month, expectedDay);

            // Action
            var result = date.GetLastDayOfMonth();

            // Assert
            result.Should().Be(expectedValue);
        }

        //JH 16/12/2020: LocalDateToUTCDate function is strange and doesn't seem to care about the timezone you pass in, or the timezone of the date either
        //[TestMethod]
        //public void LocalDateToUTCDate_With_GMT_Should_Return_Same_As_UTC()
        //{
        //    // Arrange
        //    var date = new DateTime(2020, 3, 25, 15, 0, 0);
        //    var expectedValue = new DateTime(2020, 3, 25, 15, 0, 0);

        //    // Action
        //    var result = date.LocalDateToUTCDate("GMT Standard Time");

        //    // Assert
        //    result.Should().Be(expectedValue);
        //}

        //[TestMethod]
        //public void LocalDateToUTCDate_With_BST_Should_Deduct_One_Hour()
        //{
        //    // Arrange
        //    var date = new DateTime(2020, 4, 25, 15, 0, 0);
        //    var expectedValue = new DateTime(2020, 4, 25, 14, 0, 0);

        //    // Action
        //    var result = date.LocalDateToUTCDate("GMT Standard Time");

        //    // Assert
        //    result.Should().Be(expectedValue);
        //}

        //[TestMethod]
        //public void LocalDateToUTCDate_With_UtcPlus12_Should_Deduct_Twelve_Hours()
        //{
        //    // Arrange
        //    var expectedValue = new DateTime(2020, 4, 25, 3, 0, 0);
        //    var date = TimeZoneInfo.ConvertTimeFromUtc(expectedValue, TimeZoneInfo.FindSystemTimeZoneById("UTC+12"));

        //    // Action
        //    var result = date.LocalDateToUTCDate("UTC+12");

        //    // Assert
        //    result.Should().Be(expectedValue);
        //}

        [TestMethod]
        public void LocalDateToUTCDate_With_Nullable_DateTime_Null_Should_Return_Null()
        {
            // Arrange
            DateTime? date = null;

            // Action
            var result = date.LocalDateToUTCDate("GMT Standard Time");

            // Assert
            result.Should().BeNull();
        }

        [TestMethod]
        public void ConvertTimeZones_TimeZonesFromLive_ReturnsDatetime()
        {
            // Arrange
            var timeZoneList = TimeZonesInLive();
            var destinationTimeZone = "Pakistan Standard Time";
            var nonDSTDate = new DateTime(2019, 03, 01, 8, 0, 0);

            foreach (var zone in timeZoneList)
            {
                // Action
                var result = nonDSTDate.ConvertTimeZones(zone, destinationTimeZone);

                // Assert
                result.Should<DateTime>().BeOfType(typeof(DateTime));
            }
        }

        [TestMethod]
        public void ConvertTimeZones_ValidTimeZoneAndNonDST_ReturnsDatetime()
        {
            // Arrange
            var timeZone = "GMT Standard Time";
            var destinationTimeZone = "Pakistan Standard Time";
            var nonDSTDate = new DateTime(2019, 03, 01, 8, 0, 0);

            // Action            
            var result = nonDSTDate.ConvertTimeZones(timeZone, destinationTimeZone);

            // Assert
            result.Should<DateTime>().BeOfType(typeof(DateTime));
            result.Hour.Should().Be(13);
        }

        [TestMethod]
        public void ConvertTimeZones_ValidTimeZoneAndDST_ReturnsDatetime()
        {
            // Arrange
            var timeZone = "GMT Standard Time";
            var destinationTimeZone = "Pakistan Standard Time";
            var dSTDate = new DateTime(2019, 04, 01, 8, 0, 0);

            // Action            
            var result = dSTDate.ConvertTimeZones(timeZone, destinationTimeZone);

            // Assert
            result.Should<DateTime>().BeOfType(typeof(DateTime));
            result.Hour.Should().Be(12);
        }

        [TestMethod]
        [ExpectedException(typeof(TimeZoneNotFoundException))]
        public void ConvertTimeZones_InvalidTimeZone_ReturnsException()
        {
            // Arrange
            var timeZone = "XYZ Standard Time";
            var destinationTimeZone = "Pakistan Standard Time";
            var dateTime = new DateTime(2019, 03, 01, 8, 0, 0);

            // Action            
            var result = dateTime.ConvertTimeZones(timeZone, destinationTimeZone);
        }

        private List<string> TimeZonesInLive()
        {
            return new List<string>
            {
                "Afghanistan Standard Time",
                "Alaskan Standard Time",
                "Arab Standard Time",
                "Arabian Standard Time",
                "Arabic Standard Time",
                "Astrakhan Standard Time",
                "Atlantic Standard Time",
                "AUS Eastern Standard Time",
                "Azerbaijan Standard Time",
                "Bangladesh Standard Time",
                "Belarus Standard Time",
                "Bougainville Standard Time",
                "Cen. Australia Standard Time",
                "Central America Standard Time",
                "Central Europe Standard Time",
                "Central European Standard Time",
                "Central Standard Time (Mexico)",
                "Central Standard Time",
                "China Standard Time",
                "Dateline Standard Time",
                "E. Africa Standard Time",
                "E. Australia Standard Time",
                "E. Europe Standard Time",
                "E. South America Standard Time",
                "Eastern Standard Time",
                "Egypt Standard Time",
                "Fiji Standard Time",
                "FLE Standard Time",
                "GMT Standard Time",
                "Greenwich Standard Time",
                "GTB Standard Time",
                "Hawaiian Standard Time",
                "India Standard Time",
                "Iran Standard Time",
                "Israel Standard Time",
                "Jordan Standard Time",
                "Korea Standard Time",
                "Mauritius Standard Time",
                "Middle East Standard Time",
                "Morocco Standard Time",
                "Mountain Standard Time",
                "Myanmar Standard Time",
                "Namibia Standard Time",
                "Nepal Standard Time",
                "New Zealand Standard Time",
                "Pacific Standard Time (Mexico)",
                "Pacific Standard Time",
                "Pakistan Standard Time",
                "Romance Standard Time",
                "Russian Standard Time",
                "SA Pacific Standard Time",
                "SA Western Standard Time",
                "SE Asia Standard Time",
                "Singapore Standard Time",
                "South Africa Standard Time",
                "Sri Lanka Standard Time",
                "Taipei Standard Time",
                "Tasmania Standard Time",
                "Turkey Standard Time",
                "Ulaanbaatar Standard Time",
                "US Eastern Standard Time",
                "US Mountain Standard Time",
                "UTC",
                "Venezuela Standard Time",
                "W. Australia Standard Time",
                "W. Central Africa Standard Time",
                "W. Europe Standard Time",
                "West Asia Standard Time",
                "West Pacific Standard Time",
            };
        }

        [TestMethod]
        public void Parse_With_Valid_Date_And_Default_Format_Should_Return_Correct_Value()
        {
            // Arrange
            var dateStr = "14/04/1912";
            var expectedValue = new DateTime(1912, 4, 14);

            // Action
            var result = dateStr.Parse();

            // Assert
            result.Should().Be(expectedValue);
        }

        [TestMethod]
        public void Parse_With_Valid_Date_And_Custom_Format_Should_Return_Correct_Value()
        {
            // Arrange
            var dateStr = "14-04-1912";
            var expectedValue = new DateTime(1912, 4, 14);

            // Action
            var result = dateStr.Parse(format: "dd-MM-yyyy");

            // Assert
            result.Should().Be(expectedValue);
        }

        [TestMethod]
        public void Parse_With_Valid_DateTime_And_Custom_Format_Should_Return_Correct_Value()
        {
            // Arrange
            var dateStr = "14-04-1912 02:02";
            var expectedValue = new DateTime(1912, 4, 14, 2, 2, 0);

            // Action
            var result = dateStr.Parse(format: "dd-MM-yyyy HH:mm");

            // Assert
            result.Should().Be(expectedValue);
        }

        [DataRow("16/12/2020", -7, "28/10/2020")]
        [DataRow("16/12/2020", -1, "09/12/2020")]
        [DataRow("16/12/2020", 0, "16/12/2020")]
        [DataRow("16/12/2020", 1, "23/12/2020")]
        [DataRow("16/12/2020", 7, "03/02/2021")]
        [DataRow("01/01/2021", 52, "31/12/2021")]
        [DataRow("29/02/2020", 1, "07/03/2020")] // Next three cover leap years
        [DataRow("29/02/2020", -1, "22/02/2020")]
        [DataRow("29/02/2020", 52, "27/02/2021")]
        [TestMethod]
        public void AddWeeks_Should_Return_Correct_Value(string startDateAsStr, int numberOfWeeksToAdd, string expectedDateAsStr)
        {
            // Arrange
            var date = startDateAsStr.Parse();
            var expectedValue = expectedDateAsStr.Parse();

            // Action
            var result = date.AddWeeks(numberOfWeeksToAdd);

            // Assert
            result.Should().Be(expectedValue);
        }
    }
}

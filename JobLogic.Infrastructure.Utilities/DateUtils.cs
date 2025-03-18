using System;
using System.Globalization;

namespace JobLogic.Infrastructure.Utilities
{
    public enum RollOnDateUnits
    {
        Day,
        Week,
        Month
    }

    public enum CalenderWeekRules
    {
        FirstDay = 0,
        FirstFullWeek = 1,
        FirstFourDayWeek = 2
    }

    public static class DateUtils
    {
        public const int MINUTE_PER_HOUR = 60;
        public const int MINUTE_PER_DAY = 24 * MINUTE_PER_HOUR;

        public static DateTime? ObjectToNullableDateTime(object o)
        {
            if (o == null) return null;

            DateTime dt;
            if (DateTime.TryParse(o.ToString(), out dt)) return dt;
            else
                return null;
        }

        public static DateTime GetBeginningOfDay(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
        }
        public static DateTimeOffset GetBeginningOfDay(this DateTimeOffset date)
        {
            return new DateTimeOffset(date.Year, date.Month, date.Day, 0, 0, 0, date.Offset);
        }
        public static DateTime? GetBeginningOfDay(this DateTime? date)
        {
            if (!date.HasValue) return null;

            return GetBeginningOfDay(date.Value);
        }

        public static DateTime GetEndOfDay(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
        }

        public static DateTime? GetEndOfDay(this DateTime? date)
        {
            if (!date.HasValue) return null;

            return GetEndOfDay(date.Value);
        }

        public static DateTime GetFirstDayOfYear(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, 1, 1);
        }

        public static DateTime? GetFirstDayOfYear(this DateTime? dateTime)
        {
            if (!dateTime.HasValue) return null;
            return GetFirstDayOfYear(dateTime.Value);
        }

        public static DateTime GetLastDayOfYear(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, 12, 31);
        }

        public static DateTime? GetLastDayOfYear(this DateTime? dateTime)
        {
            if (!dateTime.HasValue) return null;
            return GetLastDayOfYear(dateTime.Value);
        }

        public static DateTime GetFirstDayOfMonth(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, 1);
        }

        public static DateTime? GetFirstDayOfMonth(this DateTime? dateTime)
        {
            if (!dateTime.HasValue) return null;
            return GetFirstDayOfMonth(dateTime.Value);
        }

        public static DateTime GetLastDayOfMonth(this DateTime dateTime)
        {
            DateTime firstDayOfTheMonth = GetFirstDayOfMonth(dateTime);
            return firstDayOfTheMonth.AddMonths(1).AddDays(-1);
        }

        public static DateTime? GetLastDayOfMonth(this DateTime? dateTime)
        {
            if (!dateTime.HasValue) return null;
            return GetLastDayOfMonth(dateTime.Value);
        }

        public static DateTime? LocalDateToUTCDate(this DateTime? localDateTime, string timeZoneId)
        {
            if (!localDateTime.HasValue) return localDateTime;
            return LocalDateToUTCDate(localDateTime.Value, timeZoneId);
        }
        public static DateTime LocalDateToUTCDate(this DateTime localDateTime, string timeZoneId)
        {
            // Convert local to value base on the timezone setting of company
            var valueInTimezone = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(localDateTime, timeZoneId);

            // Convert the time zone value to utc
            return TimeZoneInfo.ConvertTimeToUtc(localDateTime);
        }

        public static DateTime UTCDateToLocalDateTime(this DateTime utcDateTime, string timeZoneId)
        {
            var localDateTime = TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, TimeZoneInfo.FindSystemTimeZoneById(timeZoneId));
            return localDateTime;
        }

        public static DateTime ConvertTimeZones(this DateTime dateTime, string sourceZoneId, string destZoneId)
        {
            try
            {
                TimeZoneInfo sourceZone = TimeZoneInfo.FindSystemTimeZoneById(sourceZoneId);
                TimeZoneInfo destZone = TimeZoneInfo.FindSystemTimeZoneById(destZoneId);

                return TimeZoneInfo.ConvertTime(dateTime, sourceZone, destZone);
            }
            catch (TimeZoneNotFoundException notfoundEx)
            {
                throw notfoundEx;
            }
            catch (InvalidTimeZoneException invalidEx)
            {
                throw invalidEx;
            }
        }

        public static DateTime Parse(this string dateValue, string format = "dd/MM/yyyy")
        {
            return DateTime.ParseExact(dateValue, format, CultureInfo.InvariantCulture);
        }

        public static DateTime AddWeeks(this DateTime dateTime, int value)
        {
            // .NET doesn't provide DateTime.AddWeeks(), so we multiply value by 7 and add it using AddDays()
            return dateTime.AddDays(value * 7);
        }

        public static DateTime? FromEpochTimeStampToUtc(this string unixTimeStamp)
        {
            double timestamp;
            if (!double.TryParse(unixTimeStamp, out timestamp))
                return null;

            var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(timestamp).ToUniversalTime();

            return dateTime;

        }

        public static DateTime RollOnDateBy(this DateTime dateTime, int value, RollOnDateUnits unit)
        {
            switch (unit)
            {
                case RollOnDateUnits.Week:
                    return dateTime.AddWeeks(value);
                case RollOnDateUnits.Month:
                    return dateTime.AddMonths(value);
                case RollOnDateUnits.Day:
                default:
                    return dateTime.AddDays(value);
            }
        }

        public static int DayToMinutes(this DayOfWeek dayOfWeek, int hour, int minute, bool isSundayLast = true)
        {
            return ((isSundayLast && dayOfWeek == DayOfWeek.Sunday)
                        ? ((int)DayOfWeek.Saturday + 1)
                        : (int)dayOfWeek) * MINUTE_PER_DAY +
                    TimeToMinutes(hour, minute);
        }

        public static int TimeToMinutes(int hour, int minute)
        {
            return hour * MINUTE_PER_HOUR +
                   minute;
        }

        public static int GetIso8601WeekOfYear(this DateTime dateTime)
        {
            DayOfWeek yDay = dateTime.DayOfWeek;
            var date = dateTime;
            if (yDay >= DayOfWeek.Monday && yDay <= DayOfWeek.Wednesday)
            {
                date = date.AddDays(3);
            }
            return GetWeekOfYear(date, DayOfWeek.Monday);
        }

		public static int GetWeekOfYear(this DateTime dateTime, DayOfWeek firstDayOfWeek, CalenderWeekRules rule = CalenderWeekRules.FirstFourDayWeek)
		{
            int result = 0;
            var firstDayOfYear = new DateTime(dateTime.Year, 1, 1);
            var systemDate = default(DateTime);
            int day = 0;
            int week = 0;
            switch (rule)
            {
                case CalenderWeekRules.FirstDay:
                    for (day = 0; day <= 6; day++)
                    {
                        systemDate = firstDayOfYear.AddDays(day);
                        if (systemDate.DayOfWeek == firstDayOfWeek)
                        {
                            break;
                        }
                    }

                    if (firstDayOfYear.DayOfWeek == firstDayOfWeek)
                    {
                        result = (int)Math.Floor(((double)dateTime.DayOfYear - (double)systemDate.DayOfYear) / 7D) + 1;
                    }
                    else
                    {
                        result = (int)Math.Floor(((double)dateTime.DayOfYear - (double)systemDate.DayOfYear) / 7D) + 2;
                    }
                    break;
                case CalenderWeekRules.FirstFullWeek:
                    for (day = 0; day <= 6; day++)
                    {
                        systemDate = firstDayOfYear.AddDays(day);
                        if (systemDate.DayOfWeek == firstDayOfWeek)
                        {
                            break;
                        }
                    }

                    week = (int)Math.Floor(((double)dateTime.DayOfYear - (double)systemDate.DayOfYear) / 7D) + 1;
                    if (week == 0)
                    {
                        result = GetWeekOfYear(new DateTime(dateTime.Year - 1, 12, 31), firstDayOfWeek, CalenderWeekRules.FirstFullWeek);
                    }
                    else
                    {
                        result = week;
                    }
                    break;
                case CalenderWeekRules.FirstFourDayWeek:
                    for (day = 0; day <= 12; day++)
                    {
                        systemDate = firstDayOfYear.AddDays(day);
                        if (systemDate.DayOfWeek == firstDayOfWeek & systemDate.DayOfYear >= 5)
                        {
                            break;
                        }
                    }

                    week = (int)Math.Floor(((double)dateTime.DayOfYear - (double)systemDate.DayOfYear) / 7D) + 2;
                    if (week == 0)
                    {
                        result = GetWeekOfYear(new DateTime(dateTime.Year - 1, 12, 31), firstDayOfWeek, CalenderWeekRules.FirstFourDayWeek);
                    }
                    else
                    {
                        result = week;
                    }
                    break;
            }
            return result;
        }

        /// <summary>
        /// Calculate whole days difference between two dates
        /// </summary>
        /// <param name="flipNegative">By deafult, will return negative days if startDate has passed endDate. Setting this to true will make it positive.</param>
        /// <returns>Number of whole days between two dates</returns>
        public static int GetDaysDifferenceFrom(this DateTime startDate, DateTime endDate, bool flipNegative = false)
        {
            var daysDifference = (int)Math.Ceiling((endDate.GetBeginningOfDay() - startDate.GetBeginningOfDay()).TotalDays);

            if (flipNegative)
                daysDifference = Math.Abs(daysDifference);

            return daysDifference;
        }

        /// <summary>
        /// ISO8601 is standard and common way to get week of year
        /// Calendar rule is FirstFourDayWeek
        /// Recommend use this function to get correct week.
        /// </summary>
        /// <param name="dayOfWeek">First day of week</param>
        /// <param name="cultureInfo">Culture info, by default if null will get from current thread</param>
        /// <returns>Week number And Year Of Week</returns>
        public static Tuple<int, int> GetIso8601WeekAndYear(this DateTime dateTime, DayOfWeek dayOfWeek, CultureInfo cultureInfo)
        {
            if (cultureInfo == null)
                cultureInfo = CultureInfo.InvariantCulture;

            DayOfWeek day = cultureInfo.Calendar.GetDayOfWeek(dateTime);
            var date = dateTime;
            int dayAdd = 0;

            //The purpose to make sure always get the day of week is center of Week
            //Example dayOfWeek is Monday => Thursday is day center of week. 
            switch (dayOfWeek)
            {
                case DayOfWeek.Monday:
                default:
                    dayAdd = (day == DayOfWeek.Monday || day == DayOfWeek.Tuesday || day == DayOfWeek.Wednesday) ? 3 : 0;
                    break;
                case DayOfWeek.Tuesday:
                    dayAdd = (day == DayOfWeek.Tuesday || day == DayOfWeek.Wednesday || day == DayOfWeek.Thursday) ? 3 : 0;
                    break;
                case DayOfWeek.Wednesday:
                    dayAdd = (day == DayOfWeek.Wednesday || day == DayOfWeek.Thursday || day == DayOfWeek.Friday) ? 3 : 0;
                    break;
                case DayOfWeek.Thursday:
                    dayAdd = (day == DayOfWeek.Thursday || day == DayOfWeek.Friday || day == DayOfWeek.Saturday) ? 3 : 0;
                    break;
                case DayOfWeek.Friday:
                    dayAdd = (day == DayOfWeek.Friday || day == DayOfWeek.Saturday || day == DayOfWeek.Sunday) ? 3 : 0;
                    break;
                case DayOfWeek.Saturday:
                    dayAdd = (day == DayOfWeek.Saturday || day == DayOfWeek.Sunday || day == DayOfWeek.Monday) ? 3 : 0;
                    break;
                case DayOfWeek.Sunday:
                    dayAdd = (day == DayOfWeek.Sunday || day == DayOfWeek.Monday || day == DayOfWeek.Tuesday) ? 3 : 0;
                    break;
            }
            date = date.AddDays(dayAdd);

            var week = cultureInfo.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, dayOfWeek);
            var year = dateTime.Year;
            var month = dateTime.Month;
            if (week == 1 && month == 12)
                year++;

            if ((week == 52 || week == 53) && month == 1)
                year--;

            return Tuple.Create(week, year);
        }


        /// <summary>
        /// Get First date of week 
        /// Calendar rule is FirstFourDayWeek
        /// </summary>
        /// <returns>First Date of Week And Year</returns>
        [Obsolete("Should use ISOWeek.ToDateTime instead")]
        public static DateTime FirstDateOfWeekISO8601(int year, int weekOfYear, DayOfWeek dayOfWeek)
        {
            DateTime jan1 = new DateTime(year, 1, 1);

            var firstDay = DayOfWeek.Thursday;

            //The purpose to make sure always get the day of week is center of Week
            //Example dayOfWeek is Monday => Thursday is day center of week.
            switch (dayOfWeek)
            {
                case DayOfWeek.Monday:
                default:
                    firstDay = DayOfWeek.Thursday;
                    break;
                case DayOfWeek.Tuesday:
                    firstDay = DayOfWeek.Friday;
                    break;
                case DayOfWeek.Wednesday:
                    firstDay = DayOfWeek.Saturday;
                    break;
                case DayOfWeek.Thursday:
                    firstDay = DayOfWeek.Sunday;
                    break;
                case DayOfWeek.Friday:
                    firstDay = DayOfWeek.Monday;
                    break;
                case DayOfWeek.Saturday:
                    firstDay = DayOfWeek.Tuesday;
                    break;
                case DayOfWeek.Sunday:
                    firstDay = DayOfWeek.Wednesday;
                    break;
            }
            int daysOffset = firstDay - jan1.DayOfWeek;


            // Use first Thursday in January to get first week of the year as
            // it will never be in Week 52/53
            DateTime firstThursday = jan1.AddDays(daysOffset);
            var cal = CultureInfo.CurrentCulture.Calendar;
            int firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, dayOfWeek);

            var weekNum = weekOfYear;
            // As we're adding days to a date in Week 1,
            // we need to subtract 1 in order to get the right date for week #1
            if (firstWeek == 1)
            {
                weekNum -= 1;
            }

            // Using the first Thursday as starting week ensures that we are starting in the right year
            // then we add number of weeks multiplied with days
            var result = firstThursday.AddDays(weekNum * 7);

            // Subtract 3 days from Thursday to get Monday, which is the first weekday in ISO8601
            return result.AddDays(-3);
        }

        public static DateTime GetFirstDayOfWeek(this DateTime date, DayOfWeek firstDayOfWeek)
        {
            var startDate = date;
            for (var i = 0; i < 7; i++)
            {
                startDate = date.AddDays(-1 * i);
                if (startDate.DayOfWeek == firstDayOfWeek) break;
            }

            return startDate;
        }

        public static DateTime GetLastDayOfWeek(this DateTime date, DayOfWeek firstDayOfWeek)
        {
            var startDate = GetFirstDayOfWeek(date, firstDayOfWeek);
            return startDate.AddDays(6);
        }

        private static string GMTTimezone => "GMT Standard Time";
        private static readonly object thisLock = new object();
        private static TimeZoneInfo _gmtTimezoneInfo = null;
        private static TimeZoneInfo GMTTimezoneInfo
        {
            get
            {
                if (_gmtTimezoneInfo == null)
                {
                    lock (thisLock)
                    {
                        if (_gmtTimezoneInfo == null)
                            _gmtTimezoneInfo = TimeZoneInfo.FindSystemTimeZoneById(GMTTimezone);
                    }
                }
                return _gmtTimezoneInfo;
            }
        }

        public static DateTime? UTCDateToGMTDateTime(this DateTime? utcDateTime)
        {
            if (!utcDateTime.HasValue) return null;
            return utcDateTime.Value.UTCDateToGMTDateTime();
        }

        public static DateTime UTCDateToGMTDateTime(this DateTime utcDateTime)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, GMTTimezoneInfo);
        }
    }
}

using JobLogic.Infrastructure.Utilities;
using System;
using System.Globalization;

namespace JobLogic.Infrastructure.CultureHelper
{
    //http://eonasdan.github.io/bootstrap-datetimepicker/Options/
    //http://momentjs.com/docs/#/displaying/format/
    //https://msdn.microsoft.com/en-us/library/8kb3ddd4(v=vs.110).aspx

    public class CultureHelper
    {
        private readonly string _timeZoneName;
        private readonly string _dateTimeFormat;
        private readonly string _dateFormat;
        private readonly string _timeFormat;
        private readonly string _currencyCode;

        public CultureHelper(string timeZoneName, string dateFormat, string timeFormat, string currencyCode, string cultureName, string dateTimeFormat = null)
        {
            _timeZoneName = timeZoneName;
            _dateTimeFormat = dateTimeFormat ?? $"{dateFormat} {timeFormat}";
            _dateFormat = dateFormat;
            _timeFormat = timeFormat;
            _currencyCode = currencyCode;
            CultureName = cultureName;
        }

        #region Properties
        public string CultureName { get; }
        #endregion

        #region Currency
        public string GetCurrencySymbol()
        {
            return CurrencyHelper.GetSymbol(_currencyCode);
        }
        public string FormatWithCurrencySign(decimal? value)
        {
            if (!value.HasValue) return GetCurrencySymbol();
            return FormatWithCurrencySign(value.Value);

        }
        public string FormatWithCurrencySign(decimal value)
        {
            if (value < 0)
            {
                return string.Format("-{0}{1}", GetCurrencySymbol(), FormatToTwoDecimalPlaces(value * (-1)));
            }

            return string.Format("{0}{1}", GetCurrencySymbol(), FormatToTwoDecimalPlaces(value));
        }

        public decimal GetValueWithoutCurrencySign(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return 0;
            }

            value = value.Replace(GetCurrencySymbol(), "");
            return DataConverter.Instance.To<decimal>(value);
        }
        #endregion

        #region 2dp
        public static string FormatToTwoDecimalPlaces(decimal? value, bool forInput = false)
        {
            if (!value.HasValue) return string.Empty;
            return FormatToTwoDecimalPlaces(value.Value, forInput);
        }
        public static string FormatToTwoDecimalPlaces(decimal value, bool forInput = false)
        {
            if (forInput) return string.Format("{0:0.00}", value);
            return string.Format("{0:#,0.00}", value);
        }
        public static string FormatToTwoDecimalPlaces(double? value, bool forInput = false)
        {
            if (!value.HasValue) return string.Empty;
            return FormatToTwoDecimalPlaces(value.Value, forInput);
        }
        public static string FormatToTwoDecimalPlaces(double value, bool forInput = false)
        {
            if (forInput) 
                return string.Format("{0:0.00}", value);
            return string.Format("{0:#,0.00}", value);
        }
        #endregion

        #region 3dp
        public static string FormatToThreeDecimalPlaces(decimal? value)
        {
            if (!value.HasValue) return string.Empty;
            return FormatToThreeDecimalPlaces(value.Value);
        }
        public static string FormatToThreeDecimalPlaces(decimal value)
        {
            return string.Format("{0:0.000}", value);
        }
        
        public static string FormatToThreeDecimalPlaces(double? value)
        {
            if (!value.HasValue) return string.Empty;
            return FormatToThreeDecimalPlaces(value.Value);
        }
        public static string FormatToThreeDecimalPlaces(double value)
        {
            return string.Format("{0:0.000}", value);
        }
        #endregion

        #region Percentage
        public static string FormatWithPercentageSign(double value)
        {
            return string.Format("{0}{1}", FormatToTwoDecimalPlaces(value), "%");
        }
        public static string FormatWithPercentageSign(double? value)
        {
            if (!value.HasValue) return string.Empty;
            return FormatWithPercentageSign((double)value);
        }
        public static string FormatWithPercentageSign(decimal value)
        {
            return FormatWithPercentageSign((double)value);
        }
        public static string FormatWithPercentageSign(decimal? value)
        {
            if (!value.HasValue) return string.Empty;
            return FormatWithPercentageSign((double)value);
        }
        #endregion

        #region Dates
        public static string FormatToHoursAndMinutes(TimeSpan timespan)
        {
            return string.Format("{0}h {1}m", (int)timespan.TotalHours, timespan.Minutes);
        }

        public string FormatDateTimeToString(DateTime dateTime, bool dateOnly = false)
        {
            var format = dateOnly ? _dateFormat : _dateTimeFormat;
            return dateTime.ToString(format, CultureInfo.InvariantCulture);
        }
        #region UTC To Local
        public DateTime GetCurrentLocalDateTime()
        {
            return UTCDateToLocalDateTime(DateTime.UtcNow);
        }
        public string UTCDateToLocalString(DateTime? utcDateTime, bool dateOnly = false)
        {
            if (!utcDateTime.HasValue) return string.Empty;
            return UTCDateToLocalString(utcDateTime.Value, dateOnly);
        }
        public string UTCDateToLocalString(DateTime utcDateTime, bool dateOnly = false)
        {
            var format = dateOnly ? _dateFormat : _dateTimeFormat;
            var localDateTime = UTCDateToLocalDateTime(utcDateTime);
            return localDateTime.ToString(format, CultureInfo.InvariantCulture);
        }
        public string UTCDateToLocalLongTimeString(DateTime? utcDateTime)
        {
            if (!utcDateTime.HasValue) return string.Empty;
            var format = _dateTimeFormat.Replace(":mm", ":mm:ss");
            var localDateTime = UTCDateToLocalDateTime(utcDateTime.Value);
            return localDateTime.ToString(format, CultureInfo.InvariantCulture);
        }
        public string UTCDateToLocalTimeOnlyString(DateTime? utcDateTime)
        {
            if (!utcDateTime.HasValue) return string.Empty;
            var format = _timeFormat;
            var localDateTime = UTCDateToLocalDateTime(utcDateTime.Value);
            return localDateTime.ToString(format, CultureInfo.InvariantCulture);
        }
        public DateTime UTCDateToLocalDateTime(DateTime utcDateTime)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, TimeZoneInfo.FindSystemTimeZoneById(_timeZoneName));
        }
        public DateTime? UTCDateToLocalDateTime(DateTime? utcDateTime)
        {
            if (!utcDateTime.HasValue) return null;
            return UTCDateToLocalDateTime(utcDateTime.Value);
        }
        public static long UTCDateToUnixTimeSeconds(DateTime utcDateTime)
        {
            var epochTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return (long)(utcDateTime - epochTime).TotalSeconds;
        }
        public string UTCDateToLocalJavascriptDateString(DateTime utcDateTime)
        {
            var format = "yyyy-MM-ddTHH:mm:ss";
            var localDateTime = UTCDateToLocalDateTime(utcDateTime);
            return localDateTime.ToString(format, CultureInfo.InvariantCulture);
        }
        public static string DateToLocalJavascriptDateString(DateTime utcDateTime)
        {
            var format = "yyyy-MM-ddTHH:mm:ss";
            return utcDateTime.ToString(format, CultureInfo.InvariantCulture);
        }
        #endregion

        public enum DateTimePart
        {
            None = 0,
            Start = 1,
            End = 2
        }

        public enum ParseErrorFallback
        {
            Throw = 0,
            NullData = 1
        }

        #region Local To UTC
        public DateTime? LocalStringToNullableUTCDateTime(string localString, bool dateOnly = false, DateTimePart dateTimePart = DateTimePart.None, ParseErrorFallback action = ParseErrorFallback.Throw)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(localString)) return null;
                return LocalStringToUTCDateTime(localString, dateOnly, dateTimePart);
            }
            catch (Exception)
            {
                if (action == ParseErrorFallback.Throw) throw;
                else return null;
            }
        }

        public DateTime LocalStringToUTCDateTime(string localString, bool dateOnly = false, DateTimePart dateTimePart = DateTimePart.None)
        {
            var dateTimeParsed = LocalStringToLocalDateTime(localString, dateOnly, dateTimePart);
            return LocalDateToUTCDateTime(dateTimeParsed);
        }

        public DateTime? LocalStringToNullableLocalDateTime(string localString, bool dateOnly = false, DateTimePart dateTimePart = DateTimePart.None, ParseErrorFallback action = ParseErrorFallback.Throw)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(localString)) return null;
                return LocalStringToLocalDateTime(localString, dateOnly, dateTimePart);
            }
            catch (Exception)
            {
                if (action == ParseErrorFallback.Throw) throw;
                else return null;
            }
        }

        public DateTime LocalStringToLocalDateTime(string localString, bool dateOnly = false, DateTimePart dateTimePart = DateTimePart.None)
        {
            var dateTimeParsed = dateStringParsed(localString, dateOnly);

            switch (dateTimePart)
            {
                case DateTimePart.Start:
                    dateTimeParsed = dateTimeParsed.GetBeginningOfDay();
                    break;
                case DateTimePart.End:
                    dateTimeParsed = dateTimeParsed.GetEndOfDay();
                    break;
            }

            return dateTimeParsed;
        }

        public DateTime LocalDateToUTCDateTime(DateTime inputDate, string localTimeZoneId = null)
        {
            var timeZoneName = !string.IsNullOrWhiteSpace(localTimeZoneId) ? localTimeZoneId : _timeZoneName;
            if (inputDate.Kind == DateTimeKind.Local)
            {
                return TimeZoneInfo.ConvertTimeToUtc(inputDate, TimeZoneInfo.Local);
            }

            return TimeZoneInfo.ConvertTimeToUtc(inputDate, TimeZoneInfo.FindSystemTimeZoneById(timeZoneName));
        }

        public bool IsValidDateTime(string localString, bool dateOnly = false)
        {
            var format = dateOnly ? _dateFormat : _dateTimeFormat;
            return DateTime.TryParseExact(localString, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result);
        }

        private DateTime dateStringParsed(string dateString, bool dateOnly = false)
        {
            var format = dateOnly ? _dateFormat : _dateTimeFormat;
            var dateTimeParsed = DateTime.ParseExact(dateString, format, CultureInfo.InvariantCulture);
            return dateTimeParsed;
        }

        #endregion

        #region Convert from saved system preference
        public string SavedPreferenceToLocalString(string savedString, string savedFormat, bool dateOnly = false)
        {
            if (string.IsNullOrWhiteSpace(savedString) || string.IsNullOrWhiteSpace(savedFormat)) return null;

            try
            {
                var dateTimeParsed = DateTime.ParseExact(savedString, savedFormat, CultureInfo.InvariantCulture);
                return UTCDateToLocalString(dateTimeParsed, dateOnly);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        public enum DateTimeType
        {
            DateOnly = 1,
            TimeOnly = 2,
            DateAndTime = 3
        }

        /// <summary>
        /// Get datetimeType by string input
        /// </summary>
        /// <param name="input"></param>
        /// <logic>
        ///     Invoice				    =>	  Date & Time
        ///     Invoice Date            =>	  Date Only
        ///     Invoice Time            =>	  Time Only
        ///     Invoice Date / Time	    =>    Date & Time
        ///     InvoiceDateTime		    =>	  Date & Time
        ///     Invoice_Date & Time     =>	  Date & Time
        ///     Invoice_Date			=>	  Date Only
        /// </logic>
        public DateTimeType GetDateTimeType(string input)
        {
            var hasDate = input.IndexOf("date", StringComparison.OrdinalIgnoreCase) >= 0;
            var hasTime = input.IndexOf("time", StringComparison.OrdinalIgnoreCase) >= 0;

            if ((hasDate && hasTime) || (!hasDate && !hasTime))
            {
                return DateTimeType.DateAndTime;
            }
            else if (hasDate)
            {
                return DateTimeType.DateOnly;
            }
            else
            {
                return DateTimeType.TimeOnly;
            }
        }

        public string UTCDateTimeToLocalString(string colName, DateTime? utcDateTime)
        {
            if (!utcDateTime.HasValue)
                return string.Empty;

            switch (GetDateTimeType(colName))
            {
                case DateTimeType.TimeOnly:
                    return UTCDateToLocalTimeOnlyString(utcDateTime);
                case DateTimeType.DateOnly:
                    return UTCDateToLocalString(utcDateTime.Value, true);
                case DateTimeType.DateAndTime:
                    return UTCDateToLocalString(utcDateTime.Value, false);
            }

            return string.Empty;
        }
        #endregion
    }
}

namespace JobLogic.Infrastructure.Utilities
{
    public static class NumberUtils
    {
        public static string FormatToDecimalPlaces(this decimal value, int numbericScale = 3)
        {
            return ((double)value).FormatToDecimalPlaces(numbericScale);
        }
        public static string FormatToDecimalPlaces(this decimal? value, int numbericScale = 3)
        {
            if (!value.HasValue) return string.Empty;
            return ((double)value).FormatToDecimalPlaces(numbericScale);
        }
        public static string FormatToDecimalPlaces(this double? value, int numbericScale = 3)
        {
            if (!value.HasValue) return string.Empty;
            return ((double)value).FormatToDecimalPlaces(numbericScale);
        }
        public static string FormatToDecimalPlaces(this double value, int numbericScale = 3)
        {
            return string.Format(string.Format("{{0:#,0.{0}}}", new string('0', numbericScale)), value);
        }
        public static string PadLeft(this int value, int length, char character = '0')
        {
            return value.ToString().PadLeft(length, character);
        }
        public static string PadLeft(this long value, int length, char character = '0')
        {
            return value.ToString().PadLeft(length, character);
        }

        public static bool IsEqual<T>(this T value, T number) => value.Equals(number);

        /// <summary>
        /// Calculate what 'number' is as a percentage of 'asPercentageOf' (10 is 10% of 100)
        /// </summary>
        /// <param name="number">Calculate this number (e.g. 10)</param>
        /// <param name="asPercentageOf">As a percentage of this number (e.g. 100)</param>
        /// <returns>10 is 10% of 100</returns>
        public static double CalculateAsPercentageOf(int number, int asPercentageOf)
        {
            if (asPercentageOf <= 0) return 0;

            var difference = (double)number / asPercentageOf;
            return MathUtils.RoundValue(difference * 100);
        }
    }
}

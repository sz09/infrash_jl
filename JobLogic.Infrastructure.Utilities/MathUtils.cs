using System;

namespace JobLogic.Infrastructure.Utilities
{
    public static class MathUtils
    {
        public static decimal CalculateSell(decimal costPerUnit, double uplift)
        {
            return RoundValue(costPerUnit + (costPerUnit * ((decimal)uplift / 100)));
        }

        public static decimal? PennyConverter(int? value)
        {
            if (!value.HasValue) return value;
            return PennyConverter(value.Value);
        }

        public static decimal? PennyConverter(long? value)
        {
            return PennyConverter((int?)value);
        }

        public static decimal PennyConverter(long value)
        {
            return PennyConverter((int)value);
        }

        public static decimal PennyConverter(int pence)
        {
            return Convert.ToDecimal(pence / 100.0);
        }

        public static decimal? RoundValue(decimal? value, int digits = 2)
        {
            if (!value.HasValue) return value;
            return Math.Round(value.Value, digits, MidpointRounding.AwayFromZero);
        }

        public static double? RoundValue(double? value, int digits = 2)
        {
            if (!value.HasValue) return value;
            return Math.Round(value.Value, digits, MidpointRounding.AwayFromZero);
        }

        public static decimal RoundValue(decimal value, int digits = 2)
        {
            return Math.Round(value, digits, MidpointRounding.AwayFromZero);
        }

        public static double RoundValue(double value, int digits = 2)
        {
            return Math.Round(value, digits, MidpointRounding.AwayFromZero);
        }

        public static int RoundToWholeMinutes(DateTime? startDate, DateTime? endDate)
        {
            if (!startDate.HasValue || !endDate.HasValue) return 0;

            var newTimespan = endDate.Value.Subtract(startDate.Value);
            return (int)RoundValue(newTimespan.TotalMinutes, 0);
        }

        public static decimal GetPercentageOf(this decimal value, double percentage)
        {
            return ((decimal)percentage / 100) * value;
        }
    }
}

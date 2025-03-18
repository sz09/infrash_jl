using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobLogic.Infrastructure.Utilities
{
    public static  class UKHolidayCalculator
    {
        public static bool IsJanuaryBankHolidayMonday(DateTime currentDateTime)
        {
            // January bank holiday falls on the first working day after New Year's day,
            // which is usually January 1st itself.
            DateTime newYearsDay = new DateTime(currentDateTime.Year, 01, 01);
            DateTime bankHoliday = newYearsDay;
            while (IsWorkingDay(bankHoliday.DayOfWeek) == false)
            {
                bankHoliday = bankHoliday.AddDays(1);
            }

            return currentDateTime.Date == bankHoliday.Date;
        }

        public static bool IsEasterBankHolidayMonday(DateTime currentDateTime)
        {
            // Easter bank holiday is always the Monday after Easter Sunday.
            DateTime easterSunday = GetEasterSunday(currentDateTime.Year);
            return easterSunday.AddDays(1).Date == currentDateTime.Date;
        }

        public static bool IsEasterGoodFriday(DateTime currentDateTime)
        {
            // Easter bank holiday is always the Monday after Easter Sunday.
            DateTime easterSunday = GetEasterSunday(currentDateTime.Year);
            return easterSunday.AddDays(-2).Date == currentDateTime.Date;
        }


        public static DateTime GetEasterSunday(int year)
        {
            // From http://stackoverflow.com/a/2510411/21574
            int day = 0;
            int month = 0;

            int g = year % 19;
            int c = year / 100;
            int h = (c - (int)(c / 4) - (int)((8 * c + 13) / 25) + 19 * g + 15) % 30;
            int i = h - (int)(h / 28) * (1 - (int)(h / 28) * (int)(29 / (h + 1)) * (int)((21 - g) / 11));

            day = i - ((year + (int)(year / 4) + i + 2 - c + (int)(c / 4)) % 7) + 28;
            month = 3;

            if (day > 31)
            {
                month++;
                day -= 31;
            }

            return new DateTime(year, month, day);
        }

        public static bool IsMayBankHolidayMonday(DateTime currentDateTime)
        {
            // The first Monday of May is a bank holiday (May day)
            DateTime firstMayBankHoliday = new DateTime(currentDateTime.Year, 05, 01);
            while (firstMayBankHoliday.DayOfWeek != DayOfWeek.Monday)
            {
                firstMayBankHoliday = firstMayBankHoliday.AddDays(1);
            }

            if (currentDateTime.Date == firstMayBankHoliday.Date)
                return true;

            // The last Monday of May is a bank holiday (Spring bank holiday)
            DateTime lastMayBankHoliday = new DateTime(currentDateTime.Year, 05, 31);
            while (lastMayBankHoliday.DayOfWeek != DayOfWeek.Monday)
            {
                lastMayBankHoliday = lastMayBankHoliday.AddDays(-1);
            }

            if (currentDateTime.Date == lastMayBankHoliday.Date)
                return true;

            return false;
        }

        public static bool IsAugustBankHolidayMonday(DateTime currentDateTime)
        {
            // The last Monday of August is a bank holiday
            DateTime augustBankHoliday = new DateTime(currentDateTime.Year, 08, 31);
            while (augustBankHoliday.DayOfWeek != DayOfWeek.Monday)
            {
                augustBankHoliday = augustBankHoliday.AddDays(-1);
            }

            if (currentDateTime.Date == augustBankHoliday.Date)
                return true;
            else
                return false;
        }

        public static bool IsDecemberBankHoliday(DateTime currentDateTime)
        {
            // December's bank holiday is the first working day on, or after Boxing day.
            DateTime boxingDay = new DateTime(currentDateTime.Year, 12, 26);
            DateTime bankHoliday = boxingDay;

            //Replacement for Christmas 
            if (boxingDay.DayOfWeek == DayOfWeek.Monday)
                bankHoliday = boxingDay.AddDays(1);

            while (IsWorkingDay(bankHoliday.DayOfWeek) == false)
            {
                bankHoliday = bankHoliday.AddDays(1);
            }

            if (currentDateTime.Date == bankHoliday.Date)
                return true;
            else
                return false;
        }

        public static bool IsWorkingDay(DayOfWeek dayOfWeek)
        {
            if (dayOfWeek == DayOfWeek.Saturday || dayOfWeek == DayOfWeek.Sunday)
                return false;

            return true;
        }

        public static bool IsHoliday(DateTime currentDate)
        {
            var isHoliday = false;
            switch (currentDate.Month)
            {
                case 1:
                    isHoliday = IsJanuaryBankHolidayMonday(currentDate);
                    break;
                case 5:
                    isHoliday = IsMayBankHolidayMonday(currentDate);
                    break;
                case 8:
                    isHoliday = IsAugustBankHolidayMonday(currentDate);
                    break;
                case 12:
                    isHoliday = IsDecemberBankHoliday(currentDate);
                    break;
                case 4:
                    if (currentDate.DayOfWeek == DayOfWeek.Friday)
                        isHoliday = IsEasterGoodFriday(currentDate);
                    if (currentDate.DayOfWeek == DayOfWeek.Monday)
                        isHoliday = IsEasterBankHolidayMonday(currentDate);
                    break;
                default:
                    break;
            }
            return isHoliday;
        }

        public static List<DateTime> GetAllBankHolidayInYear(int year)
        {
            var result = new List<DateTime>();

            // New Year 
            DateTime newYearsDay = new DateTime(year, 01, 01);
            DateTime replacementNyHoliday = newYearsDay;
            while (IsWorkingDay(replacementNyHoliday.DayOfWeek) == false)
            {
                replacementNyHoliday = replacementNyHoliday.AddDays(1);
            }
            result.Add(replacementNyHoliday);

            // APR

            DateTime easterSunday = GetEasterSunday(year);
            result.Add(easterSunday.AddDays(1));
            result.Add(easterSunday.AddDays(-2));



            // May days
            // The first Monday of May is a bank holiday (May day)
            DateTime firstMayBankHoliday = new DateTime(year, 05, 01);
            while (firstMayBankHoliday.DayOfWeek != DayOfWeek.Monday)
            {
                firstMayBankHoliday = firstMayBankHoliday.AddDays(1);
            }
            result.Add(firstMayBankHoliday);

            // The last Monday of May is a bank holiday (Spring bank holiday)
            DateTime lastMayBankHoliday = new DateTime(year, 05, 31);
            while (lastMayBankHoliday.DayOfWeek != DayOfWeek.Monday)
            {
                lastMayBankHoliday = lastMayBankHoliday.AddDays(-1);
            }
            result.Add(lastMayBankHoliday);


            // August
            DateTime augustBankHoliday = new DateTime(year, 08, 31);
            while (augustBankHoliday.DayOfWeek != DayOfWeek.Monday)
            {
                augustBankHoliday = augustBankHoliday.AddDays(-1);
            }
            result.Add(augustBankHoliday);

            // DEC
            // Chrismast 
            DateTime christmas = new DateTime(year, 12, 25);
            while (IsWorkingDay(christmas.DayOfWeek) == false)
            {
                christmas = christmas.AddDays(1);
            }
            result.Add(christmas);

            // December's bank holiday is the first working day on, or after Boxing day.
            DateTime boxingDay = new DateTime(year, 12, 26);
            DateTime bankHoliday = boxingDay;
            
            // chrismas replacement first
            if (boxingDay.DayOfWeek == DayOfWeek.Monday)
                bankHoliday = boxingDay.AddDays(1);

            while (IsWorkingDay(bankHoliday.DayOfWeek) == false)
            {
                bankHoliday = bankHoliday.AddDays(1);
            }
            result.Add(bankHoliday);

            return result;
        }
    }
}

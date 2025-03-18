using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace JobLogic.Infrastructure.Utilities
{
    public static class EnumUtils
    {
        public static string GetDescription(this Enum enumValue)
        {
            if (enumValue == null) return string.Empty;

            var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());
            if (fieldInfo == null) return string.Empty;

            object[] attr = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attr.Length > 0
                   ? ((DescriptionAttribute)attr[0]).Description
                   : enumValue.ToString();
        }

        public static string GetDisplayName(this Enum enumeration)
        {
            if (enumeration == null) return string.Empty;

            var enumType = enumeration.GetType();
            var enumName = Enum.GetName(enumType, enumeration);
            var displayName = enumName;
            try
            {
                var member = enumType.GetMember(enumName)[0];

                var attributes = member.GetCustomAttributes(typeof(DisplayAttribute), false);
                var attribute = (DisplayAttribute)attributes[0];
                displayName = attribute.Name;

                if (attribute.ResourceType != null)
                {
                    displayName = attribute.GetName();
                }
            }
            catch { }
            return displayName;
        }

        public static T TryParseEnum<T>(this string value, ref T returnValue) where T : struct
        {
            try
            {
                Enum.TryParse<T>(value, out returnValue);
            }
            catch { }

            return returnValue;
        }

        public static T? TryParseEnum<T>(this string value) where T : struct
        {
            try
            {
                T returnValue;
                if (Enum.TryParse<T>(value, out returnValue))
                {
                    return returnValue;
                }
            }
            catch { }

            return null;
        }

        public static T ParseEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static T ParseEnumFromDescription<T>(string descriptionValue, bool ignoreCaseSensitive = false)
        {
            var type = typeof(T);
            if (!type.IsEnum)
            {
                throw new InvalidOperationException();
            }

            foreach (var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field,
                    typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attribute != null)
                {
                    if (attribute.Description.Equals(descriptionValue, ignoreCaseSensitive ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal))
                    {
                        return (T)field.GetValue(null);
                    }
                }
                else
                {
                    if (field.Name.Equals(descriptionValue, ignoreCaseSensitive ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal))
                        return (T)field.GetValue(null);
                }
            }

            throw new ArgumentOutOfRangeException("descriptionValue");
        }

        public static bool TryParseEnumFromDescription<T>(string descriptionValue, ref T returnValue)
        {
            try
            {
                returnValue = ParseEnumFromDescription<T>(descriptionValue);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static bool TryParseEnumFromDescription<T>(string descriptionValue, ref T returnValue, bool ignoreCasesensitive)
        {
            try
            {
                returnValue = ParseEnumFromDescription<T>(descriptionValue, ignoreCasesensitive);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static IEnumerable<T> GetValues<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>().ToList();
        }

        public static bool IsValueDefined<T>(T valueItem)
        {
            return Enum.IsDefined(typeof(T), valueItem);
        }

        public static List<T> ConvertStringToEnumList<T>(string str) where T : struct
        {
            if (string.IsNullOrWhiteSpace(str))
                return null;

            var ValList = new List<T>();
            T returnEnum;
            foreach (var x in str.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (!string.IsNullOrWhiteSpace(x) && Enum.TryParse(x.Trim(), out returnEnum))
                {
                    ValList.Add(returnEnum);
                }
            }
            return ValList;
        }

        public static IEnumerable<DayOfWeek> GetDaysOfWeek(bool isSundayLast = true)
        {
            var enumerable = Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>();
            return isSundayLast
                ? enumerable.OrderBy(x => ((int)x + 6) % 7) //Sort Monday first
                : enumerable.OrderBy(x => (int)x);
        }

        public static DayOfWeek GetNextDay(this DayOfWeek currentDay)
        {
            return (DayOfWeek)Enum.Parse(typeof(DayOfWeek), (((int)currentDay + 1) % 7).ToString());
        }

        public static DayOfWeek GetPreviousDay(this DayOfWeek currentDay)
        {
            return (DayOfWeek)Enum.Parse(typeof(DayOfWeek), (((int)currentDay + 6) % 7).ToString());
        }

        public static int GetDayDifference(this DayOfWeek currentDay, DayOfWeek otherDay)
        {
            var diff = (int)currentDay - (int)otherDay;
            return diff < 0 ? diff + 7 : diff;
        }

        public static string NumberToString(this Enum enVal)
        {
            return enVal.ToString("D");
        }

        public static bool IsOneOf<TEnum>(this TEnum enumeration, params TEnum[] enums) where TEnum : Enum
        {
            if (enums.IsNullOrEmpty())
            {
                return false;
            }

            return enums.Contains(enumeration);
        }

        public static string GetEnumValueAsString(this Enum enumValue) =>
            enumValue.GetEnumValueAsInt().ToString();

        public static int GetEnumValueAsInt(this Enum enumValue) =>
            Convert.ToInt32(enumValue);
    }
}

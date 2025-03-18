using System;
using System.Data.SqlTypes;
using System.Text.RegularExpressions;

namespace JobLogic.Infrastructure.Utilities
{
    public static class ValidationUtils
    {
        public const string POSTCODE_REGEX = @"(([A-PR-UW-Z]{1}[A-IK-Y]?)([0-9]?[A-HJKS-UW]?[ABEHMNPRVWXY]?|[0-9]?[0-9]?))\s?([0-9]{1}[ABD-HJLNP-UW-Z]{2})";
        public const string MOBILENUMBER_REGEX = @"^(\+44\s?7\d{3}|\(?07\d{3}\)?)\s?\d{3}\s?\d{3}$";
        public const string LANDLINENUMBER_REGEX = @"^((\(?0\d{4}\)?\s?\d{3}\s?\d{3})|(\(?0\d{3}\)?\s?\d{3}\s?\d{4})|(\(?0\d{2}\)?\s?\d{4}\s?\d{4}))(\s?\#(\d{4}|\d{3}))?$";
        private static readonly Regex _emailRegex = CreateRegEx();

        public static bool isMobileNumber(string value, int length = 100, bool required = false)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return !required;
            }

            value = value.Replace(" ", string.Empty);

            if (value.Length > length)
            {
                return false;
            }

            return Regex.IsMatch(value, MOBILENUMBER_REGEX);
        }

        public static bool isLandlineNumber(string value, int length = 100, bool required = false)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return !required;
            }

            value = value.Replace(" ", string.Empty);

            if (value.Length > length)
            {
                return false;
            }

            return Regex.IsMatch(value, LANDLINENUMBER_REGEX);
        }

        public static bool isEmail(this string value, int length = 255, bool required = true)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return !required;
            }

            value = Regex.Replace(value, @"\s+", "");//Remove all white space

            if (value.Length > length)
            {
                return false;
            }

            return _emailRegex.Match(value).Length > 0;
        }

        public static bool isPostcode(string value, int length = 20, bool required = true)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return !required;
            }

            if (value.Length > length)
            {
                return false;
            }

            return Regex.IsMatch(value.ToUpper(), POSTCODE_REGEX);
        }

        public static string StripHTML(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return null;
            }

            char[] array = new char[str.Length];
            int arrayIndex = 0;
            bool inside = false;

            for (int i = 0; i < str.Length; i++)
            {
                char c = str[i];
                if (c == '<')
                {
                    inside = true;
                    continue;
                }
                if (c == '>')
                {
                    inside = false;
                    continue;
                }
                if (!inside)
                {
                    array[arrayIndex] = c;
                    arrayIndex++;
                }
            }

            string tagsRemoved = new string(array, 0, arrayIndex);

            if (!string.IsNullOrWhiteSpace(tagsRemoved))
            {
                tagsRemoved = tagsRemoved.Replace("&nbsp;", " ");
            }

            return tagsRemoved;
        }

        public static bool isValidDate(this DateTime value)
        {
            return value.Year > SqlDateTime.MinValue.Value.Year &&
                value.Year <= SqlDateTime.MaxValue.Value.Year;
        }

        public static bool isValidDecimal(decimal value)
        {
            return value > 0;
        }

        public static bool isValidCost(decimal value)
        {
            return value >= 0;
        }

        public static bool isTransient(this int? value)
        {
            if (!value.HasValue)
            {
                return true;
            }

            return value.Value == 0;
        }

        public static bool isValidStringLength(this string value, int length)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return true;
            }

            return value.Length <= length;
        }

        private static Regex CreateRegEx()
        {
            const string pattern = @"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$";
            const RegexOptions options = RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture;

            return new Regex(pattern, options);
        }
    }
}

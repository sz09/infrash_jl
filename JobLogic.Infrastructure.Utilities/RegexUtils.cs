using System.Text.RegularExpressions;

namespace JobLogic.Infrastructure.Utilities
{
    public static class RegexUtils
    {
        public static string WildCardToRegular(string value)
        {
            return "^" + Regex.Escape(value).Replace("\\?", ".").Replace("\\*", ".*") + "$";
        }

        public static bool IsWildCard(string value)
        {
            // X*
            if (Regex.IsMatch(value, @"^[A-Za-z0-9_.-]*[*]$")) return true;
            // *X
            if (Regex.IsMatch(value, @"^[*][A-Za-z0-9_.-]*$")) return true;
            // *X*
            if (Regex.IsMatch(value, @"^[*][A-Za-z0-9_.-]*[*]$")) return true;

            return false;
        }
    }
}

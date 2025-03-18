using System.Text.RegularExpressions;

namespace JobLogic.Infrastructure.Utilities
{
    public static class EmailUtils
    {
        public static bool IsValidEmailAddress(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return false;

            var regex = new Regex(RegularExpressionUtils.MatchSingleEmailExpression, RegexOptions.IgnoreCase);
            return regex.IsMatch(value);
        }

        public static bool IsValidEmail(string value)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(value);
                return addr.Address == value;
            }
            catch
            {
                return false;
            }
        }
    }
}

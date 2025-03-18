namespace JobLogic.Infrastructure.Utilities
{
    public static class StringFormatUtils
    {
        public static string FullName(string firstName, string lastName)
        {
            return firstName + (!string.IsNullOrWhiteSpace(lastName) ? (" " + lastName) : "");
        }
    }
}

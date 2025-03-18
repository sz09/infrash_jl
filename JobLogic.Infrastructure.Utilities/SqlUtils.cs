using System.Text;

namespace JobLogic.Infrastructure.Utilities
{
    public static class SqlUtils
    {
        public static string ValidateDatabaseName(this string databaseName)
        {
            if (string.IsNullOrWhiteSpace(databaseName)) return null;
            return databaseName.Replace(" ", "").OnlyNumbersandLetters();
        }

        public static string EscapeSpecialCharacters(this string keyword)
        {
            StringBuilder sb = new StringBuilder(keyword.Length);
            for (int i = 0; i < keyword.Length; i++)
            {
                char c = keyword[i];
                switch (c)
                {
                    case ']':
                    case '[':
                    case '%':
                    case '*':
                        sb.Append("[" + c + "]");
                        break;
                    case '\'':
                        sb.Append("''");
                        break;
                    default:
                        sb.Append(c);
                        break;
                }
            }
            return sb.ToString();
        }

        public static int ValidatePageSize(this int pageSize)
        {
            if (pageSize < 5) pageSize = 5;
            if (pageSize > 50)
            {
                pageSize = 50;
            }
            return pageSize;
        }

        public static int ValidatePageIndex(this int pageIndex)
        {
            if (pageIndex < 1) pageIndex = 1;
            return pageIndex;
        }
    }
}

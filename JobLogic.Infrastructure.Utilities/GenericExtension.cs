using System;
using System.Collections.Generic;

namespace JobLogic.Infrastructure.Utilities
{
    public static class GenericExtension
    {
        public static string NullIfBlank(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;
            return value;
        }

        public static string BlankStringIfNull(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return "";
            return value;
        }

        public static string SpecialSubString(this string value, int tryLength)
        {
            if (string.IsNullOrWhiteSpace(value))
                return "";

            if (value.Length > tryLength)
                return value.Substring(0, tryLength);

            return value;
        }

        public static char? NullIfBlank(this char value)
        {
            if (char.IsWhiteSpace(value))
                return null;
            return value;
        }


        public static T GetValue<T>(this string value) where T : struct
        {
            if (string.IsNullOrWhiteSpace(value))
                return default(T);

            try
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch
            {
                return default(T);
            }
        }

        public static T? GetValueOrNull<T>(this string value) where T : struct
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            try
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch
            {
                return null;
            }
        }


        public static string GetCommaSeparatedValue<T>(this IEnumerable<T> values, string separator = ", ")
        {
            if (values != null)
                return String.Join(separator, values);
            return String.Empty;
        }

        public static string AsSeparatedString(params string[] strs)
        {
            string separator = ", ";
            string s = "";
            if (strs.Length == 0) return s;

            s += strs[0];
            for (int i = 1; i < strs.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(strs[i]))
                    s += separator + strs[i];
            }

            return s;

        }

        public static string AsSeparatedStringNoSpace(params string[] strs)
        {
            string separator = ",";
            string s = "";
            if (strs.Length == 0) return s;

            s += strs[0];
            for (int i = 1; i < strs.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(strs[i]))
                    s += separator + strs[i];
            }

            return s;

        }

        public static string AsDelimiteredString(params string[] strs)
        {
            string delimiter = "\"";
            List<string> list = new List<string>();
            for (int i = 0; i < strs.Length; i++)
            {
                list.Add((delimiter + strs[i] + delimiter));
            }

            return (AsSeparatedStringNoSpace(list.ToArray()));
        }

        public static string AsDelimiteredStringForJLWEB(params string[] strs)
        {
            string delimiter = "\"";
            string SEPARATOR = ",";
            List<string> list = new List<string>();
            string stLine = "";
            for (int i = 0; i < strs.Length; i++)
            {
                 stLine =  stLine + "=" + delimiter + stripSepAndDel(strs[i]) + delimiter + SEPARATOR ;
            }

            return stLine;
        }

        public static string stripSepAndDel(object data)
        {
            string SEPARATOR = ",";
            string DELIMITER = "\"";
            if (data != null)
            {
                try
                {
                    string tmp = data.ToString();
                    if (!String.IsNullOrEmpty(DELIMITER))
                        tmp = tmp.Replace(DELIMITER, "");
                    tmp = tmp.Replace(SEPARATOR, "");
                    return tmp;
                }
                catch
                {
                }

                try
                {
                    string tmp = (string)data;
                    if (!String.IsNullOrEmpty(DELIMITER))
                        tmp = tmp.Replace(DELIMITER, "");
                    tmp = tmp.Replace(SEPARATOR, "");
                    return tmp;
                }
                catch
                {
                }
            }
            return "";
        }

        public static string GetValue(this Dictionary<int, string> dict, int key)
        {
            var value = string.Empty;
            if(dict != null)
            {
                dict.TryGetValue(key, out value);
            }
            return value;
        }

        public static void AddValue<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value)
        {
            if(dict == null)
            {
                dict = new Dictionary<TKey, TValue>();
            }
            if (!dict.ContainsKey(key))
            {
                dict.Add(key, value);
            }
            else
            {
                dict[key] = value;
            }
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            var seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
    }
}
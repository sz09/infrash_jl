using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;

namespace JobLogic.Infrastructure.Utilities
{
    public static class StringUtils
    {
        private static string _pdfFileType = ".pdf";
        private static string _docxFileType = ".docx";
        private static string _docFileType = ".docx";
        private static string _localizationMapPrefix = "<<{0}>>";
        private static string _localizationSeparator = "|";

        public static T[] AsGenericArray<T>(this string str) where T : struct
        {
            if (string.IsNullOrEmpty(str)) return null;

            var stringList = str.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);

            return stringList.Select(x => (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromString(x)).ToArray();
        }

        public static int ParseAsIntOrDefault(this string str, int defaultValue)
        {
            int.TryParse(str, out defaultValue);
            return defaultValue;
        }

        public static int[] AsIntArray(this string str)
        {
            if (string.IsNullOrEmpty(str)) return null;

            return str.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x.Trim())).ToArray();
        }

        public static string[] AsStringArray(this string str)
        {
            if (string.IsNullOrWhiteSpace(str)) return null;

            return str.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(x=>x.Trim()).ToArray();
        }

        public static string AsSeparatedString(params int[] strs)
        {
            if (strs == null || strs.Length == 0) return string.Empty;

            string separator = ",";
            return string.Join(separator, strs);
        }

        public static string AsSeparatedString(params string[] strs)
        {
            if (strs == null || strs.Length == 0) return string.Empty;

            string separator = ",";
            return string.Join(separator, strs);
        }

        public static string AsSeparatedString(this IEnumerable<string> strs)
        {
            if (strs == null || !strs.Any()) return string.Empty;
            
            string separator = ",";
            return string.Join(separator, strs);
        }

        public static string AsAddressString(params string[] strs)
        {
            if (strs == null || strs.Length == 0) return string.Empty;

            string separator = ", ";
            return string.Join(separator, strs.Where(m => !string.IsNullOrWhiteSpace(m)));
        }

        public static string AsConcatenateNewLineString(params string[] strs)
        {
            if (strs == null || strs.Length == 0) return string.Empty;

            string separator = Environment.NewLine;
            return string.Join(separator, strs.Where(m => !string.IsNullOrWhiteSpace(m)));
        }

        public static string AsDelimiteredString(IEnumerable<string> strs)
        {
            if (strs == null || !strs.Any()) return string.Empty;
            return AsDelimiteredString(strs.ToArray());
        }

        public static string AsDelimiteredString(params string[] strs)
        {
            if (strs == null || strs.Length == 0) return string.Empty;

            var delimiter = "\"";
            var list = new List<string>();
            for (int i = 0; i < strs.Length; i++)
            {
                list.Add((delimiter + strs[i] + delimiter));
            }

            return AsSeparatedString(list);
        }

		public static string AsDelimiteredStringForCSV(IEnumerable<string> strs)
		{
			if (strs == null || !strs.Any()) return string.Empty;
			return AsDelimiteredStringForCSV(strs.ToArray());
		}

		public static string AsDelimiteredStringForCSV(params string[] strs)
		{
			if (strs == null || strs.Length == 0) return string.Empty;

			var delimiter = "\"";
			var list = new List<string>();
			for (int i = 0; i < strs.Length; i++)
			{
				if (strs[i] == null)
					list.Add((delimiter + strs[i] + delimiter));
				else
					list.Add((delimiter + strs[i].Replace("\"", "\"\"") + delimiter));
			}

			return AsSeparatedString(list);
		}

		public static string RemoveCharacters(this string value, int startIndex, int count)
        {
            if (string.IsNullOrWhiteSpace(value)) return value;

            return value.Remove(startIndex, count);
        }

        public static string Upper(this string value)
        {
            if (string.IsNullOrEmpty(value)) return value;

            return value.ToUpperInvariant();
        }

        public static string Lower(this string value)
        {
            if (string.IsNullOrEmpty(value)) return value;

            return value.ToLowerInvariant();
        }

        public static string Truncate(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;

            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }

        public static string TrimValue(this string value, int maxLength = 255, string trailingCharacter = "...")
        {
            var returnValue = Truncate(value, maxLength);

            if (returnValue.Length == maxLength) return returnValue + trailingCharacter;

            return returnValue;
        }

        public static string Left(string value, int length)
        {
            if (string.IsNullOrEmpty(value)) return value;

            return value.Substring(0, length);
        }

        public static string Right(string value, int length)
        {
            if (string.IsNullOrEmpty(value)) return value;

            return value.Substring(value.Length - length, length);
        }

        public static string OnlyNumbersandLetters(this string value)
        {
            if (string.IsNullOrEmpty(value)) return value;

            var str = System.Text.RegularExpressions.Regex.Replace(value, @"[^a-zA-Z0-9]", "");
            if (string.IsNullOrEmpty(str)) return null;

            return str;
        }

        public static string SetCurrencySymbol(this string value, string currency)
        {
            if (string.IsNullOrEmpty(value)) return value;

            if (string.IsNullOrEmpty(currency)) return value;

            var str = currency.ToUpper() + ' ' + value;

            return str;
        }

        public static string GeneratePdfFileName(string pdfName = null, string docNamePrefix = null)
        {
            return GenerateFileName(pdfName, docNamePrefix, _pdfFileType);
        }

        public static string GenerateDocXFileName(string docxName = null, string docNamePrefix = null)
        {
            return GenerateFileName(docxName, docNamePrefix, _docxFileType);
        }

        private static string GenerateFileName(string pdfName, string docNamePrefix, string fileType)
        {
            var constructPdfName = !string.IsNullOrWhiteSpace(docNamePrefix) ? docNamePrefix + " - " : string.Empty;

            if (!string.IsNullOrWhiteSpace(pdfName))
                constructPdfName += pdfName + fileType;
            else
                constructPdfName += Guid.NewGuid().ToString() + fileType;

            return constructPdfName.ValidateFileName();
        }

        public static string HtmlEncode(this string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return value;

            return System.Net.WebUtility.HtmlEncode(value).Replace(Environment.NewLine, "<br/>");
        }

        public static string HtmlDecode(this string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return value;

            return System.Net.WebUtility.HtmlDecode(value).Replace("<br/>", Environment.NewLine).Replace("\r\n", Environment.NewLine).Replace("\\r\\n", Environment.NewLine);
        }

        /// <summary>
        /// This method is used to initialize value if it is null or empty
        /// </summary>
        /// <param name="value">Value need to initialize</param>
        /// <returns>Guid value if its value is null or empty. Otherwise return its value</returns>
        public static string GenerateGuidValueIfNullOrEmpty(this string value)
        {
            return (string.IsNullOrWhiteSpace(value) ? Guid.NewGuid().ToString() : value);
        }

        public static string ReplaceLineBreaks(this string lines, string replacement = " ")
        {
            if (string.IsNullOrWhiteSpace(lines)) return lines;
            return lines.Replace("\r\n", replacement)
                        .Replace("\r", replacement)
                        .Replace("\n", replacement);
        }

        public static string RemoveSuffix(this string value, string suffix)
        {
            var result = "";
            if (!string.IsNullOrWhiteSpace(value))
            {
                value = value.TrimEnd();
                result = (!string.IsNullOrWhiteSpace(suffix) && value.EndsWith(suffix)) ? value.Remove(value.LastIndexOf(suffix)) : value;
            }
            return result;
        }

        public static string RemovePrefix(this string value, string prefix)
        {
            var result = "";
            if (!string.IsNullOrWhiteSpace(value))
            {
                value = value.TrimStart();
                result = (!string.IsNullOrWhiteSpace(prefix) && value.StartsWith(prefix)) ? value.Substring(prefix.Length) : value;
            }
            return result;
        }

        /// <summary>
        /// Replaces symbols which shows as BOM when not encoded on HTML documents
        /// </summary>
        public static string ReplaceCurrencySymbol(this string value)
        {
            return value.Replace("€", "&euro;").Replace("£", "&pound;");
        }

        public static string GetPrefixNumber(this string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return value;

            return new String(value.Trim().TakeWhile(Char.IsDigit).ToArray());
        }

        //public static string GetPlainTextFromHtml(this string htmlString, bool needDecodeFirst = true)
        //{
        //    if (needDecodeFirst) htmlString = HttpUtility.HtmlDecode(htmlString);
        //    htmlString = Regex.Replace(htmlString, @"<br\s*\/>", Environment.NewLine, RegexOptions.IgnoreCase);
        //    var htmlTagPattern = "<.*?>";
        //    return Regex.Replace(htmlString, htmlTagPattern, string.Empty);
        //}

        public static bool GetEntityId(this string entityId, out int intId, out Guid guidId)
        {
            guidId = Guid.Empty;
            if (!int.TryParse(entityId, out intId))
            {
                if (!Guid.TryParse(entityId, out guidId))
                {
                    return false;
                }
            }
            return true;
        }

        public static string RemoveHtmlTagCharacters(this string value)
        {
            return string.IsNullOrEmpty(value) ? value : Regex.Replace(value, "[<>]", string.Empty);
        }

        public static string SanitiseAndSplitByComma(this string ids)
        {
            if (string.IsNullOrWhiteSpace(ids))
            {
                return "NULL";
            }
            return ids.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).SanitiseAndSplitByComma();
        }

        public static string SanitiseAndSplitByComma(this IEnumerable<string> ids)
        {
            if (ids == null || !ids.Any())
            {
                return "NULL";
            }
            var listIds = ids.Where(id => !string.IsNullOrWhiteSpace(id))
                             .Select(id => id.Replace("'", "").Trim())
                             .ToList();

            return string.Join(",", listIds.Select(id => $"'{id}'"));
        }

        /// <summary>
        /// Return list of valid parsed items, will be empty list if input is null or empty
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ids">Comma separated string</param>
        /// <returns></returns>
        public static List<T> GetValues<T>(this string ids)
        {
            if (!string.IsNullOrWhiteSpace(ids))
            {
                var type = typeof(T);

                if (type.IsValueType || type == typeof(string))
                {
                    var converter = TypeDescriptor.GetConverter(type);
                    return ids.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                              .Where(x => !string.IsNullOrWhiteSpace(x))
                              .Select(x => x.Trim())
                              .Where(it => converter.IsValid(it))
                              .Select(x => (T)converter.ConvertFromString(x))
                              .ToList();
                }
            }
            return new List<T>();
        }

        /// <summary>
        /// Format message with localication
        /// </summary>
        /// <param name="message"></param>
        /// <param name="localizations"></param>
        /// <returns></returns>
        public static string FormatWithLocalization(this string message, string[] localizations)
        {
            if (localizations?.Any() == false) return message;
            return string.Concat(message, String.Format(_localizationMapPrefix, string.Join(_localizationSeparator, localizations)));
        }
    }
}
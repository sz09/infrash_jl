using System;
using System.Collections.Generic;
using System.Linq;

namespace JobLogic.Infrastructure.Utilities
{
    /// <summary>
    /// This class is used to generate file name
    /// </summary>
    public sealed class FileNameGenerator
    {
        public static string DetailedCase = "Detailed";
        public static string TotalsOnlyCase = "Totals Only";

        /// <summary>
        /// Generate file name from parts
        /// </summary>
        /// <param name="prefixPart">Value of prefix part</param>
        /// <param name="middlePart">Value of middle part</param>
        /// <param name="suffixPart">Value of suffix part</param>
        /// <param name="fileType">Value of file type</param>
        /// <returns>File name</returns>
        public string Generate(string prefixPart, string middlePart, string suffixPart = null, string fileType = "pdf")
        {
            if(string.IsNullOrWhiteSpace(prefixPart) && string.IsNullOrWhiteSpace(middlePart) && string.IsNullOrWhiteSpace(suffixPart))
            {
                throw new ArgumentNullException();
            }

            var parts = new List<string> { prefixPart, middlePart, suffixPart };

            var fileName = string.Format("{0}.{1}", string.Join(" - ", parts.Where(x => !string.IsNullOrEmpty(x))), fileType);
            return fileName.ValidateFileName();
        }
    }
}
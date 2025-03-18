using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace JobLogic.Infrastructure.Utilities
{
    public class SearchTermUtils
    {
        private static readonly Lazy<SearchTermUtils> lazy =
        new Lazy<SearchTermUtils>(() => new SearchTermUtils());

        public static SearchTermUtils Instance { get { return lazy.Value; } }

        private SearchTermUtils()
        {
        }

        public IList<string> Build(string searchTerm, bool searchExact = true)
        {
            if (string.IsNullOrWhiteSpace(searchTerm)) return new List<string>();

            var matchesList = new List<string>();
            if (searchExact)
            {
                var regex = new Regex("\"(.*?)\"");
                var matches = regex.Matches(searchTerm);
                foreach (Match match in matches)
                {
                    matchesList.Add(match.Value.Replace("\"", ""));
                    searchTerm = searchTerm.Replace(match.Value, "");
                }
            }
            var filters = searchTerm.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            if (searchExact) filters.AddRange(matchesList);

            return filters;
        }
    }
}

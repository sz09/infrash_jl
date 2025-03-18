using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace JobLogic.Infrastructure.CultureHelper
{
    public static class CurrencyHelper
    {
        private static readonly object thisLock = new object();
        private static IDictionary<string, string> _currencyMap = null;
        public static IDictionary<string, string> CurrencyMap
        {
            get
            {
                if (_currencyMap == null)
                {
                    lock (thisLock)
                    {
                        if (_currencyMap == null)
                        {
                            _currencyMap = CultureInfo
                            .GetCultures(CultureTypes.AllCultures)
                            .Where(c => !c.IsNeutralCulture)
                            .Select(culture =>
                            {
                                try
                                {
                                    return new RegionInfo(culture.LCID);
                                }
                                catch
                                {
                                    return null;
                                }
                            })
                            .Where(ri => ri != null)
                            .GroupBy(ri => ri.ISOCurrencySymbol)
                            .ToDictionary(x => x.Key, x => x.First().CurrencySymbol);
                        }
                    }
                }
                return _currencyMap;
            }
        }

        public static string GetSymbol(string code)
        {
            CurrencyMap.TryGetValue(code, out string symbol);
            return symbol;
        }
    }
}

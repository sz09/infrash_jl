using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using PhoneNumbers;

namespace JobLogic.Infrastructure.SMS
{
    public class PhoneNumberUtils
    {
        /// <summary>
        /// Format a phone number to E164 format (+447497...) using countryCode or country Name
        /// </summary>
        /// <param name="phoneNumber"> phone number in international or national format</param>
        /// <param name="countryCode">(optional if country name is provided) GB, US, CA</param>
        /// <param name="countryName">(optional if country code was provide)"United Kingdom"</param>
        /// <returns></returns>
        public static string FormatE164PhoneNumber(string phoneNumber, string countryCode, string countryName)
        {
            // Get country code
            if ((string.IsNullOrWhiteSpace(countryCode) && string.IsNullOrWhiteSpace(countryName)) || string.IsNullOrWhiteSpace(phoneNumber))
            {
                return phoneNumber;
            }
            // handle 00 instead of +
            if (phoneNumber.StartsWith("00"))
            {
                phoneNumber = "+" + phoneNumber.Remove(0, 2);
            }

            var regions = CultureInfo.GetCultures(CultureTypes.SpecificCultures).Select(x => new RegionInfo(x.LCID));
            var countryRegion = regions.FirstOrDefault(region => (!string.IsNullOrWhiteSpace(countryName) && region.EnglishName.Contains(countryName)) || (!string.IsNullOrWhiteSpace(countryCode) &&region.TwoLetterISORegionName.Contains(countryCode)));
            if (countryRegion == null)
            {
                return phoneNumber;
            }
            countryCode = countryRegion.TwoLetterISORegionName;

            var phoneNumberUtil = PhoneNumberUtil.GetInstance();

            var number = phoneNumberUtil.Parse(phoneNumber, countryCode);
            if (!phoneNumberUtil.IsValidNumber(number))
            {
                return phoneNumber;
            }
            return phoneNumberUtil.Format(number, PhoneNumberFormat.E164);
        }
    }
}

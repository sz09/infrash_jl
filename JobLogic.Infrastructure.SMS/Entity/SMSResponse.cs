using System;
using System.Collections.Generic;

namespace JobLogic.Infrastructure.SMS
{
    public class SMSResponse
    {
        public string Sid { get; set; }
        public string To { get; set; }
        public decimal? Cost { get; set; }
        public string From { get; set; }
        public string Body { get; set; }
        public string Status { get; set; }
    }


    public class SMSTwoWayResponse
    {
        public string FlowSid { get; set; }
        public string ContactChannelAddress { get; set; }
        public string AccountSid { get; set; }
        public DateTime? DateCreated { get; set; }
        public string Sid { get; set; }
        public string ContactSid { get; set; }
    }

    public class SMSPhoneNumberResource
    {
		public Dictionary<string, string> CallerName { get; set; }
		public string CountryCode { get; set; }
		public string PhoneNumber { get; set; }
		public string NationalFormat { get; set; }
		public Dictionary<string, string> Carrier { get; set; }
		public object AddOns { get; set; }
		public Uri Url { get; set; }
	}
}

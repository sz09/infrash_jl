using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Pipedrive.Models.Response.Deals
{
    public class DealStayInPipelineStage
    {
        [JsonProperty("times_in_stages")]
        public Dictionary<long, ulong> TimesInStages { get; set; }

        [JsonProperty("order_of_stages")]
        public List<long> OrderOfStages { get; set; }
    }
}

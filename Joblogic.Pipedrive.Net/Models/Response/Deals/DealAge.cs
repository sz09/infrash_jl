﻿using Newtonsoft.Json;

namespace Pipedrive.CustomFields
{
    public class DealAge 
    {
        [JsonProperty("y")]
        public byte Y { get; set; }

        [JsonProperty("m")]
        public byte M { get; set; }

        [JsonProperty("d")]
        public byte D { get; set; }

        [JsonProperty("h")]
        public byte H { get; set; }

        [JsonProperty("i")]
        public byte I { get; set; }

        [JsonProperty("s")]
        public byte S { get; set; }

        [JsonProperty("total_seconds")]
        public ulong TotalSeconds { get; set; }
    }
}

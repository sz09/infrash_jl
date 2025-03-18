using Newtonsoft.Json;

namespace JobLogic.TVSClient.Response
{
    public class TVSJobResponse
    {
        [JsonProperty("transactionId")]
        public string TransactionId { get; set; }

        [JsonProperty("uniqueID")]
        public string UniqueID { get; set; }

        [JsonProperty("jobId")]
        public string JobId { get; set; }

        [JsonProperty("successful")]
        public string Successful { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("elapsedTime")]
        public Elapsedtime ElapsedTime { get; set; }

        [JsonProperty("sendToEsb")]
        public bool SendToEsb { get; set; }
    }

    public class Elapsedtime
    {
        [JsonProperty("ticks")]
        public int Ticks { get; set; }

        [JsonProperty("days")]
        public int Days { get; set; }

        [JsonProperty("hours")]
        public int Hours { get; set; }

        [JsonProperty("milliseconds")]
        public int Milliseconds { get; set; }

        [JsonProperty("minutes")]
        public int Minutes { get; set; }

        [JsonProperty("seconds")]
        public int Seconds { get; set; }

        [JsonProperty("totalDays")]
        public float TotalDays { get; set; }

        [JsonProperty("totalHours")]
        public float TotalHours { get; set; }

        [JsonProperty("totalMilliseconds")]
        public float TotalMilliseconds { get; set; }

        [JsonProperty("totalMinutes")]
        public float TotalMinutes { get; set; }

        [JsonProperty("totalSeconds")]
        public float TotalSeconds { get; set; }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace JobLogic.TVSClient.Request
{
    public class JobMessageContext
    {
        [JsonProperty("message_context")]
        public MessageContext MessageContext { get; set; }

        [JsonProperty("jobs")]
        public IEnumerable<JobRequest> Jobs { get; set; }

    }

    public class MessageContext
    {
        [JsonProperty("environment")]
        public string Environment { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; }

        [JsonProperty("message_type")]
        public string MessageType { get; set; }

        [JsonProperty("unique_id")]
        public string UniqueId { get; set; }

        [JsonProperty("source_system")]
        public string SourceSystem { get; set; }

        [JsonProperty("target_system")]
        public string TargetSystem { get; set; }

        [JsonProperty("master_system")]
        public string MasterSystem { get; set; }

        [JsonProperty("tracking_id")]
        public string TrackingId { get; set; }

        [JsonProperty("to_client")]
        public string ToClient { get; set; }
    }

    public class JobRequest
    {
        [JsonProperty("guid")]
        public string Guid { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("job_reference")]
        public string JobReference { get; set; }

        [JsonProperty("external_job_reference")]
        public string ExternalJobReference { get; set; }

        [JsonProperty("job_type")]
        public JobType JobType { get; set; }

        [JsonProperty("current_job_status")]
        public JobStatus CurrentJobStatus { get; set; }

        [JsonProperty("job_summary")]
        public JobSummary JobSummary { get; set; }

        [JsonProperty("job_details")]
        public JobDetail JobDetails { get; set; }

        [JsonProperty("scheduled")]
        public Schedule Scheduled { get; set; }

        [JsonProperty("planned")]
        public Planned Planned { get; set; }

        [JsonProperty("required_by")]
        public RequiredBy RequiredBy { get; set; }

        [JsonProperty("client")]
        public Client Client { get; set; }

        [JsonProperty("customer")]
        public Customer Customer { get; set; }

        [JsonProperty("used_items")]
        public UsedItems UsedItems { get; set; }

        [JsonProperty("activities")]
        public Activities Activities { get; set; }
    }

    public class Activities
    {
        [JsonProperty("activity")]
        public IEnumerable<Activity> Activity { get; set; }
    }

    public class Activity
    {
        [JsonProperty("attribute_value")]
        public string AttributeValue { get; set; }

        [JsonProperty("attribute_name")]
        public string AttributeName { get; set; }

        [JsonProperty("user_identity")]
        public string UserIdentity { get; set; }

        [JsonProperty("email_address")]
        public string EmailAddress { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("effort")]
        public Effort Effort { get; set; }

        [JsonProperty("start")]
        public Planned Start { get; set; }

        [JsonProperty("end")]
        public Planned End { get; set; }
    }

    public class Effort
    {
        [JsonProperty("time")]
        public string Time { get; set; }
    }

    public class UsedItems
    {
        [JsonProperty("used_item")]
        public IEnumerable<UsedItem> UsedItem { get; set; }
    }

    public class UsedItem
    {
        [JsonProperty("item_number")]
        public string ItemNumber { get; set; }

        [JsonProperty("external_item_number")]
        public string ExternalItemNumber { get; set; }

        [JsonProperty("quantity")]
        public string Quantity { get; set; }

        [JsonProperty("serials")]
        public IEnumerable<string> Serials { get; set; }
    }

    public class Client
    {
        [JsonProperty("client_code")]
        public string ClientCode { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class Customer
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("guid")]
        public Guid Guid { get; set; }

        [JsonProperty("external_customer_number")]
        public string ExternalCustomerNumber { get; set; }

        [JsonProperty("address")]
        public Address Address { get; set; }
    }

    public class Address
    {
        [JsonProperty("address_line_1")]
        public string AddressLine_1 { get; set; }

        [JsonProperty("address_line_2")]
        public string AddressLine_2 { get; set; }

        [JsonProperty("address_line_3")]
        public string AddressLine_3 { get; set; }

        [JsonProperty("town_or_city")]
        public string TownOrCity { get; set; }

        [JsonProperty("country_or_state_name")]
        public string CountryOrStateName { get; set; }

        [JsonProperty("postal_code")]
        public string PostalCode { get; set; }

        [JsonProperty("country_code")]
        public string CountryCode { get; set; }

        [JsonProperty("address_id")]
        public string AddressId { get; set; }
    }

    public class JobType
    {

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }

    public class JobStatus
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("user_identity")]
        public string UserIdentity { get; set; }

        [JsonProperty("email_address")]
        public string EmailAddress { get; set; }

        [JsonProperty("date")]
        public string Date { get; set; }

        [JsonProperty("time")]
        public string Time { get; set; }
    }

    public class JobSummary
    {
        [JsonProperty("text")]
        public string Text { get; set; }
    }

    public class JobDetail
    {
        [JsonProperty("text")]
        public string Text { get; set; }
    }

    public class Schedule
    {
        [JsonProperty("date")]
        public string Date { get; set; }

        [JsonProperty("time")]
        public string Time { get; set; }

        [JsonProperty("user_identity")]
        public string UserIdentity { get; set; }

        [JsonProperty("email_address")]
        public string EmailAddress { get; set; }
    }

    public class RequiredBy
    {
        [JsonProperty("date")]
        public string Date { get; set; }

        [JsonProperty("time")]
        public string Time { get; set; }
    }

    public class Planned
    {
        [JsonProperty("date")]
        public DateTime? Date { get; set; }

        [JsonProperty("time")]
        public DateTime? Time { get; set; }
    }

}

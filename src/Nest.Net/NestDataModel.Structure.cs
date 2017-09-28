using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nest.Net
{
    public partial class Structure
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("country_code")]
        public string CountryCode { get; set; }

        [JsonProperty("cameras")]
        public string[] Cameras { get; set; }

        [JsonProperty("away")]
        public string Away { get; set; }

        [JsonProperty("co_alarm_state")]
        public string CoAlarmState { get; set; }

        [JsonProperty("eta")]
        public Eta Eta { get; set; }

        [JsonProperty("devices")]
        public OtherDevices Devices { get; set; }

        [JsonProperty("eta_begin")]
        public string EtaBegin { get; set; }

        [JsonProperty("rhr_enrollment")]
        public bool RhrEnrollment { get; set; }

        [JsonProperty("peak_period_start_time")]
        public string PeakPeriodStartTime { get; set; }

        [JsonProperty("peak_period_end_time")]
        public string PeakPeriodEndTime { get; set; }

        [JsonProperty("postal_code")]
        public string PostalCode { get; set; }

        [JsonProperty("smoke_co_alarms")]
        public string[] SmokeCoAlarms { get; set; }

        [JsonProperty("thermostats")]
        public string[] Thermostats { get; set; }

        [JsonProperty("smoke_alarm_state")]
        public string SmokeAlarmState { get; set; }

        [JsonProperty("structure_id")]
        public string StructureId { get; set; }

        [JsonProperty("time_zone")]
        public string TimeZone { get; set; }

        [JsonProperty("wheres")]
        public Dictionary<string, Where> Wheres { get; set; }
    }

    public partial class Eta
    {
        [JsonProperty("estimated_arrival_window_end")]
        public string EstimatedArrivalWindowEnd { get; set; }

        [JsonProperty("estimated_arrival_window_begin")]
        public string EstimatedArrivalWindowBegin { get; set; }

        [JsonProperty("trip_id")]
        public string TripId { get; set; }
    }

    public partial class OtherDevices
    {
        [JsonProperty("$company")]
        public OtherCompany Company { get; set; }
    }

    public partial class OtherCompany
    {
        [JsonProperty("$product_type")]
        public string[] ProductType { get; set; }
    }

    public partial class Where
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("where_id")]
        public string WhereId { get; set; }
    }
}

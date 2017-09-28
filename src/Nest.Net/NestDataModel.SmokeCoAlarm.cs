using Newtonsoft.Json;

namespace Nest.Net
{
    public partial class SmokeCoAlarm
    {
        [JsonProperty("locale")]
        public string Locale { get; set; }

        [JsonProperty("is_manual_test_active")]
        public bool IsManualTestActive { get; set; }

        [JsonProperty("co_alarm_state")]
        public string CoAlarmState { get; set; }

        [JsonProperty("battery_health")]
        public string BatteryHealth { get; set; }

        [JsonProperty("device_id")]
        public string DeviceId { get; set; }

        [JsonProperty("last_connection")]
        public string LastConnection { get; set; }

        [JsonProperty("is_online")]
        public bool IsOnline { get; set; }

        [JsonProperty("last_manual_test_time")]
        public string LastManualTestTime { get; set; }

        [JsonProperty("software_version")]
        public string SoftwareVersion { get; set; }

        [JsonProperty("name_long")]
        public string NameLong { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("smoke_alarm_state")]
        public string SmokeAlarmState { get; set; }

        [JsonProperty("ui_color_state")]
        public string UiColorState { get; set; }

        [JsonProperty("structure_id")]
        public string StructureId { get; set; }

        [JsonProperty("where_id")]
        public string WhereId { get; set; }

        [JsonProperty("where_name")]
        public string WhereName { get; set; }
    }
}

using Newtonsoft.Json;
using System.Collections.Generic;

namespace Nest.Net
{
    public partial class NestDataModel
    {
        [JsonProperty("metadata")]
        public Metadata Metadata { get; set; }

        [JsonProperty("devices")]
        public Devices Devices { get; set; }

        [JsonProperty("structures")]
        public Dictionary<string, Structure> Structures { get; set; }
    }

    public partial class Metadata
    {
        [JsonProperty("client_version")]
        public long ClientVersion { get; set; }

        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("user_id")]
        public string UserId { get; set; }
    }

    public partial class Devices
    {
        [JsonProperty("thermostats")]
        public Dictionary<string, Thermostat> Thermostats { get; set; }

        [JsonProperty("smoke_co_alarms")]
        public Dictionary<string, SmokeCoAlarm> SmokeCoAlarms { get; set; }

        [JsonProperty("cameras")]
        public Dictionary<string, Camera> Cameras { get; set; }

        [JsonProperty("$company")]
        public Company Company { get; set; }
    }
}

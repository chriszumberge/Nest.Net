using Newtonsoft.Json;
using System.Collections.Generic;

namespace Nest.Net
{
    public partial class Company
    {
        [JsonProperty("$product_type")]
        public ProductType ProductType { get; set; }
    }

    public partial class ProductType
    {
        [JsonProperty("$product_type")]
        public Dictionary<string, Product> Product { get; set; }
    }

    public partial class Product
    {
        [JsonProperty("identification")]
        public Identification Identification { get; set; }

        [JsonProperty("resource_use")]
        public ResourceUse ResourceUse { get; set; }

        [JsonProperty("events")]
        public Events Events { get; set; }

        [JsonProperty("location")]
        public Location Location { get; set; }

        [JsonProperty("sensors")]
        public Sensors Sensors { get; set; }

        [JsonProperty("software")]
        public Software Software { get; set; }
    }

    public partial class Identification
    {
        [JsonProperty("device_id")]
        public string DeviceId { get; set; }

        [JsonProperty("serial_number")]
        public string SerialNumber { get; set; }
    }

    public partial class ResourceUse
    {
        [JsonProperty("gas")]
        public Electricity Gas { get; set; }

        [JsonProperty("electricity")]
        public Electricity Electricity { get; set; }

        [JsonProperty("water")]
        public Electricity Water { get; set; }
    }

    public partial class Electricity
    {
        [JsonProperty("measurement_time")]
        public string MeasurementTime { get; set; }

        [JsonProperty("measurement_reset_time")]
        public string MeasurementResetTime { get; set; }

        [JsonProperty("value")]
        public double Value { get; set; }
    }

    public partial class Events
    {
        [JsonProperty("device_interaction")]
        public ActorStateChange DeviceInteraction { get; set; }

        [JsonProperty("actor_state_change")]
        public ActorStateChange ActorStateChange { get; set; }

        [JsonProperty("observed_motion")]
        public ActorStateChange ObservedMotion { get; set; }

        [JsonProperty("occupancy_change")]
        public ActorStateChange OccupancyChange { get; set; }
    }

    public partial class ActorStateChange
    {
        [JsonProperty("actor")]
        public string Actor { get; set; }

        [JsonProperty("probability")]
        public double Probability { get; set; }

        [JsonProperty("action")]
        public string Action { get; set; }

        [JsonProperty("observed_time")]
        public string ObservedTime { get; set; }

        [JsonProperty("target")]
        public string Target { get; set; }
    }

    public partial class Location
    {
        [JsonProperty("structure_id")]
        public string StructureId { get; set; }

        [JsonProperty("where_id")]
        public string WhereId { get; set; }
    }

    public partial class Sensors
    {
        [JsonProperty("humidity")]
        public Humidity Humidity { get; set; }

        [JsonProperty("pressure")]
        public Humidity Pressure { get; set; }

        [JsonProperty("ambient_light")]
        public AmbientLight AmbientLight { get; set; }

        [JsonProperty("passive_ir")]
        public PassiveIr PassiveIr { get; set; }

        [JsonProperty("temperature")]
        public Humidity Temperature { get; set; }
    }

    public partial class Humidity
    {
        [JsonProperty("observed_time")]
        public string ObservedTime { get; set; }

        [JsonProperty("observed_value")]
        public double ObservedValue { get; set; }
    }

    public partial class AmbientLight
    {
        [JsonProperty("observed_end_time")]
        public string ObservedEndTime { get; set; }

        [JsonProperty("observed_begin_time")]
        public string ObservedBeginTime { get; set; }

        [JsonProperty("observed_value_max")]
        public double ObservedValueMax { get; set; }

        [JsonProperty("observed_value_min")]
        public double ObservedValueMin { get; set; }
    }

    public partial class PassiveIr
    {
        [JsonProperty("observed_end_time")]
        public string ObservedEndTime { get; set; }

        [JsonProperty("observed_begin_time")]
        public string ObservedBeginTime { get; set; }

        [JsonProperty("observed_value_max")]
        public long ObservedValueMax { get; set; }

        [JsonProperty("observed_value_min")]
        public long ObservedValueMin { get; set; }
    }

    public partial class Software
    {
        [JsonProperty("version")]
        public string Version { get; set; }
    }
}

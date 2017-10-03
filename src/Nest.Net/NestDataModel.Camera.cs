using Newtonsoft.Json;
using System;

namespace Nest.Net
{
    public partial class Camera
    {
        [JsonProperty("is_video_history_enabled")]
        public bool IsVideoHistoryEnabled { get; set; }

        [JsonProperty("is_audio_input_enabled")]
        public bool IsAudioInputEnabled { get; set; }

        [JsonProperty("app_url")]
        public string AppUrl { get; set; }

        [JsonProperty("activity_zones")]
        public ActivityZone[] ActivityZones { get; set; }

        [JsonProperty("device_id")]
        public string DeviceId { get; set; }

        [JsonProperty("is_public_share_enabled")]
        public bool IsPublicShareEnabled { get; set; }

        [JsonProperty("is_online")]
        public bool IsOnline { get; set; }

        [JsonProperty("is_streaming")]
        public bool IsStreaming { get; set; }

        [JsonProperty("name_long")]
        public string NameLong { get; set; }

        [JsonProperty("structure_id")]
        public string StructureId { get; set; }

        [JsonProperty("last_is_online_change")]
        public string LastIsOnlineChange { get; set; }

        [JsonProperty("last_event")]
        public LastEvent LastEvent { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("snapshot_url")]
        public string SnapshotUrl { get; set; }

        [JsonProperty("public_share_url")]
        public string PublicShareUrl { get; set; }

        [JsonProperty("software_version")]
        public string SoftwareVersion { get; set; }

        [JsonProperty("where_id")]
        public string WhereId { get; set; }

        [JsonProperty("web_url")]
        public string WebUrl { get; set; }

        [JsonProperty("where_name")]
        public string WhereName { get; set; }
    }

    public partial class ActivityZone
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public partial class LastEvent
    {
        [JsonProperty("end_time")]
        public DateTime EndTime { get; set; }

        [JsonProperty("image_url")]
        public string ImageUrl { get; set; }

        [JsonProperty("animated_image_url")]
        public string AnimatedImageUrl { get; set; }

        [JsonProperty("activity_zone_ids")]
        public string[] ActivityZoneIds { get; set; }

        [JsonProperty("app_url")]
        public string AppUrl { get; set; }

        [JsonProperty("has_person")]
        public bool HasPerson { get; set; }

        [JsonProperty("has_motion")]
        public bool HasMotion { get; set; }

        [JsonProperty("has_sound")]
        public bool HasSound { get; set; }

        [JsonProperty("urls_expire_time")]
        public string UrlsExpireTime { get; set; }

        [JsonProperty("start_time")]
        public DateTime StartTime { get; set; }

        [JsonProperty("web_url")]
        public string WebUrl { get; set; }
    }
}

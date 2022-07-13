using Newtonsoft.Json;

namespace MediaService
{
    public record struct MediaCreateParams
    {
        [JsonProperty("upload", NullValueHandling = NullValueHandling.Ignore)]
        public MediaUpload? Upload { get; set; }
        [JsonProperty("metadata", NullValueHandling = NullValueHandling.Ignore)]
        public MediaMetadata? Metadata { get; set; }
        [JsonProperty("relationships", NullValueHandling = NullValueHandling.Ignore)]
        public MediaRelationships? Relationships { get; set; }
    }
}

using Newtonsoft.Json;

namespace MediaService
{
    public record struct MediaUpdateParams
    {
        [JsonProperty("metadata", NullValueHandling = NullValueHandling.Ignore)]
        public MediaMetadata? Metadata { get; set; }
        [JsonProperty("relationships", NullValueHandling = NullValueHandling.Ignore)]
        public MediaRelationships? Relationships { get; set; }
    }
}

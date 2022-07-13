using Newtonsoft.Json;

namespace MediaService
{
    public record struct MediaReuploadParams
    {
        [JsonProperty("upload", NullValueHandling = NullValueHandling.Ignore)]
        public MediaUpload Upload { get; set; }
    }
}

using Newtonsoft.Json;

namespace MediaService
{
    public record struct MediaUpload
    {
        /// <summary>
        /// Upload method <see cref="MediaUploadMethod"/>
        /// </summary>
        [JsonProperty("method")]
        public string Method { get; set; }

        /// <summary>
        /// MIME type <see cref="MediaUploadMimeType"/> for the uploaded media, required for the external upload method
        /// </summary>
        [JsonProperty("mime_type", NullValueHandling = NullValueHandling.Ignore)]
        public string MimeType { get; set; }

        /// <summary>
        /// URL of the external media, required for the external upload method
        /// </summary>
        [JsonProperty("source_url", NullValueHandling = NullValueHandling.Ignore)]
        public string SourceUrl { get; set; }

        /// <summary>
        /// URL of the media to fetch
        /// </summary>
        [JsonProperty("download_url", NullValueHandling = NullValueHandling.Ignore)]
        public string DownloadUrl { get; set; }

        /// <summary>
        /// Starting point to trim the video, not applicable to the external upload method
        /// </summary>
        [JsonProperty("trim_in_point", NullValueHandling = NullValueHandling.Ignore)]
        public string TrimInPoint { get; set; }

        /// <summary>
        /// Ending point to trim the video, not applicable to the external upload method
        /// </summary>
        [JsonProperty("trim_out_point", NullValueHandling = NullValueHandling.Ignore)]
        public string TrimOutPoint { get; set; }
    }
}

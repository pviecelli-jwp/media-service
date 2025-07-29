using System.ComponentModel;
using System.Text.Json.Serialization;

namespace JWPlayer.MediaApiService.Model;

public record struct MediaUpload
{
    /// <summary>
    /// Upload method <see cref="MediaUploadMethod"/>
    /// </summary>
    [JsonPropertyName("method"), DefaultValue("direct")]
    public MediaUploadMethod Method { get; set; }

    /// <summary>
    /// MIME type <see cref="MediaUploadMimeType"/> for the uploaded media, required for the external upload method
    /// </summary>
    [JsonPropertyName("mime_type"), DefaultValue("videomp4")]
    public MediaUploadMimeType? MimeType { get; set; }

    /// <summary>
    /// URL of the external media, required for the external upload method
    /// </summary>
    [JsonPropertyName("source_url")]
    public string? SourceUrl { get; set; }

    /// <summary>
    /// URL of the media to fetch
    /// </summary>
    [JsonPropertyName("download_url")]
    public string? DownloadUrl { get; set; }

    /// <summary>
    /// Starting point to trim the video, not applicable to the external upload method
    /// </summary>
    [JsonPropertyName("trim_in_point")]
    public DateTime? TrimInPoint { get; set; }

    /// <summary>
    /// Ending point to trim the video, not applicable to the external upload method
    /// </summary>
    [JsonPropertyName("trim_out_point")]
    public DateTime? TrimOutPoint { get; set; }
}

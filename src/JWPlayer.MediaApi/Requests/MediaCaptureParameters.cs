using JWPlayer.MediaApi.Utils;
using System.Text.Json.Serialization;

namespace JWPlayer.MediaApi.Requests;

public struct MediaCaptureParameters
{
    [JsonPropertyName("method")]
    public static string Method => "capture";
    [JsonPropertyName("capture_urls")]
    public string[]? CaptureUrls { get; set; }
    [JsonPropertyName("origin")]
    public string? Origin { get; set; }
    [JsonPropertyName("trim_in_point"), JsonConverter(typeof(CaptureDateTimeConverter))]
    public DateTime? TrimInPoint { get; set; }
    [JsonPropertyName("trim_out_point"), JsonConverter(typeof(CaptureDateTimeConverter))]
    public DateTime? TrimOutPoint { get; set; }
}

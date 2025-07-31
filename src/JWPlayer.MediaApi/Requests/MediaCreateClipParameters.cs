using JWPlayer.MediaApi.Model;
using System.Text.Json.Serialization;

namespace JWPlayer.MediaApi.Requests;

public struct MediaCreateClipParameters
{
    [JsonPropertyName("metadata")]
    public MediaMetadata? Metadata { get; set; }
    [JsonPropertyName("relationships")]
    public MediaCaptureParameters? CaptureParameters { get; set; }
}

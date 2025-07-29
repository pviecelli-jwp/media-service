using System.Text.Json.Serialization;

namespace JWPlayer.MediaApiService.Model;

public record struct MediaReuploadParams
{
    [JsonPropertyName("upload")]
    public MediaUpload Upload { get; set; }
}

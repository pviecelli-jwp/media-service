using System.Text.Json.Serialization;

namespace JWPlayer.MediaApiService.Model;

public record struct MediaReuploadParams
{
    [JsonPropertyName("upload"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public MediaUpload Upload { get; set; }
}

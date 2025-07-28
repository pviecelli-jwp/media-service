using System.Text.Json.Serialization;

namespace MediaService.Model;

public record struct MediaReuploadParams
{
    [JsonPropertyName("upload"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public MediaUpload Upload { get; set; }
}

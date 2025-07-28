using System.Text.Json.Serialization;

namespace MediaService.Model;

public record struct MediaCreateParams
{
    [JsonPropertyName("upload"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public MediaUpload? Upload { get; set; }
    [JsonPropertyName("metadata"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public MediaMetadata? Metadata { get; set; }
    [JsonPropertyName("relationships"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public MediaRelationships? Relationships { get; set; }
}

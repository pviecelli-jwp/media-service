using System.Text.Json.Serialization;

namespace JWPlayer.MediaApiService.Model;

public record struct MediaUpdateParams
{
    [JsonPropertyName("metadata"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public MediaMetadata? Metadata { get; set; }
    [JsonPropertyName("relationships"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public MediaRelationships? Relationships { get; set; }
}

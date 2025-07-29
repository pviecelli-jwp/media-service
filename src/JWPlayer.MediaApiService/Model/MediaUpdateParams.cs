using System.Text.Json.Serialization;

namespace JWPlayer.MediaApiService.Model;

public record struct MediaUpdateParams
{
    [JsonPropertyName("metadata")]
    public MediaMetadata? Metadata { get; set; }
    [JsonPropertyName("relationships")]
    public MediaRelationships? Relationships { get; set; }
}

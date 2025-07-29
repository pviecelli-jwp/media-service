using System.Text.Json.Serialization;

namespace JWPlayer.MediaApiService.Model;

public record struct MediaCreateParams
{
    [JsonPropertyName("upload")]
    public MediaUpload? Upload { get; set; }
    [JsonPropertyName("metadata")]
    public MediaMetadata? Metadata { get; set; }
    [JsonPropertyName("relationships")]
    public MediaRelationships? Relationships { get; set; }
}

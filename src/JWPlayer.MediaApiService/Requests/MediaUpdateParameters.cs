using JWPlayer.ApiGateway.Schemas;
using JWPlayer.MediaApiService.Model;
using System.Text.Json.Serialization;

namespace JWPlayer.MediaApiService.Requests;

public struct MediaUpdateParameters
{
    [JsonPropertyName("metadata")]
    public MediaMetadata? Metadata { get; set; }
    [JsonPropertyName("relationships")]
    public RelationshipDictionary? Relationships { get; set; }
}

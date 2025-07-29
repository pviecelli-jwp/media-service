using JWPlayer.ApiGateway.Schemas;
using JWPlayer.MediaApi.Model;
using System.Text.Json.Serialization;

namespace JWPlayer.MediaApi.Requests;

public struct MediaCreateParameters
{
    [JsonPropertyName("upload")]
    public MediaUpload? Upload { get; set; }
    [JsonPropertyName("metadata")]
    public MediaMetadata? Metadata { get; set; }
    [JsonPropertyName("relationships")]
    public RelationshipDictionary? Relationships { get; set; }
}

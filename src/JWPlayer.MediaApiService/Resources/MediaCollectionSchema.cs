using JWPlayer.ApiGateway.Schemas;
using System.Text.Json.Serialization;

namespace JWPlayer.MediaApiService.Resources;
public class MediaCollectionSchema : CollectionSchema<MediaObjectSchema>
{
    [JsonPropertyName("media")]
    public override required IEnumerable<MediaObjectSchema> Data { get; init; }
}

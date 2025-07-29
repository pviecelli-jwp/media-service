using System.Text.Json.Serialization;

namespace JWPlayer.MediaApiService.Model;

public class MediaRelationshipId(string id)
{
    [JsonPropertyName("id")]
    public string Id = id;
}

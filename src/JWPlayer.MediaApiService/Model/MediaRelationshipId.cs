using System.Text.Json.Serialization;

namespace JWPlayer.MediaApiService.Model;

public class MediaRelationshipId(string id)
{
    [JsonPropertyName("id"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string Id = id;
}

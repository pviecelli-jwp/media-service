using System.Text.Json.Serialization;

namespace MediaService.Model;

public class MediaRelationshipId(string id)
{
    [JsonPropertyName("id"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string Id = id;
}

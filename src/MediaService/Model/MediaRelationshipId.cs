using System.Text.Json.Serialization;

namespace MediaService.Model;

public class MediaRelationshipId
{
    [JsonPropertyName("id"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string Id;

    public MediaRelationshipId(string id)
    {
        Id = id;
    }
}

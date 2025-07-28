using System.Text.Json.Serialization;

namespace MediaService.Model;

public class MediaRelationships
{
    [JsonPropertyName("protection_rule"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public MediaRelationshipId ProtectionRule;

    public MediaRelationships(string protectionRuleId)
    {
        ProtectionRule = new MediaRelationshipId(protectionRuleId);
    }
}

using System.Text.Json.Serialization;

namespace MediaService.Model;

public class MediaRelationships(string protectionRuleId)
{
    [JsonPropertyName("protection_rule"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public MediaRelationshipId ProtectionRule = new(protectionRuleId);
}

using System.Text.Json.Serialization;

namespace JWPlayer.MediaApiService.Model;

public class MediaRelationships(string protectionRuleId)
{
    [JsonPropertyName("protection_rule")]
    public MediaRelationshipId ProtectionRule = new(protectionRuleId);
}

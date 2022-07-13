using Newtonsoft.Json;

namespace MediaService
{
    public class MediaRelationships
    {
        [JsonProperty("protection_rule", NullValueHandling = NullValueHandling.Ignore)]
        public MediaRelationshipId ProtectionRule;

        public MediaRelationships(string protectionRuleId)
        {
            ProtectionRule = new MediaRelationshipId(protectionRuleId);
        }
    }
}

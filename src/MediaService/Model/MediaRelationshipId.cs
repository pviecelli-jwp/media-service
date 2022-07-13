using Newtonsoft.Json;

namespace MediaService
{
    public class MediaRelationshipId
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id;

        public MediaRelationshipId(string id)
        {
            Id = id;
        }
    }
}

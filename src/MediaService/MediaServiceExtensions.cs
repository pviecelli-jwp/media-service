using JWPlayer.Identity;
using Newtonsoft.Json.Linq;

namespace MediaService
{
    public static class MediaServiceExtensions
    {
        public static Alpha GetMediaId(this IRestResponse response)
        {
            var content = JObject.Parse(response.Content);
            var mediaId = content.GetValue("id").ToString();
            return new Alpha(mediaId);
        }
    }
}

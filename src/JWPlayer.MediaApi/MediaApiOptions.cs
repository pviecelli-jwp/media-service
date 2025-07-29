namespace JWPlayer.MediaApi
{
    public class MediaApiOptions
    {
        public string BaseUrl { get; set; } = "https://api.jwplayer.com/";
        public string SourceUrl { get; set; } = "https://content.jwplatform.com/live/broadcast/{media_id}.m3u8";
    }
}
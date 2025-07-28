namespace MediaService
{
    public class MediaOptions
    {
        public string BaseUrl { get; set; } = "https://api.jwplayer.com/";
        public string SourceUrl { get; set; } = "https://content.jwplatform.com/live/broadcast/{media_id}.m3u8";
    }
}
namespace JWPlayer.MediaApi
{
    public class MediaApiOptions
    {
        public string BaseUrl { get; set; } = "https://api.jwplayer.com/";
        public string SetDrmEndpointBaseUrl { get; set; } = "https://api.jwplayer.com";
        public string SetDrmEndpointSecret { get; set; } = "";
    }
}
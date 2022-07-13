namespace MediaService
{
    public class MediaOptions
    {
        public string AuthSecretsSection { get; set; } = "ldap_entity";
        public string AuthsvcGrpcChannel { get; set; } = "https://authsvc.longtailvideo.com:443";
        public string CreateUrl { get; set; } = "https://api.jwplayer.com/v2/sites/{0}/media/";
        public string ActionUrl { get; set; } = "https://api.jwplayer.com/v2/sites/{0}/media/{1}/";
    }
}
using Grpc.Net.Client;
using JWPlayer.AuthSvc;
using JWPlayer.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace MediaService
{
    public class MediaService : IMediaService
    {
        protected readonly string? ApiKey;
        protected readonly string CreateUrl;
        protected readonly string ActionUrl;
        protected readonly GrpcChannel GrpcChannel;
        protected readonly AuthService AuthSvc;
        protected readonly MediaOptions MediaOptions = new();
        protected string AuthToken;

        private readonly ILogger<MediaService> Logger;
        private const string MediaSection = "Media";
        private const string UserKey = "user";
        private const string PasswordKey = "password";

        public MediaService(IConfiguration configuration, ILogger<MediaService> logger)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));

            if (configuration is null) throw new ArgumentNullException(nameof(configuration));

            configuration.GetSection(MediaSection).Bind(MediaOptions);

            GrpcChannel = GrpcChannel.ForAddress(MediaOptions.AuthsvcGrpcChannel);
            AuthSvc = new AuthService(GrpcChannel);

            var secrets = configuration.GetSection(MediaOptions.AuthSecretsSection);
            ApiKey = secrets[nameof(ApiKey).ToLower()];
            AuthToken = (ApiKey is not null)
                ? GetAuthToken(AuthSvc, ApiKey)
                : GetAuthToken(AuthSvc, secrets[PasswordKey], secrets[UserKey]);

            CreateUrl = MediaOptions.CreateUrl;
            ActionUrl = MediaOptions.ActionUrl;
        }

        public async Task<IRestResponse> CreateAsync(Alpha siteId, string metadata)
        {
            var client = new RestClient(String.Format(CreateUrl, siteId));
            var body = CreateMediaParams(metadata);
            var request = CreateRestRequest(Method.POST);
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            return await client.ExecuteTaskAsync(request);
        }

        public void Delete(Alpha siteId, MediaId mediaId)
        {
            var client = new RestClient(String.Format(ActionUrl, siteId, mediaId));
            var request = CreateRestRequest(Method.DELETE);
            client.ExecuteTaskAsync(request);
        }

        public void Reupload(Alpha siteId, MediaId mediaId, MediaUpload upload)
        {
            var clientUrl = String.Format(ActionUrl, siteId, mediaId) + "reupload/";
            var client = new RestClient(clientUrl);

            var mediaReupload = new MediaReuploadParams
            {
                Upload = upload
            };
            var body = JsonConvert.SerializeObject(mediaReupload);
            var request = CreateRestRequest(Method.PUT);
            request.AddParameter("application/json", body, ParameterType.RequestBody);

            client.ExecuteTaskAsync(request);
        }

        private static string GetAuthToken(AuthService authSvc, string secret, string? user = null)
        {
            var authResult = (user is null)
                ? authSvc.AuthenticateApiKey(secret)
                : authSvc.LoginUser(user, secret);
            if (authResult is AuthSuccess success) return success.Jwt;
            throw new Exception("Authentication failed");
        }

        private static string CreateMediaParams(string metadata)
        {
            //TODO: This code probably needs improvement

            var jsonMetadata = new JObject();

            try
            {
                jsonMetadata = JObject.Parse(metadata);
            }
            catch (Newtonsoft.Json.JsonReaderException)
            {
                jsonMetadata["metadata"] = GetDefaultMetadataJObject();
                return jsonMetadata.ToString();
            };

            if (jsonMetadata.ContainsKey("metadata"))
            {
                if (jsonMetadata["metadata"]["title"] == null || jsonMetadata["metadata"]["title"].ToString() == string.Empty)
                {
                    jsonMetadata["metadata"]["title"] = DateTime.UtcNow.ToString();
                }
            }
            else
            {
                jsonMetadata["metadata"] = GetDefaultMetadataJObject();
            }
            return jsonMetadata.ToString();
        }

        private RestRequest CreateRestRequest(Method method)
        {
            var request = new RestRequest(method);
            request.AddHeader("Accept", "application/json");
            if (method is not Method.DELETE) request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", $"Bearer {AuthToken}");

            return request;
        }

        private static JObject GetDefaultMetadataJObject()
        {
            return JObject.FromObject(new
            {
                title = DateTime.UtcNow.ToString()
            });
        }
    }
}

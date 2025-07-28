using JWPlayer.ApiGateway.Services;
using JWPlayer.ApiGateway.Services.Abstractions;
using JWPlayer.Identity;
using JWPlayer.Outcomes;
using MediaService.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RestSharp;
using System.Text.Json;
using System.Text.Json.Nodes;
using JsonObject = System.Text.Json.Nodes.JsonObject;

namespace MediaService;

public class MediaService : IMediaService
{
    internal static Endpoint CreateBroadcastLiveMedia(Alpha siteId) => new($"internal/v2/sites/{siteId}/live_broadcast/", Method.PUT);
    internal static Endpoint CreateMedia(Alpha siteId) => new($"v2/sites/{siteId}/media/", Method.POST);
    internal static Endpoint GetMedia(Alpha siteId, Alpha mediaId) => new($"v2/sites/{siteId}/media/{mediaId}/", Method.GET);
    internal static Endpoint DeleteMedia(Alpha siteId, Alpha mediaId) => new($"v2/sites/{siteId}/media/{mediaId}/", Method.DELETE);
    internal static Endpoint EnableDrmOnExternalMedia(Alpha siteId, Alpha mediaId) => new($"v2/sites/{siteId}/media/{mediaId}/enable_drm_on_external_media", Method.PUT);
    internal static Endpoint IsDrmEnabled(Alpha siteId) => new($"v2/sites/{siteId}/is_drm_enabled", Method.GET);
    internal static Endpoint GetDrmPolicies(Alpha siteId) => new($"v2/sites/{siteId}/drm_policies/", Method.GET);
    internal static Endpoint CaptureMedia(Alpha siteId, Alpha mediaId) => new($"internal/v2/sites/{siteId}/media/{mediaId}/live_broadcast_capture/", Method.PUT);
    internal static Endpoint UpdateMedia(Alpha siteId, Alpha mediaId) => new($"v2/sites/{siteId}/media/{mediaId}/", Method.PATCH);
    internal static Endpoint CreateClip(Alpha siteId) => new($"internal/v2/sites/{siteId}/live_broadcast_clip/", Method.POST);

    private readonly ILogger<MediaService> _logger;
    private readonly IRestClientFactory _restClientFactory;
    private readonly IAuthTokenFactory _tokenFactory;
    protected readonly MediaOptions _mediaOptions;

    private readonly string _baseUrl;
    private readonly string _sourceUrl;

    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        AllowTrailingCommas = true,
    };

    public MediaService(
        IConfiguration configuration,
        ILogger<MediaService> logger,
        IOptions<MediaOptions> options,
        IRestClientFactory restClientFactory,
        IAuthTokenFactory tokenFactory)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _restClientFactory = restClientFactory ?? throw new ArgumentNullException(nameof(restClientFactory));
        _tokenFactory = tokenFactory ?? throw new ArgumentNullException(nameof(tokenFactory));
        _mediaOptions = options?.Value ?? throw new ArgumentNullException(nameof(options));

        _baseUrl = _mediaOptions.BaseUrl;
        _sourceUrl = _mediaOptions.SourceUrl;
    }

    public async Task<IRestResponse> CreateAsync(Alpha siteId, string metadata)
    {
        var client = _restClientFactory.Create(_baseUrl);
        var body = CreateMediaParams(metadata);
        var request = await CreateRestRequestAsync(CreateMedia(siteId));
        request.AddParameter("application/json", body, ParameterType.RequestBody);
        return await client.ExecuteAsync(request);
    }

    public async Task Delete(Alpha siteId, Alpha mediaId)
    {
        var client = _restClientFactory.Create(_baseUrl);
        var request = await CreateRestRequestAsync(DeleteMedia(siteId, mediaId));
        await client.ExecuteAsync(request);
    }

    public async Task Reupload(Alpha siteId, Alpha mediaId, MediaUpload upload)
    {
        throw new NotImplementedException();
        //var clientUrl = String.Format(ActionUrl, siteId, mediaId) + "reupload/";
        //var client = new RestClient(clientUrl);

        //var mediaReupload = new MediaReuploadParams
        //{
        //    Upload = upload
        //};
        //var body = JsonConvert.SerializeObject(mediaReupload);
        //var request = await CreateRestRequestAsync(Method.PUT);
        //request.AddParameter("application/json", body, ParameterType.RequestBody);

        //client.ExecuteAsync(request);
    }

    private static string CreateMediaParams(string metadata)
    {
        //TODO: This code probably needs improvement

        JsonObject jsonMetadata;

        try
        {
            jsonMetadata = JsonNode.Parse(metadata)?.AsObject() ?? [];
        }
        catch (JsonException)
        {
            jsonMetadata = new JsonObject { ["metadata"] = GetDefaultMetadataJObject() };
            return jsonMetadata.ToString();
        };

        if (jsonMetadata["metadata"] != null)
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

    private async Task<RestRequest> CreateRestRequestAsync(Endpoint endpoint)
    {
        var tokenResult = await _tokenFactory.GetAuthTokenAsync();
        if (tokenResult.IsErr)
            _logger.LogError(tokenResult.GetError(), "Failed to get Auth Token");
        var token = tokenResult.GetValueOrDefault(string.Empty);
        var request = new RestRequest(endpoint.Resource, endpoint.Method);
        request.AddHeader("Accept", "application/json");
        if (request.Method is not Method.DELETE) request.AddHeader("Content-Type", "application/json");
        request.AddHeader("Authorization", $"Bearer {token}");
        return request;
    }

    private static JsonObject GetDefaultMetadataJObject() =>
        new()
        {
            ["title"] = DateTime.UtcNow.ToString()
        };
}

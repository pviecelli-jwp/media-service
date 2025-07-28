using JWPlayer.ApiGateway.Services;
using JWPlayer.ApiGateway.Services.Abstractions;
using JWPlayer.Identity;
using JWPlayer.Outcomes;
using MediaService.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RestSharp;
using System.Text.Json;

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
    internal static Endpoint ReuploadMedia(Alpha siteId, Alpha mediaId) => new($"v2/sites/{siteId}/media/{mediaId}/reupload/", Method.PUT);

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

    public async Task<IRestResponse> CreateAsync(Alpha siteId, MediaCreateParams mediaCreateParams)
    {
        var client = _restClientFactory.Create(_baseUrl);
        var body = CreateMediaParams(mediaCreateParams);
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

    public async Task<IRestResponse> Reupload(Alpha siteId, Alpha mediaId, MediaUpload upload)
    {
        var client = _restClientFactory.Create(_baseUrl);
        var request = await CreateRestRequestAsync(ReuploadMedia(siteId, mediaId));
        var body = JsonSerializer.Serialize(upload, _jsonOptions);
        request.AddParameter("application/json", body, ParameterType.RequestBody);
        return await client.ExecuteAsync(request);
    }

    private string CreateMediaParams(MediaCreateParams mediaCreateParams)
    {
        if (mediaCreateParams.Metadata != null)
        {
            if (mediaCreateParams.Metadata.Value.Title == null || mediaCreateParams.Metadata.Value.Title == string.Empty)
                mediaCreateParams.Metadata = mediaCreateParams.Metadata.Value with { Title = DateTime.UtcNow.ToString() };
        }
        else
            mediaCreateParams.Metadata = GetDefaultMetadataJObject();

        return JsonSerializer.Serialize(mediaCreateParams, _jsonOptions);
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

    private static MediaMetadata GetDefaultMetadataJObject() =>
        new()
        {
            Title = DateTime.UtcNow.ToString()
        };
}

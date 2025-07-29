using JWPlayer.ApiGateway.Errors;
using JWPlayer.ApiGateway.Services;
using JWPlayer.ApiGateway.Services.Abstractions;
using JWPlayer.Identity;
using JWPlayer.MediaApiService.Model;
using JWPlayer.Outcomes;
using Microsoft.Extensions.Options;
using RestSharp;
using System.Text.Json;

namespace JWPlayer.MediaApiService;

public class MediaApiService : IMediaApiService
{
    internal static Endpoint CreateBroadcastLiveMedia(Alpha siteId) => new($"internal/v2/sites/{siteId}/live_broadcast/", Method.PUT);
    internal static Endpoint CreateMedia(Alpha siteId) => new($"v2/sites/{siteId}/media/", Method.POST);
    internal static Endpoint GetMedia(Alpha siteId, Alpha mediaId) => new($"v2/sites/{siteId}/media/{mediaId}/", Method.GET);
    internal static Endpoint GetAllMedia(Alpha siteId) => new($"v2/sites/{siteId}/media/", Method.GET);
    internal static Endpoint DeleteMedia(Alpha siteId, Alpha mediaId) => new($"v2/sites/{siteId}/media/{mediaId}/", Method.DELETE);
    internal static Endpoint EnableDrmOnExternalMedia(Alpha siteId, Alpha mediaId) => new($"v2/sites/{siteId}/media/{mediaId}/enable_drm_on_external_media", Method.PUT);
    internal static Endpoint IsDrmEnabled(Alpha siteId) => new($"v2/sites/{siteId}/is_drm_enabled", Method.GET);
    internal static Endpoint GetDrmPolicies(Alpha siteId) => new($"v2/sites/{siteId}/drm_policies/", Method.GET);
    internal static Endpoint CaptureMedia(Alpha siteId, Alpha mediaId) => new($"internal/v2/sites/{siteId}/media/{mediaId}/live_broadcast_capture/", Method.PUT);
    internal static Endpoint UpdateMedia(Alpha siteId, Alpha mediaId) => new($"v2/sites/{siteId}/media/{mediaId}/", Method.PATCH);
    internal static Endpoint CreateClip(Alpha siteId) => new($"internal/v2/sites/{siteId}/live_broadcast_clip/", Method.POST);
    internal static Endpoint ReuploadMedia(Alpha siteId, Alpha mediaId) => new($"v2/sites/{siteId}/media/{mediaId}/reupload/", Method.PUT);

    private readonly IRestClientFactory _restClientFactory;
    private readonly IAuthTokenFactory _tokenFactory;
    protected readonly MediaApiOptions _mediaOptions;

    private readonly string _baseUrl;

    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        AllowTrailingCommas = true,
    };

    public MediaApiService(
        IOptions<MediaApiOptions> options,
        IRestClientFactory restClientFactory,
        IAuthTokenFactory tokenFactory)
    {
        _restClientFactory = restClientFactory ?? throw new ArgumentNullException(nameof(restClientFactory));
        _tokenFactory = tokenFactory ?? throw new ArgumentNullException(nameof(tokenFactory));
        _mediaOptions = options?.Value ?? throw new ArgumentNullException(nameof(options));

        _baseUrl = _mediaOptions.BaseUrl;
    }

    public async Task<Result<MediaObjectSchema, JwErrorResponse>> CreateMediaAsync(Alpha siteId, MediaCreateParams mediaCreateParams)
    {
        const string RequestDescription = "Creating Media";
        var client = _restClientFactory.Create(_baseUrl);
        var body = JsonSerializer.Serialize(mediaCreateParams, _jsonOptions);
        var request = await CreateRestRequestAsync(CreateMedia(siteId));

        request.AddParameter("application/json", body, ParameterType.RequestBody);

        var response = await client.ExecuteAsync(request);

        return DeserializeOrError<MediaObjectSchema>(response, RequestDescription);
    }

    public async Task<Result<MediaObjectSchema, JwErrorResponse>> GetMediaAsync(Alpha siteId, Alpha mediaId)
    {
        const string RequestDescription = "Get Media";
        var client = _restClientFactory.Create(_baseUrl);
        var request = await CreateRestRequestAsync(GetMedia(siteId, mediaId));

        var response = await client.ExecuteAsync(request);

        return DeserializeOrError<MediaObjectSchema>(response, RequestDescription);
    }

    public async Task<Result<MediaCollectionSchema, JwErrorResponse>> GetAllMediasAsync(Alpha siteId, int? page, int? pageLength, string? q, string? sort)
    {
        const string RequestDescription = "Get All Media";
        var client = _restClientFactory.Create(_baseUrl);
        var request = await CreateRestRequestAsync(GetAllMedia(siteId));

        var response = await client.ExecuteAsync(request);

        return DeserializeOrError<MediaCollectionSchema>(response, RequestDescription);
    }

    public async Task<Result<MediaObjectSchema, JwErrorResponse>> UpdateMediaAsync(Alpha siteId, Alpha mediaId, MediaUpdateParams mediaUpdateParams)
    {
        const string RequestDescription = "Update Media";
        var client = _restClientFactory.Create(_baseUrl);
        var body = JsonSerializer.Serialize(mediaUpdateParams, _jsonOptions);
        var request = await CreateRestRequestAsync(UpdateMedia(siteId, mediaId));

        request.AddParameter("application/json", body, ParameterType.RequestBody);

        var response = await client.ExecuteAsync(request);

        return DeserializeOrError<MediaObjectSchema>(response, RequestDescription);
    }


    public async Task<Result<JwErrorResponse>> DeleteMediaAsync(Alpha siteId, Alpha mediaId)
    {
        const string RequestDescription = "Delete Media";
        var client = _restClientFactory.Create(_baseUrl);
        var request = await CreateRestRequestAsync(DeleteMedia(siteId, mediaId));

        var response = await client.ExecuteAsync(request);

        return DeserializeOrError(response, RequestDescription);
    }

    public async Task<IRestResponse> ReuploadMediaAsync(Alpha siteId, Alpha mediaId, MediaUpload upload)
    {
        var client = _restClientFactory.Create(_baseUrl);
        var request = await CreateRestRequestAsync(ReuploadMedia(siteId, mediaId));
        var body = JsonSerializer.Serialize(upload, _jsonOptions);
        request.AddParameter("application/json", body, ParameterType.RequestBody);
        return await client.ExecuteAsync(request);
    }

    private async Task<RestRequest> CreateRestRequestAsync(Endpoint endpoint)
    {
        var tokenResult = await _tokenFactory.GetAuthTokenAsync();
        if (tokenResult.IsErr)
            throw new MediaApiServiceException("Failed to get Auth Token", tokenResult.GetError());

        var token = tokenResult.GetValueOrDefault(string.Empty);
        var request = new RestRequest(endpoint.Resource, endpoint.Method);
        request.AddHeader("Accept", "application/json");
        if (request.Method is not Method.DELETE) request.AddHeader("Content-Type", "application/json");
        request.AddHeader("Authorization", $"Bearer {token}");
        return request;
    }

    /// <summary>
    /// Helper method for deserializing a <paramref name="response"/> content, or producing an error using <see cref="Result{T, E}"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="response"></param>
    /// <param name="requestDesc">Used to generate appropriate logs, see additional remarks.
    /// <para> Trace: "{<paramref name="requestDesc"/>} was successful"</para>
    /// <para> Deserialization Error: "{<paramref name="requestDesc"/>}, status code {...}, response content {...}"</para>
    /// </param>
    private static Result<T, JwErrorResponse> DeserializeOrError<T>(IRestResponse response, string requestDesc)
    {
        if (response.IsSuccessful)
        {
            try
            {
                var obj = JsonSerializer.Deserialize<T>(response.Content);
                if (obj != null) return obj;
                throw new Exception(
                    $"Failed to deserialize {typeof(T).Name}, resulted in null for API response {response.Content}'");
            }
            catch (Exception ex)
            {
                return HandleError(response, requestDesc, ex);
            }
        }

        return HandleError(response, requestDesc);
    }

    private static Result<JwErrorResponse> DeserializeOrError(IRestResponse response, string requestDesc)
    {
        if (response.IsSuccessful)
            return Result.Ok<JwErrorResponse>();

        return HandleError(response, requestDesc);
    }

    /// <summary>
    /// Helper method for producing an error using <see cref="Result{T, E}"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="response"></param>
    /// <param name="requestDesc">Used to generate appropriate logs, see additional remarks.
    /// <para> Deserialization Error: "{<paramref name="requestDesc"/>}, status code {...}, response content {...}"</para>
    /// </param>
    /// <param name="exception">Exception to be logged, in case there is one.</param>
    private static JwErrorResponse HandleError(IRestResponse response, string requestDesc, Exception? exception = null)
    {
        var isJwError = response.Content.TryGetJwErrorResponse(out var err);
        if (!isJwError)
            throw new MediaApiServiceException($"Failed to deserialize error response for {requestDesc}, status code {response.StatusCode}, response content: {response.Content}", exception);

        return err;
    }
}

using JWPlayer.ApiGateway.Errors;
using JWPlayer.ApiGateway.Services;
using JWPlayer.ApiGateway.Services.Abstractions;
using JWPlayer.Identity;
using JWPlayer.MediaApi.Model;
using JWPlayer.MediaApi.Objects;
using JWPlayer.MediaApi.Requests;
using JWPlayer.Outcomes;
using Microsoft.Extensions.Options;
using RestSharp;
using System.Text.Json;

namespace JWPlayer.MediaApi.Resources;

public class MediaResource(
    IOptions<MediaApiOptions> options,
    IRestClientFactory restClientFactory,
    IAuthTokenFactory tokenFactory) : BaseResource(restClientFactory, tokenFactory, options), IMediaResource
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

    public async Task<Result<MediaObjectSchema, JwErrorResponse>> CreateMediaAsync(Alpha siteId, MediaCreateParameters mediaCreateParameters)
    {
        const string RequestDescription = "Creating Media";
        var client = CreateBaseRestClient();
        var body = JsonSerializer.Serialize(mediaCreateParameters, _jsonOptions);
        var request = await CreateRestRequestAsync(CreateMedia(siteId));

        request.AddParameter("application/json", body, ParameterType.RequestBody);

        var response = await client.ExecuteAsync(request);

        return DeserializeOrError<MediaObjectSchema>(response, RequestDescription);
    }

    public async Task<Result<MediaObjectSchema, JwErrorResponse>> GetMediaAsync(Alpha siteId, Alpha mediaId)
    {
        const string RequestDescription = "Get Media";
        var client = CreateBaseRestClient();
        var request = await CreateRestRequestAsync(GetMedia(siteId, mediaId));

        var response = await client.ExecuteAsync(request);

        return DeserializeOrError<MediaObjectSchema>(response, RequestDescription);
    }

    public async Task<Result<MediaCollectionSchema, JwErrorResponse>> GetAllMediasAsync(Alpha siteId, int? page, int? pageLength, string? q, string? sort)
    {
        const string RequestDescription = "Get All Media";
        var client = CreateBaseRestClient();
        var request = await CreateRestRequestAsync(GetAllMedia(siteId));

        var response = await client.ExecuteAsync(request);

        return DeserializeOrError<MediaCollectionSchema>(response, RequestDescription);
    }

    public async Task<Result<MediaObjectSchema, JwErrorResponse>> UpdateMediaAsync(Alpha siteId, Alpha mediaId, MediaUpdateParameters mediaUpdateParams)
    {
        const string RequestDescription = "Update Media";
        var client = CreateBaseRestClient();
        var body = JsonSerializer.Serialize(mediaUpdateParams, _jsonOptions);
        var request = await CreateRestRequestAsync(UpdateMedia(siteId, mediaId));

        request.AddParameter("application/json", body, ParameterType.RequestBody);

        var response = await client.ExecuteAsync(request);

        return DeserializeOrError<MediaObjectSchema>(response, RequestDescription);
    }


    public async Task<Result<JwErrorResponse>> DeleteMediaAsync(Alpha siteId, Alpha mediaId)
    {
        const string RequestDescription = "Delete Media";
        var client = CreateBaseRestClient();
        var request = await CreateRestRequestAsync(DeleteMedia(siteId, mediaId));

        var response = await client.ExecuteAsync(request);

        return DeserializeOrError(response, RequestDescription);
    }

    public async Task<Result<MediaObjectSchema, JwErrorResponse>> ReuploadMediaAsync(Alpha siteId, Alpha mediaId, MediaUpload upload)
    {
        const string RequestDescription = "Reupload Media";
        var client = CreateBaseRestClient();
        var body = JsonSerializer.Serialize(upload, _jsonOptions);
        var request = await CreateRestRequestAsync(ReuploadMedia(siteId, mediaId));

        request.AddParameter("application/json", body, ParameterType.RequestBody);

        var response = await client.ExecuteAsync(request);

        return DeserializeOrError<MediaObjectSchema>(response, RequestDescription);
    }
}

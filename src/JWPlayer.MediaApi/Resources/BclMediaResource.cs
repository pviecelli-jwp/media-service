using JWPlayer.ApiGateway.Errors;
using JWPlayer.ApiGateway.Services;
using JWPlayer.ApiGateway.Services.Abstractions;
using JWPlayer.Identity;
using JWPlayer.MediaApi.Objects;
using JWPlayer.MediaApi.Requests;
using JWPlayer.Outcomes;
using Microsoft.Extensions.Options;
using RestSharp;
using System.Text.Json;

namespace JWPlayer.MediaApi.Resources;

public class BclMediaResource(
    IOptions<MediaApiOptions> options,
    IRestClientFactory restClientFactory,
    IAuthTokenFactory tokenFactory) : BaseResource(restClientFactory, tokenFactory, options), IBclMediaResource
{
    internal static Endpoint CreateBroadcastLiveMedia(Alpha siteId) => new($"internal/v2/sites/{siteId}/live_broadcast/", Method.PUT);
    internal static Endpoint CaptureMedia(Alpha siteId, Alpha mediaId) => new($"internal/v2/sites/{siteId}/media/{mediaId}/live_broadcast_capture/", Method.PUT);
    internal static Endpoint CreateClip(Alpha siteId) => new($"internal/v2/sites/{siteId}/live_broadcast_clip/", Method.POST);

    public async Task<Result<MediaObjectSchema, JwErrorResponse>> CreateBclMediaAsync(Alpha siteId, MediaCreateParameters mediaCreateParameters)
    {
        const string RequestDescription = "Creating BCL Media";

        var body = JsonSerializer.Serialize(mediaCreateParameters, _jsonOptions);

        var response = await ExecuteRequestAsync(CreateBroadcastLiveMedia(siteId), body);

        return DeserializeOrError<MediaObjectSchema>(response, RequestDescription);
    }

    public async Task<Result<JwErrorResponse>> CaptureMediaAsync(Alpha siteId, Alpha mediaId, MediaCaptureParameters mediaCaptureParameters)
    {
        const string RequestDescription = "Capturing Media";

        var body = new JsonObject
        {
            ["upload"] = JsonSerializer.Serialize(mediaCaptureParameters, _jsonOptions)
        };

        var response = await ExecuteRequestAsync(CaptureMedia(siteId, mediaId), body);

        return DeserializeOrError(response, RequestDescription);
    }

    public async Task<Result<JwErrorResponse>> CreateClipAsync(Alpha siteId, Alpha mediaId, MediaCreateClipParameters mediaCreateClipParameters)
    {
        const string RequestDescription = "Creating Clip for media";

        var body = new JsonObject
        {
            ["metadata"] = JsonSerializer.Serialize(mediaCreateClipParameters.Metadata, _jsonOptions),
            ["upload"] = JsonSerializer.Serialize(mediaCreateClipParameters.CaptureParameters, _jsonOptions),
            ["relationships"] = new JsonObject
            {
                ["media_id"] = mediaId.ToString()
            }
        };

        var response = await ExecuteRequestAsync(CaptureMedia(siteId, mediaId), body);

        return DeserializeOrError(response, RequestDescription);
    }
}

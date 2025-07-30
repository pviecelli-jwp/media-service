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

    public async Task<Result<MediaObjectSchema, JwErrorResponse>> CreateBclMediaAsync(Alpha siteId, MediaCreateParameters mediaCreateParams)
    {
        const string RequestDescription = "Creating BCL Media";
        var client = CreateBaseRestClient();
        var body = JsonSerializer.Serialize(mediaCreateParams, _jsonOptions);
        var request = await CreateRestRequestAsync(CreateBroadcastLiveMedia(siteId));

        request.AddParameter("application/json", body, ParameterType.RequestBody);

        var response = await client.ExecuteAsync(request);

        return DeserializeOrError<MediaObjectSchema>(response, RequestDescription);
    }

    public Task<Result<MediaObjectSchema, JwErrorResponse>> Capture(Alpha siteId, Alpha mediaId)
    {
        throw new NotImplementedException();
    }

    public Task<Result<MediaObjectSchema, JwErrorResponse>> CreateClip(Alpha siteId, Alpha mediaId)
    {
        throw new NotImplementedException();
    }
}

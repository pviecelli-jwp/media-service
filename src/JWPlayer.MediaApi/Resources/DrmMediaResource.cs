using JWPlayer.ApiGateway.Errors;
using JWPlayer.ApiGateway.Services;
using JWPlayer.ApiGateway.Services.Abstractions;
using JWPlayer.Identity;
using JWPlayer.MediaApi.Model;
using JWPlayer.Outcomes;
using Microsoft.Extensions.Options;
using RestSharp;
using System.Text.Json.Nodes;
using JsonObject = System.Text.Json.Nodes.JsonObject;

namespace JWPlayer.MediaApi.Resources;
public class DrmMediaResource(
    IOptions<MediaApiOptions> mediaOptions,
    IRestClientFactory restClientFactory,
    IAuthTokenFactory tokenFactory) : BaseResource(restClientFactory, tokenFactory, mediaOptions), IDrmMediaResource
{
    internal static Endpoint EnableDrmOnExternalMedia(Alpha siteId, Alpha mediaId) => new($"v2/sites/{siteId}/media/{mediaId}/enable_drm_on_external_media", Method.PUT);
    internal static Endpoint IsDrmEnabled(Alpha siteId) => new($"v2/sites/{siteId}/is_drm_enabled", Method.GET);
    internal static Endpoint GetDrmPolicies(Alpha siteId) => new($"v2/sites/{siteId}/drm_policies/", Method.GET);

    public async Task<Result<string, JwErrorResponse>> SetContentIdOnMediaAsync(Alpha siteId, Alpha mediaId, string contentId)
    {
        const string RequestDescription = "Setting content Id for Media";

        var client = _restClientFactory.Create(_mediaOptions.SetDrmEndpointBaseUrl);

        // Endpoint has unique authorization:
        var endpoint = EnableDrmOnExternalMedia(siteId, mediaId);
        var request = new RestRequest(endpoint.Resource, endpoint.Method);
        request.AddHeader("Authorization", _mediaOptions.SetDrmEndpointSecret);
        request.AddHeader("Content-Type", "application/json");

        var body = new JsonObject
        {
            ["content_id"] = contentId
        };

        request.AddParameter("application/json", body, ParameterType.RequestBody);

        var response = await client.ExecuteAsync(request);
        return DeserializeOrError<string>(response, RequestDescription);
    }

    public async Task<Result<bool, JwErrorResponse>> IsDrmEnabledAsync(Alpha siteId)
    {
        const string RequestDescription = "Querying site for DRM enabled";

        var response = await ExecuteRequestAsync(IsDrmEnabled(siteId));

        if (response.IsSuccessful)
        {
            var responseData = JsonNode.Parse(response.Content);

            var isEnabled = (bool?)responseData?["is_enabled"];
            if (isEnabled != null) return isEnabled.Value;

            throw new MediaApiException($"Site DRM query for {siteId} encountered a problem parsing the response content: {response.Content}");
        }

        return HandleError(response, RequestDescription);
    }

    public async Task<Result<bool, JwErrorResponse>> HasDrmPoliciesAsync(Alpha siteId)
    {
        const string RequestDescription = "Querying site for DRM policies";

        var response = await ExecuteRequestAsync(GetDrmPolicies(siteId));

        if (response.IsSuccessful)
        {
            var responseData = JsonNode.Parse(response.Content);

            var policies = responseData?["drm_policies"]?.AsArray();
            if (policies != null) return policies.Count != 0;

            throw new MediaApiException($"Site DRM policy query for {siteId} encountered a problem parsing the response content: {response.Content}");
        }

        return HandleError(response, RequestDescription);
    }
}
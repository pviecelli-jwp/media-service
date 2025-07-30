using JWPlayer.ApiGateway.Errors;
using JWPlayer.ApiGateway.Services;
using JWPlayer.ApiGateway.Services.Abstractions;
using JWPlayer.MediaApi.Model;
using JWPlayer.Outcomes;
using Microsoft.Extensions.Options;
using RestSharp;
using System.Text.Json;

namespace JWPlayer.MediaApi.Resources;
public abstract class BaseResource
{
    protected readonly IRestClientFactory _restClientFactory;
    protected readonly IAuthTokenFactory _tokenFactory;
    protected readonly MediaApiOptions _mediaOptions;
    private readonly string _baseUrl;

    protected readonly JsonSerializerOptions _jsonOptions = new()
    {
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        AllowTrailingCommas = true,
    };

    protected BaseResource(
        IRestClientFactory restClientFactory,
        IAuthTokenFactory tokenFactory,
        IOptions<MediaApiOptions> mediaOptions)
    {
        _restClientFactory = restClientFactory ?? throw new ArgumentNullException(nameof(restClientFactory));
        _tokenFactory = tokenFactory ?? throw new ArgumentNullException(nameof(tokenFactory));
        _mediaOptions = mediaOptions.Value ?? throw new ArgumentNullException(nameof(mediaOptions));
        _baseUrl = _mediaOptions.BaseUrl;
    }

    protected IRestClient CreateBaseRestClient() => _restClientFactory.Create(_baseUrl);

    protected async Task<RestRequest> CreateRestRequestAsync(Endpoint endpoint)
    {
        var tokenResult = await _tokenFactory.GetAuthTokenAsync();
        if (tokenResult.IsErr)
            throw new MediaApiException("Failed to get Auth Token", tokenResult.GetError());

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
    protected static Result<T, JwErrorResponse> DeserializeOrError<T>(IRestResponse response, string requestDesc)
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

    protected static Result<JwErrorResponse> DeserializeOrError(IRestResponse response, string requestDesc)
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
    protected static JwErrorResponse HandleError(IRestResponse response, string requestDesc, Exception? exception = null)
    {
        var isJwError = response.Content.TryGetJwErrorResponse(out var err);
        if (!isJwError)
            throw new MediaApiException($"Failed to deserialize error response for {requestDesc}, status code {response.StatusCode}, response content: {response.Content}", exception);

        return err;
    }
}

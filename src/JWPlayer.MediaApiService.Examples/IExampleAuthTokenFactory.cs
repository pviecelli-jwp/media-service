using JWPlayer.ApiGateway.Services.Abstractions;

namespace JWPlayer.MediaApiService.Examples;

public interface IExampleAuthTokenFactory : IAuthTokenFactory
{
    /// <summary>
    /// Sets the authentication token.
    /// </summary>
    /// <param name="authToken">The authentication token to set.</param>
    void SetAuthToken(string authToken);
}

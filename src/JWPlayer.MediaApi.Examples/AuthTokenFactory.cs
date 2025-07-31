using JWPlayer.Outcomes;

namespace JWPlayer.MediaApi.Examples;

public class AuthTokenFactory : IExampleAuthTokenFactory
{
    private string? _authToken;
    public Result<string, Exception> GetAuthToken()
    {
        return _authToken is not null
            ? Result.Ok(_authToken)
            : Result.Err<string>(new Exception("Auth token is not set."));
    }

    public async Task<Result<string, Exception>> GetAuthTokenAsync()
    {
        await Task.CompletedTask;

        return _authToken is not null
            ? Result.Ok(_authToken)
            : Result.Err<string>(new Exception("Auth token is not set."));
    }

    public void SetAuthToken(string authToken)
    {
        _authToken = authToken ?? throw new ArgumentNullException(nameof(authToken), "Auth token cannot be null.");
    }
}

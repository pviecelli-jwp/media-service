using Microsoft.AspNetCore.Mvc;

namespace JWPlayer.MediaApiService.Examples.Controllers;
[ApiController]
[Route("[controller]")]
public class AuthTokenController(IExampleAuthTokenFactory exampleAuthTokenFactory) : ControllerBase
{
    private readonly IExampleAuthTokenFactory _authTokenFactory = exampleAuthTokenFactory ?? throw new ArgumentNullException(nameof(exampleAuthTokenFactory), "Auth token factory cannot be null.");

    [HttpPost]
    public IActionResult SetToken(string authToken)
    {
        _authTokenFactory.SetAuthToken(authToken);

        return Ok();
    }
}

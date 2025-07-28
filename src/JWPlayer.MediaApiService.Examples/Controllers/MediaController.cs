using JWPlayer.ApiGateway.Errors;
using JWPlayer.MediaApiService.Model;
using Microsoft.AspNetCore.Mvc;

namespace JWPlayer.MediaApiService.Examples.Controllers;
public class MediaController(ILogger<MediaController> logger, IMediaApiService mediaApiService) : Controller
{
    private readonly ILogger<MediaController> _logger = logger;
    private readonly IMediaApiService _mediaApiService = mediaApiService;

    [HttpPost("sites/{siteId}/")]
    public async Task<IActionResult> CreateMedia(string siteId, [FromBody] MediaCreateParams mediaCreateParams)
    {
        _logger.LogInformation("Creating media with parameters: {@MediaCreateParams}", mediaCreateParams);

        var result = await _mediaApiService.CreateMediaAsync(siteId, mediaCreateParams);
        return result.IsOk
            ? Ok(result.Value)
            : result.Error.ToObjectResult();
    }

    [HttpGet("sites/{siteId}/media/{mediaId}")]
    public async Task<IActionResult> GetMedia(string siteId, string mediaId)
    {
        _logger.LogInformation("Getting media {MediaId} from site {SiteId}", mediaId, siteId);

        var result = await _mediaApiService.GetMediaAsync(siteId, mediaId);
        return result.IsOk
            ? Ok(result.Value)
            : result.Error.ToObjectResult();
    }

    [HttpDelete("sites/{siteId}/media/{mediaId}")]
    public async Task<IActionResult> DeleteMedia(string siteId, string mediaId)
    {
        _logger.LogInformation("Getting media {MediaId} from site {SiteId}", mediaId, siteId);

        var result = await _mediaApiService.DeleteMediaAsync(siteId, mediaId);
        return result.IsOk
            ? Ok()
            : result.Error.ToObjectResult();
    }
}

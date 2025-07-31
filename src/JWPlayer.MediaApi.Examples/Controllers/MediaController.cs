using JWPlayer.ApiGateway.Errors;
using JWPlayer.MediaApi.Requests;
using JWPlayer.MediaApi.Resources;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace JWPlayer.MediaApi.Examples.Controllers;
public class MediaController(
    ILogger<MediaController> logger,
    IMediaResource mediaResource,
    IBclMediaResource bclMediaResource) : Controller
{
    private readonly ILogger<MediaController> _logger = logger;
    private readonly IMediaResource _mediaResource = mediaResource;
    private readonly IBclMediaResource _bclMediaResource = bclMediaResource;

    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        AllowTrailingCommas = true,
    };

    [HttpPost("sites/{siteId}/")]
    public async Task<IActionResult> CreateMedia(string siteId, [FromBody] MediaCreateParameters mediaCreateParams)
    {
        _logger.LogInformation("Creating media with parameters: {@MediaCreateParams}", mediaCreateParams);

        var result = await _mediaResource.CreateMediaAsync(siteId, mediaCreateParams);
        return result.IsOk
            ? Ok(result.Value)
            : result.Error.ToObjectResult();
    }

    [HttpGet("sites/{siteId}/media/{mediaId}")]
    public async Task<IActionResult> GetMedia(string siteId, string mediaId)
    {
        _logger.LogInformation("Getting media {MediaId} from site {SiteId}", mediaId, siteId);

        var result = await _mediaResource.GetMediaAsync(siteId, mediaId);
        return result.IsOk
            ? Ok(result.Value)
            : result.Error.ToObjectResult();
    }

    [HttpGet("sites/{siteId}/media")]
    public async Task<IActionResult> GetAllMedias(string siteId, [FromQuery] int page = 1, [FromQuery] int pageLength = 10, [FromQuery] string? q = null, [FromQuery] string? sort = null)
    {
        _logger.LogInformation("Getting all media from site {SiteId} with page {Page}, pageLength {PageLength}, query {Q}, sort {Sort}", siteId, page, pageLength, q, sort);
        var result = await _mediaResource.GetAllMediasAsync(siteId, page, pageLength, q, sort);
        return result.IsOk
            ? Ok(result.Value)
            : result.Error.ToObjectResult();
    }

    [HttpDelete("sites/{siteId}/media/{mediaId}")]
    public async Task<IActionResult> DeleteMedia(string siteId, string mediaId)
    {
        _logger.LogInformation("Getting media {MediaId} from site {SiteId}", mediaId, siteId);

        var result = await _mediaResource.DeleteMediaAsync(siteId, mediaId);
        return result.IsOk
            ? Ok()
            : result.Error.ToObjectResult();
    }

    [HttpPut("sites/{siteId}/media/{mediaId}/capture")]
    public async Task<IActionResult> CaptureMedia(string siteId, string mediaId, [FromBody] MediaCaptureParameters mediaCaptureParams)
    {
        _logger.LogInformation("Capturing media {MediaId} on site {SiteId} with parameters: {@MediaCaptureParams}", mediaId, siteId, mediaCaptureParams);
        var result = await _bclMediaResource.CaptureMediaAsync(siteId, mediaId, mediaCaptureParams);
        return result.IsOk
            ? Ok()
            : result.Error.ToObjectResult();
    }

    [HttpPut("serialise")]
    public IActionResult SerialiseParams([FromBody] MediaCreateParameters parameters)
    {
        var serialized = JsonSerializer.Serialize(parameters, _jsonOptions);
        return Ok(serialized);
    }
}

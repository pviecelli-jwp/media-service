using JWPlayer.ApiGateway.Errors;
using JWPlayer.Identity;
using JWPlayer.MediaApi.Objects;
using JWPlayer.MediaApi.Requests;
using JWPlayer.Outcomes;

namespace JWPlayer.MediaApi.Resources;
public interface IBclMediaResource
{
    Task<Result<MediaObjectSchema, JwErrorResponse>> CreateBclMediaAsync(Alpha siteId, MediaCreateParameters mediaCreateParams);
    Task<Result<JwErrorResponse>> CaptureMediaAsync(Alpha siteId, Alpha mediaId, MediaCaptureParameters mediaCaptureParameters);
    Task<Result<JwErrorResponse>> CreateClipAsync(Alpha siteId, Alpha mediaId, MediaCreateClipParameters mediaCaptureParameters);
}

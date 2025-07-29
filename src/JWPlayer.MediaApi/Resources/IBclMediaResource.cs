using JWPlayer.ApiGateway.Errors;
using JWPlayer.Identity;
using JWPlayer.MediaApi.Objects;
using JWPlayer.MediaApi.Requests;
using JWPlayer.Outcomes;

namespace JWPlayer.MediaApi.Resources;
public interface IBclMediaResource
{
    Task<Result<MediaObjectSchema, JwErrorResponse>> CreateBclMediaAsync(Alpha siteId, MediaCreateParameters mediaCreateParams);
    Task<Result<MediaObjectSchema, JwErrorResponse>> Capture(Alpha siteId, Alpha mediaId);
    Task<Result<MediaObjectSchema, JwErrorResponse>> CreateClip(Alpha siteId, Alpha mediaId);
}

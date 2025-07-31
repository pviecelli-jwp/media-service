using JWPlayer.ApiGateway.Errors;
using JWPlayer.Identity;
using JWPlayer.MediaApi.Model;
using JWPlayer.MediaApi.Objects;
using JWPlayer.MediaApi.Requests;
using JWPlayer.Outcomes;

namespace JWPlayer.MediaApi.Resources;
public interface IMediaResource
{
    Task<Result<MediaObjectSchema, JwErrorResponse>> CreateMediaAsync(Alpha siteId, MediaCreateParameters mediaCreateParams);
    Task<Result<MediaObjectSchema, JwErrorResponse>> GetMediaAsync(Alpha siteId, Alpha mediaId);
    Task<Result<MediaCollectionSchema, JwErrorResponse>> GetAllMediasAsync(Alpha siteId, int? page, int? pageLength, string? q, string? sort);
    Task<Result<MediaObjectSchema, JwErrorResponse>> UpdateMediaAsync(Alpha siteId, Alpha mediaId, MediaUpdateParameters mediaUpdateParams);
    Task<Result<JwErrorResponse>> DeleteMediaAsync(Alpha siteId, Alpha mediaId);
    Task<Result<MediaObjectSchema, JwErrorResponse>> ReuploadMediaAsync(Alpha siteId, Alpha mediaId, MediaUpload upload);
}

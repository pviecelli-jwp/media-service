using JWPlayer.ApiGateway.Errors;
using JWPlayer.Identity;
using JWPlayer.MediaApiService.Model;
using JWPlayer.Outcomes;
using RestSharp;

namespace JWPlayer.MediaApiService;

public interface IMediaApiService
{
    Task<Result<MediaObjectSchema, JwErrorResponse>> CreateMediaAsync(Alpha siteId, MediaCreateParams mediaCreateParams);
    Task<Result<MediaObjectSchema, JwErrorResponse>> GetMediaAsync(Alpha siteId, Alpha mediaId);
    Task<Result<MediaObjectSchema, JwErrorResponse>> UpdateMediaAsync(Alpha siteId, Alpha mediaId, MediaUpdateParams mediaUpdateParams);
    Task<Result<JwErrorResponse>> DeleteMediaAsync(Alpha siteId, Alpha mediaId);
    Task<IRestResponse> ReuploadMediaAsync(Alpha siteId, Alpha mediaId, MediaUpload upload);
}
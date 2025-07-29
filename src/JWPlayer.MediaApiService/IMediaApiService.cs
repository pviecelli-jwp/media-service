using JWPlayer.ApiGateway.Errors;
using JWPlayer.Identity;
using JWPlayer.MediaApiService.Model;
using JWPlayer.MediaApiService.Requests;
using JWPlayer.MediaApiService.Resources;
using JWPlayer.Outcomes;
using RestSharp;

namespace JWPlayer.MediaApiService;

public interface IMediaApiService
{
    Task<Result<MediaObjectSchema, JwErrorResponse>> CreateMediaAsync(Alpha siteId, MediaCreateParameters mediaCreateParams);
    Task<Result<MediaObjectSchema, JwErrorResponse>> GetMediaAsync(Alpha siteId, Alpha mediaId);
    Task<Result<MediaCollectionSchema, JwErrorResponse>> GetAllMediasAsync(Alpha siteId, int? page, int? pageLength, string? q, string? sort);
    Task<Result<MediaObjectSchema, JwErrorResponse>> UpdateMediaAsync(Alpha siteId, Alpha mediaId, MediaUpdateParameters mediaUpdateParams);
    Task<Result<JwErrorResponse>> DeleteMediaAsync(Alpha siteId, Alpha mediaId);
    Task<IRestResponse> ReuploadMediaAsync(Alpha siteId, Alpha mediaId, MediaUpload upload);
}
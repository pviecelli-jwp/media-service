using JWPlayer.ApiGateway.Errors;
using JWPlayer.Identity;
using JWPlayer.Outcomes;
using MediaService.Model;
using RestSharp;

namespace MediaService
{
    public interface IMediaService
    {
        Task<Result<MediaObjectSchema, JwErrorResponse>> CreateMediaAsync(Alpha siteId, MediaCreateParams mediaCreateParams);
        Task<Result<MediaObjectSchema, JwErrorResponse>> GetMediaAsync(Alpha siteId, Alpha mediaId);
        Task<Result<MediaObjectSchema, JwErrorResponse>> UpdateMediaAsync(Alpha siteId, Alpha mediaId, MediaUpdateParams mediaUpdateParams);
        Task<Result<JwErrorResponse>> DeleteMediaAsync(Alpha siteId, Alpha mediaId);

        Task<IRestResponse> ReuploadMediaAsync(Alpha siteId, Alpha mediaId, MediaUpload upload);
    }
}
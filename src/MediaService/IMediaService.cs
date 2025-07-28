using JWPlayer.Identity;
using MediaService.Model;
using RestSharp;

namespace MediaService
{
    public interface IMediaService
    {
        Task<IRestResponse> CreateAsync(Alpha siteId, MediaCreateParams mediaCreateParams);
        Task Delete(Alpha siteId, Alpha mediaId);

        Task<IRestResponse> Reupload(Alpha siteId, Alpha mediaId, MediaUpload upload);
    }
}
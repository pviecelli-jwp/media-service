using JWPlayer.Identity;
using MediaService.Model;
using RestSharp;

namespace MediaService
{
    public interface IMediaService
    {
        Task<IRestResponse> CreateAsync(Alpha siteId, string metadata);
        Task Delete(Alpha siteId, Alpha mediaId);

        Task Reupload(Alpha siteId, Alpha mediaId, MediaUpload upload);
    }
}
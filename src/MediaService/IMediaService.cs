using JWPlayer.Identity;

namespace MediaService
{
    public interface IMediaService
    {
        Task<IRestResponse> CreateAsync(Alpha siteId, string metadata);
        void Delete(Alpha siteId, Alpha mediaId);

        void Reupload(Alpha siteId, Alpha mediaId, MediaUpload upload);
    }
}
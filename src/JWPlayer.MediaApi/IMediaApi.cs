using JWPlayer.MediaApi.Resources;

namespace JWPlayer.MediaApi;

public interface IMediaApi
{
    IMediaResource Media { get; }
    IBclMediaResource Bcl { get; }
    IDrmMediaResource Drm { get; }
}
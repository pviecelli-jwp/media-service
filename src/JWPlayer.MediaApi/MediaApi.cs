using JWPlayer.ApiGateway.Services;
using JWPlayer.ApiGateway.Services.Abstractions;
using JWPlayer.MediaApi.Resources;
using Microsoft.Extensions.Options;

namespace JWPlayer.MediaApi;
public class MediaApi : IMediaApi
{
    public MediaApi(MediaApiOptions mediaApiOptions, IAuthTokenFactory authTokenFactory)
    {
        var options = Options.Create(mediaApiOptions);
        var restClientFactory = new RestClientFactory();
        Media = new MediaResource(options, restClientFactory, authTokenFactory);
        Bcl = new BclMediaResource(options, restClientFactory, authTokenFactory);
        Drm = new DrmMediaResource(options, restClientFactory, authTokenFactory);
    }

    public MediaApi(IOptions<MediaApiOptions> options, IAuthTokenFactory authTokenFactory)
    {
        var restClientFactory = new RestClientFactory();
        Media = new MediaResource(options, restClientFactory, authTokenFactory);
        Bcl = new BclMediaResource(options, restClientFactory, authTokenFactory);
        Drm = new DrmMediaResource(options, restClientFactory, authTokenFactory);
    }

    public MediaApi(IMediaResource media, IBclMediaResource bcl, IDrmMediaResource drm)
    {
        Media = media;
        Bcl = bcl;
        Drm = drm;
    }

    public IMediaResource Media { get; }
    public IBclMediaResource Bcl { get; }
    public IDrmMediaResource Drm { get; }
}

using JWPlayer.ApiGateway.Errors;
using JWPlayer.Identity;
using JWPlayer.Outcomes;

namespace JWPlayer.MediaApi.Resources;
public interface IDrmMediaResource
{
    Task<Result<string, JwErrorResponse>> SetContentIdOnMediaAsync(Alpha siteId, Alpha mediaId, string contentId);
    Task<Result<bool, JwErrorResponse>> IsDrmEnabledAsync(Alpha siteId);
    Task<Result<bool, JwErrorResponse>> HasDrmPoliciesAsync(Alpha siteId);
}

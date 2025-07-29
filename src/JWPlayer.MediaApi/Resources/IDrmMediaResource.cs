using JWPlayer.ApiGateway.Errors;
using JWPlayer.Identity;
using JWPlayer.MediaApi.Objects;
using JWPlayer.Outcomes;

namespace JWPlayer.MediaApi.Resources;
public interface IDrmMediaResource
{
    Task<Result<MediaObjectSchema, JwErrorResponse>> EnableDrmOnExternalMediaAsync(Alpha siteId, Alpha mediaId);
    Task<Result<bool, JwErrorResponse>> IsDrmEnabledAsync(Alpha siteId);
    Task<Result<string, JwErrorResponse>> GetDrmPoliciesAsync(Alpha siteId);
}

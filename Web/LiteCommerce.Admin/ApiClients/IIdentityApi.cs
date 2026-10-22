using LiteCommerce.Admin.Constants;
using LiteCommerce.Admin.Models.Business.Auth;
using LiteCommerce.Admin.Models.Common;
using Refit;

namespace LiteCommerce.Admin.ApiClients
{
    // Points at the Identity service, not Catalog.
    public interface IIdentityApi
    {
        [Post(ApiRoutes.Auth.Login)]
        Task<BaseResponse<AuthResponse>> LoginAsync([Body] LoginRequest request);

        [Post(ApiRoutes.Auth.Refresh)]
        Task<BaseResponse<AuthResponse>> RefreshAsync([Body] RefreshRequest request);

        [Post(ApiRoutes.Auth.Revoke)]
        Task<BaseResponse<bool>> RevokeAsync([Body] RevokeRequest request);

        // Takes the token explicitly: this client has no AuthHeaderHandler.
        [Post(ApiRoutes.Auth.ChangePassword)]
        Task<BaseResponse<bool>> ChangePasswordAsync([Authorize("Bearer")] string token, [Body] ChangePasswordRequest request);

    }
}

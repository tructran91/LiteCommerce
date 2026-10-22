using LiteCommerce.Admin.Constants;
using LiteCommerce.Admin.Models.Business.Profile;
using LiteCommerce.Admin.Models.Common;
using Refit;

namespace LiteCommerce.Admin.ApiClients
{
    public interface IProfileApi
    {
        [Get(ApiRoutes.Auth.Me)]
        Task<BaseResponse<UserProfileResponse>> GetMeAsync();

        [Put(ApiRoutes.Auth.Me)]
        Task<BaseResponse<UserProfileResponse>> UpdateMeAsync([Body] UpdateProfileRequest request);

        [Get(ApiRoutes.Auth.Sessions)]
        Task<BaseResponse<List<UserSessionResponse>>> GetSessionsAsync();

        [Delete(ApiRoutes.Auth.SessionById)]
        Task<BaseResponse<bool>> RevokeSessionAsync(string id);

        [Delete(ApiRoutes.Auth.Sessions)]
        Task<BaseResponse<bool>> RevokeAllSessionsAsync();
    }
}

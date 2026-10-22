using LiteCommerce.Admin.Constants;
using LiteCommerce.Admin.Models.Business.Auth;
using LiteCommerce.Admin.Models.Common;
using Refit;

namespace LiteCommerce.Admin.ApiClients
{
    public interface IPasswordRecoveryApi
    {
        [Post(ApiRoutes.Auth.ForgotPassword)]
        Task<BaseResponse<bool>> ForgotPasswordAsync([Body] ForgotPasswordRequest request);

        [Post(ApiRoutes.Auth.ResetPassword)]
        Task<BaseResponse<bool>> ResetPasswordAsync([Body] ResetPasswordRequest request);
    }
}

using LiteCommerce.Admin.Constants;
using LiteCommerce.Admin.Models.Business.Auth;
using LiteCommerce.Admin.Models.Common;
using Refit;

namespace LiteCommerce.Admin.ApiClients
{
    // Takes the Authorization header as an argument instead of going through
    // AuthHeaderHandler, which would otherwise need AuthService, which needs this client.
    public interface ISessionApi
    {
        [Get(ApiRoutes.Session.Get)]
        Task<BaseResponse<SessionResponse>> GetSessionAsync([Authorize("Bearer")] string token);
    }
}

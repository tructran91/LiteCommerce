using LiteCommerce.Admin.Constants;
using LiteCommerce.Admin.Models.Business.User;
using LiteCommerce.Admin.Models.Common;
using Refit;

namespace LiteCommerce.Admin.ApiClients
{
    public interface IAdminUserApi
    {
        [Get(ApiRoutes.AdminUser.GetAll)]
        Task<BaseResponse<List<AdminUserResponse>>> GetUsersAsync(int currentPage, int pageSize, string? search = null, string? role = null, bool? isActive = null, bool? isLocked = null);

        [Get(ApiRoutes.AdminUser.GetById)]
        Task<BaseResponse<AdminUserResponse>> GetUserAsync(string id);

        [Post(ApiRoutes.AdminUser.Create)]
        Task<BaseResponse<AdminUserResponse>> CreateUserAsync([Body] CreateUserRequest request);

        [Put(ApiRoutes.AdminUser.Update)]
        Task<BaseResponse<AdminUserResponse>> UpdateUserAsync(string id, [Body] UpdateUserRequest request);

        [Delete(ApiRoutes.AdminUser.Delete)]
        Task<BaseResponse<bool>> DeleteUserAsync(string id);

        [Patch(ApiRoutes.AdminUser.UpdateStatus)]
        Task<BaseResponse<AdminUserResponse>> UpdateStatusAsync(string id, [Body] UpdateUserStatusRequest request);

        [Put(ApiRoutes.AdminUser.UpdateRole)]
        Task<BaseResponse<AdminUserResponse>> UpdateRoleAsync(string id, [Body] UpdateUserRoleRequest request);

        [Post(ApiRoutes.AdminUser.Unlock)]
        Task<BaseResponse<bool>> UnlockAsync(string id);

        [Post(ApiRoutes.AdminUser.RevokeSessions)]
        Task<BaseResponse<bool>> RevokeSessionsAsync(string id);

        [Get(ApiRoutes.AdminRole.GetAll)]
        Task<BaseResponse<List<RoleResponse>>> GetRolesAsync();
    }
}

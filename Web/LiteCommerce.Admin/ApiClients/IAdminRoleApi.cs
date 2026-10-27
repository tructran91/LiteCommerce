using LiteCommerce.Admin.Constants;
using LiteCommerce.Admin.Models.Business.Role;
using LiteCommerce.Admin.Models.Common;
using Refit;

namespace LiteCommerce.Admin.ApiClients
{
    public interface IAdminRoleApi
    {
        [Get(ApiRoutes.AdminRole.GetAll)]
        Task<BaseResponse<List<RoleDetailResponse>>> GetRolesAsync();

        [Get(ApiRoutes.AdminRole.GetById)]
        Task<BaseResponse<RoleDetailResponse>> GetRoleAsync(string id);

        [Get(ApiRoutes.AdminRole.GetPermissionCatalog)]
        Task<BaseResponse<List<PermissionGroupResponse>>> GetPermissionCatalogAsync();

        [Post(ApiRoutes.AdminRole.Create)]
        Task<BaseResponse<RoleDetailResponse>> CreateRoleAsync([Body] CreateRoleRequest request);

        [Put(ApiRoutes.AdminRole.Update)]
        Task<BaseResponse<RoleDetailResponse>> UpdateRoleAsync(string id, [Body] UpdateRoleRequest request);

        [Delete(ApiRoutes.AdminRole.Delete)]
        Task<BaseResponse<bool>> DeleteRoleAsync(string id);

        [Put(ApiRoutes.AdminRole.UpdatePermissions)]
        Task<BaseResponse<RoleDetailResponse>> UpdatePermissionsAsync(string id, [Body] UpdateRolePermissionsRequest request);
    }
}

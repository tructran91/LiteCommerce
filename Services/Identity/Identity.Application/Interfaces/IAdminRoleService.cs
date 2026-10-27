using Identity.Application.DTOs;
using LiteCommerce.Shared.Models;

namespace Identity.Application.Interfaces
{
    public interface IAdminRoleService
    {
        Task<BaseResponse<List<RoleDetailResponse>>> GetRolesAsync(CancellationToken cancellationToken = default);

        Task<BaseResponse<RoleDetailResponse>> GetRoleAsync(string id, CancellationToken cancellationToken = default);

        Task<BaseResponse<RoleDetailResponse>> CreateRoleAsync(CreateRoleRequest request, CancellationToken cancellationToken = default);

        Task<BaseResponse<RoleDetailResponse>> UpdateRoleAsync(string id, UpdateRoleRequest request, CancellationToken cancellationToken = default);

        Task<BaseResponse<bool>> DeleteRoleAsync(string id, CancellationToken cancellationToken = default);

        Task<BaseResponse<List<PermissionGroupResponse>>> GetPermissionCatalogAsync(CancellationToken cancellationToken = default);

        Task<BaseResponse<RoleDetailResponse>> UpdateRolePermissionsAsync(
            Guid actorUserId, string id, UpdateRolePermissionsRequest request, CancellationToken cancellationToken = default);
    }
}

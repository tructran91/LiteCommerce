using Identity.Application.DTOs;
using LiteCommerce.Shared.Models;

namespace Identity.Application.Interfaces
{
    public interface IAdminUserService
    {
        Task<BaseResponse<List<AdminUserResponse>>> GetUsersAsync(GetUsersQuery query, CancellationToken cancellationToken = default);
        Task<BaseResponse<AdminUserResponse>> GetUserAsync(string id, CancellationToken cancellationToken = default);
        Task<BaseResponse<AdminUserResponse>> CreateUserAsync(CreateUserRequest request, CancellationToken cancellationToken = default);
        Task<BaseResponse<AdminUserResponse>> UpdateUserAsync(string id, UpdateUserRequest request, CancellationToken cancellationToken = default);
        Task<BaseResponse<bool>> DeleteUserAsync(Guid actorUserId, string id, CancellationToken cancellationToken = default);
        Task<BaseResponse<AdminUserResponse>> UpdateStatusAsync(Guid actorUserId, string id, UpdateUserStatusRequest request, CancellationToken cancellationToken = default);
        Task<BaseResponse<AdminUserResponse>> UpdateRoleAsync(Guid actorUserId, string id, UpdateUserRoleRequest request, CancellationToken cancellationToken = default);
        Task<BaseResponse<bool>> UnlockAsync(string id, CancellationToken cancellationToken = default);
        Task<BaseResponse<bool>> RevokeSessionsAsync(string id, CancellationToken cancellationToken = default);
    }
}

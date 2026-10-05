using Identity.Application.DTOs;
using LiteCommerce.Shared.Models;

namespace Identity.Application.Interfaces
{
    public interface IAuthService
    {
        Task<BaseResponse<AuthResponse>> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default);

        Task<BaseResponse<AuthResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);

        Task<BaseResponse<AuthResponse>> RefreshAsync(RefreshRequest request, CancellationToken cancellationToken = default);

        Task<BaseResponse<bool>> RevokeAsync(RevokeRequest request, CancellationToken cancellationToken = default);

        Task<BaseResponse<UserResponse>> GetMeAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}

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

        Task<BaseResponse<bool>> ChangePasswordAsync(Guid userId, ChangePasswordRequest request, CancellationToken cancellationToken = default);

        Task<BaseResponse<UserResponse>> UpdateProfileAsync(Guid userId, UpdateProfileRequest request, CancellationToken cancellationToken = default);

        Task<BaseResponse<List<SessionResponse>>> GetSessionsAsync(Guid userId, CancellationToken cancellationToken = default);

        Task<BaseResponse<bool>> RevokeSessionAsync(Guid userId, string sessionId, CancellationToken cancellationToken = default);

        Task<BaseResponse<bool>> RevokeAllSessionsAsync(Guid userId, CancellationToken cancellationToken = default);

        Task<BaseResponse<bool>> ForgotPasswordAsync(ForgotPasswordRequest request, CancellationToken cancellationToken = default);

        Task<BaseResponse<bool>> ResetPasswordAsync(ResetPasswordRequest request, CancellationToken cancellationToken = default);

    }
}

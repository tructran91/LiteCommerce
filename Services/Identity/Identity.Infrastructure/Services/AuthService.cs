using FluentValidation;
using Identity.Application.DTOs;
using Identity.Application.Interfaces;
using Identity.Application.Options;
using Identity.Core.Constants;
using Identity.Core.Entities;
using Identity.Core.Enums;
using Identity.Infrastructure.Data;
using LiteCommerce.Shared.Authorization;
using LiteCommerce.Shared.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Data;

namespace Identity.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private const int MaxFailedLoginAttempts = 5;
        private static readonly TimeSpan LockoutDuration = TimeSpan.FromMinutes(15);

        // Verified against when the email is unknown, so unknown and known emails cost the same time.
        private static readonly string DummyPasswordHash = new PasswordHasher<User>().HashPassword(null!, "dummy-password");

        private readonly IdentityDbContext _db;
        private readonly ITokenService _tokens;
        private readonly IPasswordHasher<User> _hasher;
        private readonly JwtOptions _jwt;
        private readonly IValidator<RegisterRequest> _registerValidator;
        private readonly IValidator<LoginRequest> _loginValidator;
        private readonly IValidator<RefreshRequest> _refreshValidator;
        private readonly IValidator<RevokeRequest> _revokeValidator;
        private readonly IValidator<ChangePasswordRequest> _changePasswordValidator;
        private readonly IValidator<UpdateProfileRequest> _updateProfileValidator;
        private readonly IValidator<ForgotPasswordRequest> _forgotPasswordValidator;
        private readonly IValidator<ResetPasswordRequest> _resetPasswordValidator;
        private readonly IActivityLogger _activityLogger;
        private readonly ICurrentUserService _currentUser;
        private readonly IPasswordResetNotifier _passwordResetNotifier;

        public AuthService(
            IdentityDbContext db,
            ITokenService tokens,
            IPasswordHasher<User> hasher,
            IOptions<JwtOptions> jwt,
            IValidator<RegisterRequest> registerValidator,
            IValidator<LoginRequest> loginValidator,
            IValidator<RefreshRequest> refreshValidator,
            IValidator<RevokeRequest> revokeValidator,
            IValidator<ChangePasswordRequest> changePasswordValidator,
            IValidator<UpdateProfileRequest> updateProfileValidator,
            IValidator<ForgotPasswordRequest> forgotPasswordValidator,
            IValidator<ResetPasswordRequest> resetPasswordValidator,
            IActivityLogger activityLogger,
            ICurrentUserService currentUser,
            IPasswordResetNotifier passwordResetNotifier)
        {
            _db = db;
            _tokens = tokens;
            _hasher = hasher;
            _jwt = jwt.Value;
            _registerValidator = registerValidator;
            _loginValidator = loginValidator;
            _refreshValidator = refreshValidator;
            _revokeValidator = revokeValidator;
            _changePasswordValidator = changePasswordValidator;
            _updateProfileValidator = updateProfileValidator;
            _forgotPasswordValidator = forgotPasswordValidator;
            _resetPasswordValidator = resetPasswordValidator;
            _activityLogger = activityLogger;
            _currentUser = currentUser;
            _passwordResetNotifier = passwordResetNotifier;
        }

        public async Task<BaseResponse<AuthResponse>> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
        {
            var errors = Validate(_registerValidator, request);
            if (errors is not null)
                return BaseResponse<AuthResponse>.Failure("One or more validation errors occurred", errors);

            var email = request.Email.Trim().ToLowerInvariant();
            if (await _db.Users.AnyAsync(x => x.Email == email, cancellationToken))
            {
                await _activityLogger.LogAsync(new(ActivityAction.Register, false, null, email, ActivityFailureReason.EmailAlreadyRegistered), cancellationToken);
                return BaseResponse<AuthResponse>.Failure($"Email \"{email}\" is already registered.", statusCode: HttpStatusCode.Conflict);
            }

            var role = await _db.Roles.FirstAsync(x => x.Name == IdentityRoles.Customer, cancellationToken);
            var now = DateTime.UtcNow;
            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = email,
                FullName = request.FullName.Trim(),
                RoleId = role.Id,
                Role = role,
                IsActive = true,
                CreatedDate = now
            };
            user.PasswordHash = _hasher.HashPassword(user, request.Password);

            _db.Users.Add(user);
            var pair = await IssueTokensAsync(user, now, cancellationToken);

            try
            {
                await _db.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException)
            {
                // Lost a race with a concurrent register for the same email (unique index). Anything else is a real error.
                _db.ChangeTracker.Clear();
                if (await _db.Users.AnyAsync(x => x.Email == email, cancellationToken))
                {
                    await _activityLogger.LogAsync(new(ActivityAction.Register, false, null, email, ActivityFailureReason.EmailAlreadyRegistered), cancellationToken);
                    return BaseResponse<AuthResponse>.Failure($"Email \"{email}\" is already registered.", statusCode: HttpStatusCode.Conflict);
                }
                throw;
            }

            await _activityLogger.LogAsync(new(ActivityAction.Register, true, user.Id, email), cancellationToken);
            return BaseResponse<AuthResponse>.Success(pair, "Registered successfully", HttpStatusCode.Created);
        }

        public async Task<BaseResponse<AuthResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
        {
            var errors = Validate(_loginValidator, request);
            if (errors is not null)
                return BaseResponse<AuthResponse>.Failure("One or more validation errors occurred", errors);

            var email = request.Email.Trim().ToLowerInvariant();
            var user = await _db.Users
                .Include(x => x.Role)
                .FirstOrDefaultAsync(x => x.Email == email, cancellationToken);

            // Generic message on purpose: don't reveal whether the email exists.
            if (user is null)
            {
                _hasher.VerifyHashedPassword(null!, DummyPasswordHash, request.Password);
                await _activityLogger.LogAsync(new(ActivityAction.Login, false, null, email, ActivityFailureReason.UnknownEmail), cancellationToken);
                return BaseResponse<AuthResponse>.Failure("Invalid email or password.", statusCode: HttpStatusCode.Unauthorized);
            }

            var now = DateTime.UtcNow;
            if (user.LockoutEnd is not null && user.LockoutEnd > now)
            {
                await _activityLogger.LogAsync(new(ActivityAction.Login, false, user.Id, email, ActivityFailureReason.LockedOut), cancellationToken);
                return BaseResponse<AuthResponse>.Failure(
                    "Too many failed login attempts. Please try again later.",
                    statusCode: HttpStatusCode.TooManyRequests);
            }

            var passwordOk = _hasher.VerifyHashedPassword(user, user.PasswordHash, request.Password) != PasswordVerificationResult.Failed;
            if (!passwordOk)
            {
                // An expired lockout starts a fresh count.
                if (user.LockoutEnd is not null)
                {
                    user.FailedLoginCount = 0;
                    user.LockoutEnd = null;
                }

                user.FailedLoginCount++;
                var lockedNow = user.FailedLoginCount >= MaxFailedLoginAttempts;
                if (lockedNow)
                {
                    user.LockoutEnd = now.Add(LockoutDuration);
                    user.FailedLoginCount = 0;
                }

                user.LastModifiedDate = now;
                await _db.SaveChangesAsync(cancellationToken);

                await _activityLogger.LogAsync(new(ActivityAction.Login, false, user.Id, email, ActivityFailureReason.InvalidPassword), cancellationToken);
                if (lockedNow)
                    await _activityLogger.LogAsync(new(ActivityAction.AccountLocked, false, user.Id, email), cancellationToken);

                return BaseResponse<AuthResponse>.Failure("Invalid email or password.", statusCode: HttpStatusCode.Unauthorized);
            }

            if (!user.IsActive)
            {
                await _activityLogger.LogAsync(new(ActivityAction.Login, false, user.Id, email, ActivityFailureReason.InactiveUser), cancellationToken);
                return BaseResponse<AuthResponse>.Failure("Invalid email or password.", statusCode: HttpStatusCode.Unauthorized);
            }

            if (user.FailedLoginCount != 0 || user.LockoutEnd is not null)
            {
                user.FailedLoginCount = 0;
                user.LockoutEnd = null;
                user.LastModifiedDate = now;
            }

            var pair = await IssueTokensAsync(user, now, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);

            await _activityLogger.LogAsync(new(ActivityAction.Login, true, user.Id, email), cancellationToken);
            return BaseResponse<AuthResponse>.Success(pair, "Logged in successfully");
        }

        public async Task<BaseResponse<AuthResponse>> RefreshAsync(RefreshRequest request, CancellationToken cancellationToken = default)
        {
            var errors = Validate(_refreshValidator, request);
            if (errors is not null)
                return BaseResponse<AuthResponse>.Failure("One or more validation errors occurred", errors);

            var stored = await _db.RefreshTokens
                .Include(x => x.User)
                .ThenInclude(x => x.Role)
                .FirstOrDefaultAsync(x => x.Token == request.RefreshToken, cancellationToken);

            if (stored is null)
                return BaseResponse<AuthResponse>.Failure("Invalid refresh token.", statusCode: HttpStatusCode.Unauthorized);

            // Rotation reuse detection: a revoked token that was already rotated means theft.
            if (stored.Revoked is not null && stored.ReplacedByToken is not null)
            {
                await RevokeAllUserTokensAsync(stored.UserId, cancellationToken);
                await _db.SaveChangesAsync(cancellationToken);
                await _activityLogger.LogAsync(
                    new(ActivityAction.TokenReuseDetected, false, stored.UserId, stored.User.Email, ActivityFailureReason.TokenReused),
                    cancellationToken);
                return BaseResponse<AuthResponse>.Failure(
                    "Refresh token was already used. All sessions have been revoked, please log in again.",
                    statusCode: HttpStatusCode.Unauthorized);
            }

            if (!stored.IsActive || !stored.User.IsActive)
                return BaseResponse<AuthResponse>.Failure("Invalid refresh token.", statusCode: HttpStatusCode.Unauthorized);

            var now = DateTime.UtcNow;
            stored.Revoked = now;
            stored.LastModifiedDate = now;

            var pair = await IssueTokensAsync(stored.User, now, cancellationToken);
            stored.ReplacedByToken = pair.RefreshToken;

            try
            {
                await _db.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException)
            {
                // A parallel request rotated this token first; only one rotation may win.
                return BaseResponse<AuthResponse>.Failure("Invalid refresh token.", statusCode: HttpStatusCode.Unauthorized);
            }

            return BaseResponse<AuthResponse>.Success(pair, "Token refreshed successfully");
        }

        public async Task<BaseResponse<bool>> RevokeAsync(RevokeRequest request, CancellationToken cancellationToken = default)
        {
            var errors = Validate(_revokeValidator, request);
            if (errors is not null)
                return BaseResponse<bool>.Failure("One or more validation errors occurred", errors);

            var stored = await _db.RefreshTokens
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Token == request.RefreshToken, cancellationToken);

            // Idempotent: unknown tokens still return success so attackers can't probe which tokens exist.
            if (stored is not null && stored.Revoked is null)
            {
                var now = DateTime.UtcNow;
                stored.Revoked = now;
                stored.LastModifiedDate = now;

                try
                {
                    await _db.SaveChangesAsync(cancellationToken);
                    // Only a logout that actually revoked a token is logged; unknown tokens leave no trace.
                    await _activityLogger.LogAsync(new(ActivityAction.Logout, true, stored.UserId, stored.User.Email), cancellationToken);
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Already rotated or revoked by a parallel request: the token is unusable either way.
                }
            }

            return BaseResponse<bool>.Success(true, "Logged out successfully");
        }

        public async Task<BaseResponse<UserResponse>> GetMeAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var user = await _db.Users
                .Include(x => x.Role)
                .FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);

            if (user is null || !user.IsActive)
                return BaseResponse<UserResponse>.Failure("User not found.", statusCode: HttpStatusCode.NotFound);

            return BaseResponse<UserResponse>.Success(new UserResponse
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                Role = user.Role.Name,
                IsActive = user.IsActive
            });
        }

        public async Task<BaseResponse<bool>> ChangePasswordAsync(Guid userId, ChangePasswordRequest request, CancellationToken cancellationToken = default)
        {
            var errors = Validate(_changePasswordValidator, request);
            if (errors is not null)
                return BaseResponse<bool>.Failure("One or more validation errors occurred", errors);

            var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);

            if (user is null || !user.IsActive)
                return BaseResponse<bool>.Failure("User not found.", statusCode: HttpStatusCode.NotFound);

            // Keyed under currentPassword so the client can show it on the right field.
            if (_hasher.VerifyHashedPassword(user, user.PasswordHash, request.CurrentPassword) == PasswordVerificationResult.Failed)
            {
                await _activityLogger.LogAsync(
                    new(ActivityAction.ChangePassword, false, user.Id, user.Email, ActivityFailureReason.WrongCurrentPassword),
                    cancellationToken);
                return BaseResponse<bool>.Failure(
                    "Current password is incorrect.",
                    new Dictionary<string, List<string>>
                    {
                        ["currentPassword"] = ["Current password is incorrect."]
                    },
                    statusCode: HttpStatusCode.BadRequest);
            }

            var now = DateTime.UtcNow;
            user.PasswordHash = _hasher.HashPassword(user, request.NewPassword);
            user.FailedLoginCount = 0;
            user.LockoutEnd = null;
            user.LastModifiedDate = now;

            // Ending every session is the point: if the old password leaked, this locks the
            // other party out. Access tokens already issued live until they expire, so the
            // window is AccessTokenMinutes.
            //
            // ExecuteUpdate runs outside the SaveChanges transaction, so it goes first:
            // revoking and then failing to save only forces a re-login with the old password,
            // whereas saving and then failing to revoke would leave the old sessions alive.
            await RevokeAllUserTokensAsync(user.Id, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);

            await _activityLogger.LogAsync(new(ActivityAction.ChangePassword, true, user.Id, user.Email), cancellationToken);
            return BaseResponse<bool>.Success(true, "Password changed successfully. Please log in again.");
        }

        public async Task<BaseResponse<UserResponse>> UpdateProfileAsync(Guid userId, UpdateProfileRequest request, CancellationToken cancellationToken = default)
        {
            var errors = Validate(_updateProfileValidator, request);
            if (errors is not null)
                return BaseResponse<UserResponse>.Failure("One or more validation errors occurred", errors);

            var user = await _db.Users.Include(x => x.Role).FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);
            if (user is null || !user.IsActive)
                return BaseResponse<UserResponse>.Failure("User not found.", statusCode: HttpStatusCode.NotFound);

            user.FullName = request.FullName.Trim();
            user.LastModifiedDate = DateTime.UtcNow;
            await _db.SaveChangesAsync(cancellationToken);
            await _activityLogger.LogAsync(new(ActivityAction.UpdateProfile, true, user.Id, user.Email), cancellationToken);

            return BaseResponse<UserResponse>.Success(ToUserResponse(user), "Profile updated successfully");
        }

        public async Task<BaseResponse<List<SessionResponse>>> GetSessionsAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            if (!await _db.Users.AnyAsync(x => x.Id == userId && x.IsActive, cancellationToken))
                return BaseResponse<List<SessionResponse>>.Failure("User not found.", statusCode: HttpStatusCode.NotFound);

            var now = DateTime.UtcNow;
            var sessions = await _db.RefreshTokens.AsNoTracking()
                .Where(x => x.UserId == userId && x.Revoked == null && x.Expires > now)
                .OrderByDescending(x => x.CreatedDate)
                .Select(x => new SessionResponse
                {
                    Id = x.Id,
                    CreatedDate = x.CreatedDate,
                    Expires = x.Expires,
                    IpAddress = x.IpAddress,
                    UserAgent = x.UserAgent
                })
                .ToListAsync(cancellationToken);

            return BaseResponse<List<SessionResponse>>.Success(sessions);
        }

        public async Task<BaseResponse<bool>> RevokeSessionAsync(Guid userId, string sessionId, CancellationToken cancellationToken = default)
        {
            if (!Guid.TryParse(sessionId, out var parsedSessionId))
                return BaseResponse<bool>.Failure("One or more validation errors occurred", InvalidId("sessionId"));

            var session = await _db.RefreshTokens
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == parsedSessionId && x.UserId == userId, cancellationToken);
            if (session is null)
                return BaseResponse<bool>.Failure("Session not found.", statusCode: HttpStatusCode.NotFound);

            if (session.Revoked is null)
            {
                var now = DateTime.UtcNow;
                session.Revoked = now;
                session.LastModifiedDate = now;
                try
                {
                    await _db.SaveChangesAsync(cancellationToken);
                }
                catch (DbUpdateConcurrencyException)
                {
                    return BaseResponse<bool>.Failure("Session has already changed.", statusCode: HttpStatusCode.Conflict);
                }

                await _activityLogger.LogAsync(new(ActivityAction.RevokeSession, true, userId, session.User.Email), cancellationToken);
            }

            return BaseResponse<bool>.Success(true, "Session revoked successfully");
        }

        public async Task<BaseResponse<bool>> RevokeAllSessionsAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var user = await _db.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);
            if (user is null)
                return BaseResponse<bool>.Failure("User not found.", statusCode: HttpStatusCode.NotFound);

            await RevokeAllUserTokensAsync(userId, cancellationToken);
            await _activityLogger.LogAsync(new(ActivityAction.RevokeSession, true, userId, user.Email), cancellationToken);
            return BaseResponse<bool>.Success(true, "All sessions revoked successfully");
        }

        public async Task<BaseResponse<bool>> ForgotPasswordAsync(ForgotPasswordRequest request, CancellationToken cancellationToken = default)
        {
            var errors = Validate(_forgotPasswordValidator, request);
            if (errors is not null)
                return BaseResponse<bool>.Failure("One or more validation errors occurred", errors);

            var email = request.Email.Trim().ToLowerInvariant();
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Email == email && x.IsActive, cancellationToken);
            if (user is not null)
            {
                await using var transaction = await _db.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
                var now = DateTime.UtcNow;
                await _db.PasswordResetTokens
                    .Where(x => x.UserId == user.Id && x.UsedDate == null && x.Expires > now)
                    .ExecuteUpdateAsync(x => x.SetProperty(t => t.UsedDate, now), cancellationToken);

                var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(48));
                _db.PasswordResetTokens.Add(new PasswordResetToken
                {
                    Id = Guid.NewGuid(),
                    UserId = user.Id,
                    TokenHash = HashToken(token),
                    Expires = now.AddMinutes(30),
                    CreatedDate = now
                });
                await _db.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
                await _passwordResetNotifier.SendAsync(email, token, cancellationToken);
                await _activityLogger.LogAsync(new(ActivityAction.ForgotPassword, true, user.Id, email), cancellationToken);
            }

            return BaseResponse<bool>.Success(true, "If the email is registered, password reset instructions have been sent.");
        }

        public async Task<BaseResponse<bool>> ResetPasswordAsync(ResetPasswordRequest request, CancellationToken cancellationToken = default)
        {
            var errors = Validate(_resetPasswordValidator, request);
            if (errors is not null)
                return BaseResponse<bool>.Failure("One or more validation errors occurred", errors);

            await using var transaction = await _db.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
            var hash = HashToken(request.Token);
            var resetToken = await _db.PasswordResetTokens
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.TokenHash == hash, cancellationToken);
            var now = DateTime.UtcNow;

            if (resetToken is null || resetToken.UsedDate is not null || resetToken.Expires <= now || !resetToken.User.IsActive)
            {
                await _activityLogger.LogAsync(new(ActivityAction.ResetPassword, false, resetToken?.UserId, resetToken?.User.Email, ActivityFailureReason.InvalidResetToken), cancellationToken);
                return BaseResponse<bool>.Failure("Invalid or expired password reset token.", statusCode: HttpStatusCode.BadRequest);
            }

            resetToken.User.PasswordHash = _hasher.HashPassword(resetToken.User, request.NewPassword);
            resetToken.User.FailedLoginCount = 0;
            resetToken.User.LockoutEnd = null;
            resetToken.User.LastModifiedDate = now;
            resetToken.UsedDate = now;
            resetToken.LastModifiedDate = now;
            await RevokeAllUserTokensAsync(resetToken.UserId, cancellationToken);
            await _db.PasswordResetTokens
                .Where(x => x.UserId == resetToken.UserId && x.Id != resetToken.Id && x.UsedDate == null)
                .ExecuteUpdateAsync(x => x.SetProperty(t => t.UsedDate, now), cancellationToken);

            try
            {
                await _db.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException)
            {
                _db.ChangeTracker.Clear();
                await _activityLogger.LogAsync(new(ActivityAction.ResetPassword, false, resetToken.UserId, resetToken.User.Email, ActivityFailureReason.InvalidResetToken), cancellationToken);
                return BaseResponse<bool>.Failure("Invalid or expired password reset token.", statusCode: HttpStatusCode.BadRequest);
            }

            await _activityLogger.LogAsync(new(ActivityAction.ResetPassword, true, resetToken.UserId, resetToken.User.Email), cancellationToken);
            return BaseResponse<bool>.Success(true, "Password reset successfully. Please log in again.");
        }

        private async Task<AuthResponse> IssueTokensAsync(User user, DateTime now, CancellationToken cancellationToken)
        {
            var permissions = await GetPermissionsAsync(user.RoleId, cancellationToken);
            var (accessToken, expires) = _tokens.CreateAccessToken(user, permissions);
            var refreshToken = _tokens.CreateRefreshToken();

            _db.RefreshTokens.Add(new RefreshToken
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                Token = refreshToken,
                Expires = now.AddDays(_jwt.RefreshTokenDays),
                IpAddress = Truncate(_currentUser.IpAddress, 45),
                UserAgent = Truncate(_currentUser.UserAgent, 512),
                CreatedDate = now
            });

            return new AuthResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresIn = (long)(expires - now).TotalSeconds
            };
        }

        // Read on every token issue, so a refresh always carries the role's current grants.
        private async Task<List<string>> GetPermissionsAsync(Guid roleId, CancellationToken cancellationToken)
        {
            var codes = await _db.RolePermissions
                .Where(x => x.RoleId == roleId && !x.Permission.IsObsolete && !x.Permission.IsDeleted)
                .Select(x => x.Permission.Code)
                .ToListAsync(cancellationToken);

            return PermissionMatcher.Compress(codes);
        }

        private Task RevokeAllUserTokensAsync(Guid userId, CancellationToken cancellationToken)
        {
            return _db.RefreshTokens
                .Where(x => x.UserId == userId && x.Revoked == null)
                .ExecuteUpdateAsync(x => x.SetProperty(t => t.Revoked, DateTime.UtcNow), cancellationToken);
        }

        private static Dictionary<string, List<string>>? Validate<T>(IValidator<T> validator, T request)
            => ValidationErrors.From(validator.Validate(request));

        private static UserResponse ToUserResponse(User user) => new()
        {
            Id = user.Id,
            Email = user.Email,
            FullName = user.FullName,
            Role = user.Role.Name,
            IsActive = user.IsActive
        };

        private static string HashToken(string token)
            => Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(token)));

        private static string? Truncate(string? value, int maxLength)
            => value is not null && value.Length > maxLength ? value[..maxLength] : value;

        private static Dictionary<string, List<string>> InvalidId(string field)
            => new() { [field] = ["Id must be a valid GUID."] };
    }
}

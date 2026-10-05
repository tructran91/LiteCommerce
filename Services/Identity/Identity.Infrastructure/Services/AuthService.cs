using FluentValidation;
using Identity.Application.DTOs;
using Identity.Application.Interfaces;
using Identity.Application.Options;
using Identity.Core.Constants;
using Identity.Core.Entities;
using Identity.Infrastructure.Data;
using LiteCommerce.Shared.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Net;

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

        public AuthService(
            IdentityDbContext db,
            ITokenService tokens,
            IPasswordHasher<User> hasher,
            IOptions<JwtOptions> jwt,
            IValidator<RegisterRequest> registerValidator,
            IValidator<LoginRequest> loginValidator,
            IValidator<RefreshRequest> refreshValidator,
            IValidator<RevokeRequest> revokeValidator)
        {
            _db = db;
            _tokens = tokens;
            _hasher = hasher;
            _jwt = jwt.Value;
            _registerValidator = registerValidator;
            _loginValidator = loginValidator;
            _refreshValidator = refreshValidator;
            _revokeValidator = revokeValidator;
        }

        public async Task<BaseResponse<AuthResponse>> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
        {
            var errors = Validate(_registerValidator, request);
            if (errors is not null)
                return BaseResponse<AuthResponse>.Failure("One or more validation errors occurred", errors);

            var email = request.Email.Trim().ToLowerInvariant();
            if (await _db.Users.AnyAsync(x => x.Email == email, cancellationToken))
                return BaseResponse<AuthResponse>.Failure(
                    $"Email \"{email}\" is already registered.",
                    statusCode: HttpStatusCode.Conflict);

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
            var pair = IssueTokens(user, now);

            try
            {
                await _db.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException)
            {
                // Lost a race with a concurrent register for the same email (unique index). Anything else is a real error.
                _db.ChangeTracker.Clear();
                if (await _db.Users.AnyAsync(x => x.Email == email, cancellationToken))
                    return BaseResponse<AuthResponse>.Failure(
                        $"Email \"{email}\" is already registered.",
                        statusCode: HttpStatusCode.Conflict);
                throw;
            }

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
                return BaseResponse<AuthResponse>.Failure("Invalid email or password.", statusCode: HttpStatusCode.Unauthorized);
            }

            var now = DateTime.UtcNow;
            if (user.LockoutEnd is not null && user.LockoutEnd > now)
                return BaseResponse<AuthResponse>.Failure(
                    "Too many failed login attempts. Please try again later.",
                    statusCode: HttpStatusCode.TooManyRequests);

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
                if (user.FailedLoginCount >= MaxFailedLoginAttempts)
                {
                    user.LockoutEnd = now.Add(LockoutDuration);
                    user.FailedLoginCount = 0;
                }

                user.LastModifiedDate = now;
                await _db.SaveChangesAsync(cancellationToken);

                return BaseResponse<AuthResponse>.Failure("Invalid email or password.", statusCode: HttpStatusCode.Unauthorized);
            }

            if (!user.IsActive)
                return BaseResponse<AuthResponse>.Failure("Invalid email or password.", statusCode: HttpStatusCode.Unauthorized);

            if (user.FailedLoginCount != 0 || user.LockoutEnd is not null)
            {
                user.FailedLoginCount = 0;
                user.LockoutEnd = null;
                user.LastModifiedDate = now;
            }

            var pair = IssueTokens(user, now);
            await _db.SaveChangesAsync(cancellationToken);

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
                return BaseResponse<AuthResponse>.Failure(
                    "Refresh token was already used. All sessions have been revoked, please log in again.",
                    statusCode: HttpStatusCode.Unauthorized);
            }

            if (!stored.IsActive || !stored.User.IsActive)
                return BaseResponse<AuthResponse>.Failure("Invalid refresh token.", statusCode: HttpStatusCode.Unauthorized);

            var now = DateTime.UtcNow;
            stored.Revoked = now;
            stored.LastModifiedDate = now;

            var pair = IssueTokens(stored.User, now);
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

        private AuthResponse IssueTokens(User user, DateTime now)
        {
            var (accessToken, expires) = _tokens.CreateAccessToken(user);
            var refreshToken = _tokens.CreateRefreshToken();

            _db.RefreshTokens.Add(new RefreshToken
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                Token = refreshToken,
                Expires = now.AddDays(_jwt.RefreshTokenDays),
                CreatedDate = now
            });

            return new AuthResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresIn = (long)(expires - now).TotalSeconds
            };
        }

        private Task RevokeAllUserTokensAsync(Guid userId, CancellationToken cancellationToken)
        {
            return _db.RefreshTokens
                .Where(x => x.UserId == userId && x.Revoked == null)
                .ExecuteUpdateAsync(x => x.SetProperty(t => t.Revoked, DateTime.UtcNow), cancellationToken);
        }

        private static Dictionary<string, List<string>>? Validate<T>(IValidator<T> validator, T request)
        {
            var result = validator.Validate(request);
            if (result.IsValid)
                return null;

            // Same error shape as Catalog: keyed by camelCase field name.
            return result.Errors
                .GroupBy(x => LowerFirst(x.PropertyName))
                .ToDictionary(
                    x => x.Key,
                    x => x.Select(e => e.ErrorMessage).ToList());
        }

        private static string LowerFirst(string value)
            => string.IsNullOrEmpty(value) ? value : char.ToLowerInvariant(value[0]) + value[1..];
    }
}

using FluentValidation;
using Identity.Application.DTOs;
using Identity.Application.Interfaces;
using Identity.Core.Entities;
using Identity.Core.Enums;
using Identity.Infrastructure.Data;
using LiteCommerce.Shared.Authorization;
using LiteCommerce.Shared.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Net;

namespace Identity.Infrastructure.Services
{
    public class AdminUserService : IAdminUserService
    {
        private readonly IdentityDbContext _db;
        private readonly IPasswordHasher<User> _hasher;
        private readonly IActivityLogger _activityLogger;
        private readonly IValidator<GetUsersQuery> _getUsersValidator;
        private readonly IValidator<CreateUserRequest> _createUserValidator;
        private readonly IValidator<UpdateUserRequest> _updateUserValidator;
        private readonly IValidator<UpdateUserRoleRequest> _updateRoleValidator;

        public AdminUserService(
            IdentityDbContext db,
            IPasswordHasher<User> hasher,
            IActivityLogger activityLogger,
            IValidator<GetUsersQuery> getUsersValidator,
            IValidator<CreateUserRequest> createUserValidator,
            IValidator<UpdateUserRequest> updateUserValidator,
            IValidator<UpdateUserRoleRequest> updateRoleValidator)
        {
            _db = db;
            _hasher = hasher;
            _activityLogger = activityLogger;
            _getUsersValidator = getUsersValidator;
            _createUserValidator = createUserValidator;
            _updateUserValidator = updateUserValidator;
            _updateRoleValidator = updateRoleValidator;
        }

        public async Task<BaseResponse<List<AdminUserResponse>>> GetUsersAsync(GetUsersQuery query, CancellationToken cancellationToken = default)
        {
            var errors = ValidationErrors.From(_getUsersValidator.Validate(query));
            if (errors is not null)
                return BaseResponse<List<AdminUserResponse>>.Failure("One or more validation errors occurred", errors);

            var users = _db.Users.AsNoTracking().Include(x => x.Role).AsQueryable();
            var now = DateTime.UtcNow;

            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                var search = query.Search.Trim();
                users = users.Where(x => x.Email.Contains(search) || x.FullName.Contains(search));
            }
            if (!string.IsNullOrWhiteSpace(query.Role))
            {
                var role = query.Role.Trim();
                users = users.Where(x => x.Role.Name == role);
            }
            if (query.IsActive.HasValue)
                users = users.Where(x => x.IsActive == query.IsActive.Value);
            if (query.IsLocked.HasValue)
                users = query.IsLocked.Value
                    ? users.Where(x => x.LockoutEnd != null && x.LockoutEnd > now)
                    : users.Where(x => x.LockoutEnd == null || x.LockoutEnd <= now);

            var totalRecords = await users.CountAsync(cancellationToken);
            var page = await users.OrderByDescending(x => x.CreatedDate)
                .Skip((query.CurrentPage - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync(cancellationToken);
            var items = page.Select(x => ToResponse(x, now)).ToList();

            var response = BaseResponse<List<AdminUserResponse>>.Success(items);
            response.Pagination = new Pagination(totalRecords, query.CurrentPage, query.PageSize);
            return response;
        }

        public async Task<BaseResponse<AdminUserResponse>> GetUserAsync(string id, CancellationToken cancellationToken = default)
        {
            if (!TryParseId<AdminUserResponse>(id, out var userId, out var failure))
                return failure!;

            var user = await _db.Users.AsNoTracking().Include(x => x.Role)
                .FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);
            return user is null
                ? BaseResponse<AdminUserResponse>.Failure("User not found.", statusCode: HttpStatusCode.NotFound)
                : BaseResponse<AdminUserResponse>.Success(ToResponse(user, DateTime.UtcNow));
        }

        public async Task<BaseResponse<AdminUserResponse>> CreateUserAsync(CreateUserRequest request, CancellationToken cancellationToken = default)
        {
            var errors = ValidationErrors.From(_createUserValidator.Validate(request));
            if (errors is not null)
                return BaseResponse<AdminUserResponse>.Failure("One or more validation errors occurred", errors);

            var email = request.Email.Trim().ToLowerInvariant();
            if (await _db.Users.AnyAsync(x => x.Email == email, cancellationToken))
                return BaseResponse<AdminUserResponse>.Failure($"Email \"{email}\" is already registered.", statusCode: HttpStatusCode.Conflict);

            var role = await _db.Roles.FirstOrDefaultAsync(x => x.Id == request.RoleId, cancellationToken);
            if (role is null)
                return BaseResponse<AdminUserResponse>.Failure("Role not found.", statusCode: HttpStatusCode.NotFound);

            var now = DateTime.UtcNow;
            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = email,
                FullName = request.FullName.Trim(),
                RoleId = role.Id,
                Role = role,
                IsActive = request.IsActive,
                CreatedDate = now
            };
            user.PasswordHash = _hasher.HashPassword(user, request.Password);
            _db.Users.Add(user);

            try
            {
                await _db.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException)
            {
                _db.ChangeTracker.Clear();
                if (await _db.Users.AnyAsync(x => x.Email == email, cancellationToken))
                    return BaseResponse<AdminUserResponse>.Failure($"Email \"{email}\" is already registered.", statusCode: HttpStatusCode.Conflict);
                throw;
            }

            return BaseResponse<AdminUserResponse>.Success(ToResponse(user, now), "User created successfully", HttpStatusCode.Created);
        }

        public async Task<BaseResponse<AdminUserResponse>> UpdateUserAsync(string id, UpdateUserRequest request, CancellationToken cancellationToken = default)
        {
            if (!TryParseId<AdminUserResponse>(id, out var userId, out var failure))
                return failure!;
            var errors = ValidationErrors.From(_updateUserValidator.Validate(request));
            if (errors is not null)
                return BaseResponse<AdminUserResponse>.Failure("One or more validation errors occurred", errors);

            await using var transaction = await _db.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
            var user = await _db.Users.Include(x => x.Role).FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);
            if (user is null)
                return BaseResponse<AdminUserResponse>.Failure("User not found.", statusCode: HttpStatusCode.NotFound);

            user.FullName = request.FullName.Trim();
            user.LastModifiedDate = DateTime.UtcNow;
            await _db.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return BaseResponse<AdminUserResponse>.Success(ToResponse(user, DateTime.UtcNow), "User updated successfully");
        }

        public async Task<BaseResponse<bool>> DeleteUserAsync(Guid actorUserId, string id, CancellationToken cancellationToken = default)
        {
            if (!TryParseId<bool>(id, out var userId, out BaseResponse<bool>? failure))
                return failure!;
            if (userId == actorUserId)
                return BaseResponse<bool>.Failure("You cannot delete your own account.", statusCode: HttpStatusCode.Conflict);

            await using var transaction = await _db.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
            var user = await _db.Users.Include(x => x.Role).FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);
            if (user is null)
                return BaseResponse<bool>.Failure("User not found.", statusCode: HttpStatusCode.NotFound);
            if (await IsLastActiveAdminAsync(user, cancellationToken))
                return BaseResponse<bool>.Failure("The last active administrator cannot be deleted.", statusCode: HttpStatusCode.Conflict);

            var now = DateTime.UtcNow;
            user.IsDeleted = true;
            user.IsActive = false;
            user.LastModifiedDate = now;
            await RevokeAllSessionsAsync(user.Id, now, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            return BaseResponse<bool>.Success(true, "User deleted successfully");
        }

        public async Task<BaseResponse<AdminUserResponse>> UpdateStatusAsync(Guid actorUserId, string id, UpdateUserStatusRequest request, CancellationToken cancellationToken = default)
        {
            if (!TryParseId<AdminUserResponse>(id, out var userId, out var failure))
                return failure!;
            if (userId == actorUserId && !request.IsActive)
                return BaseResponse<AdminUserResponse>.Failure("You cannot deactivate your own account.", statusCode: HttpStatusCode.Conflict);

            await using var transaction = await _db.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
            var user = await _db.Users.Include(x => x.Role).FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);
            if (user is null)
                return BaseResponse<AdminUserResponse>.Failure("User not found.", statusCode: HttpStatusCode.NotFound);
            if (!request.IsActive && await IsLastActiveAdminAsync(user, cancellationToken))
                return BaseResponse<AdminUserResponse>.Failure("The last active administrator cannot be deactivated.", statusCode: HttpStatusCode.Conflict);

            var now = DateTime.UtcNow;
            user.IsActive = request.IsActive;
            user.LastModifiedDate = now;
            if (!request.IsActive)
                await RevokeAllSessionsAsync(user.Id, now, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            return BaseResponse<AdminUserResponse>.Success(ToResponse(user, now), "User status updated successfully");
        }

        public async Task<BaseResponse<AdminUserResponse>> UpdateRoleAsync(Guid actorUserId, string id, UpdateUserRoleRequest request, CancellationToken cancellationToken = default)
        {
            if (!TryParseId<AdminUserResponse>(id, out var userId, out var failure))
                return failure!;
            var errors = ValidationErrors.From(_updateRoleValidator.Validate(request));
            if (errors is not null)
                return BaseResponse<AdminUserResponse>.Failure("One or more validation errors occurred", errors);

            await using var transaction = await _db.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
            var user = await _db.Users.Include(x => x.Role).FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);
            if (user is null)
                return BaseResponse<AdminUserResponse>.Failure("User not found.", statusCode: HttpStatusCode.NotFound);
            var role = await _db.Roles.FirstOrDefaultAsync(x => x.Id == request.RoleId, cancellationToken);
            if (role is null)
                return BaseResponse<AdminUserResponse>.Failure("Role not found.", statusCode: HttpStatusCode.NotFound);
            var targetRoleIsAdmin = await RoleCanManagePermissionsAsync(role.Id, cancellationToken);
            if (userId == actorUserId && !targetRoleIsAdmin)
                return BaseResponse<AdminUserResponse>.Failure("You cannot remove your own administrator role.", statusCode: HttpStatusCode.Conflict);
            if (!targetRoleIsAdmin && await IsLastActiveAdminAsync(user, cancellationToken))
                return BaseResponse<AdminUserResponse>.Failure("The last active administrator cannot change role.", statusCode: HttpStatusCode.Conflict);

            var now = DateTime.UtcNow;
            user.RoleId = role.Id;
            user.Role = role;
            user.LastModifiedDate = now;
            await RevokeAllSessionsAsync(user.Id, now, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            return BaseResponse<AdminUserResponse>.Success(ToResponse(user, now), "User role updated successfully");
        }

        public async Task<BaseResponse<bool>> UnlockAsync(string id, CancellationToken cancellationToken = default)
        {
            if (!TryParseId<bool>(id, out var userId, out BaseResponse<bool>? failure))
                return failure!;
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);
            if (user is null)
                return BaseResponse<bool>.Failure("User not found.", statusCode: HttpStatusCode.NotFound);

            user.FailedLoginCount = 0;
            user.LockoutEnd = null;
            user.LastModifiedDate = DateTime.UtcNow;
            await _db.SaveChangesAsync(cancellationToken);
            await _activityLogger.LogAsync(new(ActivityAction.AdminUnlockUser, true, user.Id, user.Email), cancellationToken);
            return BaseResponse<bool>.Success(true, "User unlocked successfully");
        }

        public async Task<BaseResponse<bool>> RevokeSessionsAsync(string id, CancellationToken cancellationToken = default)
        {
            if (!TryParseId<bool>(id, out var userId, out BaseResponse<bool>? failure))
                return failure!;
            var user = await _db.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);
            if (user is null)
                return BaseResponse<bool>.Failure("User not found.", statusCode: HttpStatusCode.NotFound);

            await using var transaction = await _db.Database.BeginTransactionAsync(cancellationToken);
            await RevokeAllSessionsAsync(user.Id, DateTime.UtcNow, cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            await _activityLogger.LogAsync(new(ActivityAction.AdminRevokeSessions, true, user.Id, user.Email), cancellationToken);
            return BaseResponse<bool>.Success(true, "All user sessions revoked successfully");
        }

        private Task<int> RevokeAllSessionsAsync(Guid userId, DateTime now, CancellationToken cancellationToken)
            => _db.RefreshTokens.Where(x => x.UserId == userId && x.Revoked == null)
                .ExecuteUpdateAsync(x => x.SetProperty(t => t.Revoked, now), cancellationToken);

        // "Administrator" means holding the permission that can hand out permissions, not
        // being in a role called Admin: roles are user-defined now.
        private async Task<bool> IsLastActiveAdminAsync(User user, CancellationToken cancellationToken)
        {
            if (!user.IsActive || !await RoleCanManagePermissionsAsync(user.RoleId, cancellationToken))
                return false;

            return await CountActiveAdminsAsync(cancellationToken) <= 1;
        }

        private Task<bool> RoleCanManagePermissionsAsync(Guid roleId, CancellationToken cancellationToken)
            => _db.RolePermissions.AnyAsync(
                x => x.RoleId == roleId && x.Permission.Code == AppPermissions.Role.UpdatePermissions,
                cancellationToken);

        private Task<int> CountActiveAdminsAsync(CancellationToken cancellationToken)
            => _db.Users.CountAsync(
                x => x.IsActive
                     && x.Role.RolePermissions.Any(rp => rp.Permission.Code == AppPermissions.Role.UpdatePermissions),
                cancellationToken);

        private static AdminUserResponse ToResponse(User user, DateTime now) => new()
        {
            Id = user.Id,
            Email = user.Email,
            FullName = user.FullName,
            RoleId = user.RoleId,
            Role = user.Role.Name,
            IsActive = user.IsActive,
            IsLocked = user.LockoutEnd is not null && user.LockoutEnd > now,
            LockoutEnd = user.LockoutEnd,
            CreatedDate = user.CreatedDate,
            LastModifiedDate = user.LastModifiedDate
        };

        private static bool TryParseId<T>(string id, out Guid parsedId, out BaseResponse<T>? failure)
        {
            if (Guid.TryParse(id, out parsedId))
            {
                failure = null;
                return true;
            }

            failure = BaseResponse<T>.Failure(
                "One or more validation errors occurred",
                new Dictionary<string, List<string>> { ["id"] = ["Id must be a valid GUID."] });
            return false;
        }
    }
}

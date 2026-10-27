using FluentValidation;
using Identity.Application.DTOs;
using Identity.Application.Interfaces;
using Identity.Core.Entities;
using Identity.Core.Enums;
using Identity.Infrastructure.Data;
using LiteCommerce.Shared.Authorization;
using LiteCommerce.Shared.Constants;
using LiteCommerce.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Net;

namespace Identity.Infrastructure.Services
{
    public class AdminRoleService : IAdminRoleService
    {
        private readonly IdentityDbContext _db;
        private readonly IActivityLogger _activityLogger;
        private readonly IValidator<CreateRoleRequest> _createValidator;
        private readonly IValidator<UpdateRoleRequest> _updateValidator;
        private readonly IValidator<UpdateRolePermissionsRequest> _updatePermissionsValidator;

        public AdminRoleService(
            IdentityDbContext db,
            IActivityLogger activityLogger,
            IValidator<CreateRoleRequest> createValidator,
            IValidator<UpdateRoleRequest> updateValidator,
            IValidator<UpdateRolePermissionsRequest> updatePermissionsValidator)
        {
            _db = db;
            _activityLogger = activityLogger;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _updatePermissionsValidator = updatePermissionsValidator;
        }

        public async Task<BaseResponse<List<RoleDetailResponse>>> GetRolesAsync(CancellationToken cancellationToken = default)
        {
            var roles = await _db.Roles.AsNoTracking()
                .OrderBy(x => x.Name)
                .Select(x => new RoleDetailResponse
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    IsSystem = x.IsSystem,
                    UserCount = x.Users.Count(u => !u.IsDeleted),
                    PermissionCodes = x.RolePermissions
                        .Where(rp => !rp.Permission.IsObsolete && !rp.Permission.IsDeleted)
                        .Select(rp => rp.Permission.Code)
                        .ToList(),
                    CreatedDate = x.CreatedDate,
                    LastModifiedDate = x.LastModifiedDate
                })
                .ToListAsync(cancellationToken);

            return BaseResponse<List<RoleDetailResponse>>.Success(roles);
        }

        public async Task<BaseResponse<RoleDetailResponse>> GetRoleAsync(string id, CancellationToken cancellationToken = default)
        {
            if (!TryParseId<RoleDetailResponse>(id, out var roleId, out var failure))
                return failure!;

            var role = await LoadResponseAsync(roleId, cancellationToken);
            return role is null
                ? NotFound<RoleDetailResponse>()
                : BaseResponse<RoleDetailResponse>.Success(role);
        }

        public async Task<BaseResponse<RoleDetailResponse>> CreateRoleAsync(CreateRoleRequest request, CancellationToken cancellationToken = default)
        {
            var errors = ValidationErrors.From(_createValidator.Validate(request));
            if (errors is not null)
                return BaseResponse<RoleDetailResponse>.Failure("One or more validation errors occurred", errors);

            var name = request.Name.Trim();

            await using var transaction = await _db.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);

            if (await _db.Roles.AnyAsync(x => x.Name == name, cancellationToken))
                return Conflict<RoleDetailResponse>($"Role \"{name}\" already exists.");

            if (!TryResolvePermissions(request.PermissionCodes, out var permissionIds, out var unknown))
                return UnknownPermissions<RoleDetailResponse>(unknown);

            var now = DateTime.UtcNow;
            var role = new Role
            {
                Id = Guid.NewGuid(),
                Name = name,
                Description = request.Description?.Trim(),
                CreatedDate = now
            };
            _db.Roles.Add(role);

            foreach (var permissionId in permissionIds)
                _db.RolePermissions.Add(new RolePermission { RoleId = role.Id, PermissionId = permissionId });

            try
            {
                await _db.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
            }
            catch (DbUpdateException)
            {
                // Lost a race on the unique name index.
                _db.ChangeTracker.Clear();
                if (await _db.Roles.AnyAsync(x => x.Name == name, cancellationToken))
                    return Conflict<RoleDetailResponse>($"Role \"{name}\" already exists.");
                throw;
            }

            await _activityLogger.LogAsync(new(ActivityAction.AdminCreateRole, true, null, null), cancellationToken);

            var created = await LoadResponseAsync(role.Id, cancellationToken);
            return BaseResponse<RoleDetailResponse>.Success(created!, "Role created successfully", HttpStatusCode.Created);
        }

        public async Task<BaseResponse<RoleDetailResponse>> UpdateRoleAsync(string id, UpdateRoleRequest request, CancellationToken cancellationToken = default)
        {
            if (!TryParseId<RoleDetailResponse>(id, out var roleId, out var failure))
                return failure!;

            var errors = ValidationErrors.From(_updateValidator.Validate(request));
            if (errors is not null)
                return BaseResponse<RoleDetailResponse>.Failure("One or more validation errors occurred", errors);

            var name = request.Name.Trim();

            await using var transaction = await _db.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);

            var role = await _db.Roles.FirstOrDefaultAsync(x => x.Id == roleId, cancellationToken);
            if (role is null)
                return NotFound<RoleDetailResponse>();

            // Code looks system roles up by name (AuthService.RegisterAsync, IdentityDbInitializer),
            // so renaming one would break registration or admin seeding.
            if (role.IsSystem && !string.Equals(role.Name, name, StringComparison.Ordinal))
                return Conflict<RoleDetailResponse>($"System role \"{role.Name}\" cannot be renamed.");

            if (await _db.Roles.AnyAsync(x => x.Name == name && x.Id != roleId, cancellationToken))
                return Conflict<RoleDetailResponse>($"Role \"{name}\" already exists.");

            role.Name = name;
            role.Description = request.Description?.Trim();
            role.LastModifiedDate = DateTime.UtcNow;

            await _db.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            await _activityLogger.LogAsync(new(ActivityAction.AdminUpdateRole, true, null, null), cancellationToken);

            var updated = await LoadResponseAsync(roleId, cancellationToken);
            return BaseResponse<RoleDetailResponse>.Success(updated!, "Role updated successfully");
        }

        public async Task<BaseResponse<bool>> DeleteRoleAsync(string id, CancellationToken cancellationToken = default)
        {
            if (!TryParseId<bool>(id, out var roleId, out BaseResponse<bool>? failure))
                return failure!;

            await using var transaction = await _db.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);

            var role = await _db.Roles.FirstOrDefaultAsync(x => x.Id == roleId, cancellationToken);
            if (role is null)
                return NotFound<bool>();

            if (role.IsSystem)
                return Conflict<bool>($"System role \"{role.Name}\" cannot be deleted.");

            var userCount = await _db.Users.CountAsync(x => x.RoleId == roleId, cancellationToken);
            if (userCount > 0)
                return Conflict<bool>(ErrorMessages.CannotDeleteInUse("role", role.Name, userCount, "user", "users"));

            // Soft-delete the role, hard-delete its grants: a grant has no history worth keeping,
            // and leaving rows behind would resurrect them if the name were reused.
            var grants = await _db.RolePermissions.Where(x => x.RoleId == roleId).ToListAsync(cancellationToken);
            _db.RolePermissions.RemoveRange(grants);

            role.IsDeleted = true;
            role.LastModifiedDate = DateTime.UtcNow;

            await _db.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            await _activityLogger.LogAsync(new(ActivityAction.AdminDeleteRole, true, null, null), cancellationToken);
            return BaseResponse<bool>.Success(true, "Role deleted successfully");
        }

        public async Task<BaseResponse<List<PermissionGroupResponse>>> GetPermissionCatalogAsync(CancellationToken cancellationToken = default)
        {
            var rows = await _db.Permissions.AsNoTracking()
                .Where(x => !x.IsObsolete)
                .ToListAsync(cancellationToken);

            // AppPermissions.All defines the display order; the table only supplies what is
            // actually installed, so a permission added in code but not yet synced is hidden.
            var order = AppPermissions.All
                .Select((descriptor, index) => (descriptor.Code, index))
                .ToDictionary(x => x.Code, x => x.index, StringComparer.Ordinal);

            var groups = rows
                .Where(x => order.ContainsKey(x.Code))
                .OrderBy(x => order[x.Code])
                .GroupBy(x => (x.Service, x.GroupName))
                .Select(group => new PermissionGroupResponse
                {
                    Service = group.Key.Service,
                    GroupName = group.Key.GroupName,
                    Permissions = group.Select(x => new PermissionResponse
                    {
                        Code = x.Code,
                        DisplayName = x.DisplayName,
                        Description = x.Description
                    }).ToList()
                })
                .ToList();

            return BaseResponse<List<PermissionGroupResponse>>.Success(groups);
        }

        public async Task<BaseResponse<RoleDetailResponse>> UpdateRolePermissionsAsync(
            Guid actorUserId, string id, UpdateRolePermissionsRequest request, CancellationToken cancellationToken = default)
        {
            if (!TryParseId<RoleDetailResponse>(id, out var roleId, out var failure))
                return failure!;

            var errors = ValidationErrors.From(_updatePermissionsValidator.Validate(request));
            if (errors is not null)
                return BaseResponse<RoleDetailResponse>.Failure("One or more validation errors occurred", errors);

            await using var transaction = await _db.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);

            var role = await _db.Roles
                .Include(x => x.RolePermissions)
                .FirstOrDefaultAsync(x => x.Id == roleId, cancellationToken);
            if (role is null)
                return NotFound<RoleDetailResponse>();

            // The Admin role is the escape hatch when another role is misconfigured, so its
            // grants are fixed.
            if (role.IsSystem)
                return Conflict<RoleDetailResponse>($"Permissions of system role \"{role.Name}\" cannot be changed.");

            if (!TryResolvePermissions(request.PermissionCodes, out var permissionIds, out var unknown))
                return UnknownPermissions<RoleDetailResponse>(unknown);

            var keepsAdminRights = request.PermissionCodes.Contains(
                AppPermissions.Role.UpdatePermissions, StringComparer.Ordinal);

            if (!keepsAdminRights)
            {
                var actorHasThisRole = await _db.Users
                    .AnyAsync(x => x.Id == actorUserId && x.RoleId == roleId, cancellationToken);
                if (actorHasThisRole)
                    return Conflict<RoleDetailResponse>("You cannot remove your own permission to manage roles.");

                // Nobody else may be left who can hand out permissions, or the system is
                // permanently frozen in whatever state this request leaves behind.
                var othersCanManage = await _db.Users.AnyAsync(
                    x => x.IsActive
                         && x.RoleId != roleId
                         && x.Role.RolePermissions.Any(rp => rp.Permission.Code == AppPermissions.Role.UpdatePermissions),
                    cancellationToken);
                if (!othersCanManage)
                    return Conflict<RoleDetailResponse>(
                        "This would leave no active user able to manage permissions.");
            }

            _db.RolePermissions.RemoveRange(role.RolePermissions);
            foreach (var permissionId in permissionIds)
                _db.RolePermissions.Add(new RolePermission { RoleId = roleId, PermissionId = permissionId });

            role.LastModifiedDate = DateTime.UtcNow;
            await _db.SaveChangesAsync(cancellationToken);

            // Permissions live in the access token, so holders keep the old set until they are
            // reissued. Revoking forces that on the next refresh.
            await RevokeSessionsForRoleAsync(roleId, cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            await _activityLogger.LogAsync(new(ActivityAction.AdminUpdateRolePermissions, true, null, null), cancellationToken);

            var updated = await LoadResponseAsync(roleId, cancellationToken);
            return BaseResponse<RoleDetailResponse>.Success(updated!, "Role permissions updated successfully");
        }

        private Task<int> RevokeSessionsForRoleAsync(Guid roleId, CancellationToken cancellationToken)
        {
            var now = DateTime.UtcNow;
            return _db.RefreshTokens
                .Where(x => x.Revoked == null && x.User.RoleId == roleId)
                .ExecuteUpdateAsync(x => x.SetProperty(t => t.Revoked, now), cancellationToken);
        }

        private bool TryResolvePermissions(List<string> codes, out List<Guid> permissionIds, out List<string> unknown)
        {
            permissionIds = [];
            unknown = codes.Where(c => !AppPermissions.AllCodes.Contains(c)).ToList();
            if (unknown.Count > 0)
                return false;

            // Resolved against the table, not the catalog, because the grant is a foreign key.
            var installed = _db.Permissions
                .Where(x => !x.IsObsolete && codes.Contains(x.Code))
                .Select(x => new { x.Id, x.Code })
                .ToList();

            unknown = codes.Except(installed.Select(x => x.Code), StringComparer.Ordinal).ToList();
            if (unknown.Count > 0)
                return false;

            permissionIds = installed.Select(x => x.Id).ToList();
            return true;
        }

        private Task<RoleDetailResponse?> LoadResponseAsync(Guid roleId, CancellationToken cancellationToken)
            => _db.Roles.AsNoTracking()
                .Where(x => x.Id == roleId)
                .Select(x => new RoleDetailResponse
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    IsSystem = x.IsSystem,
                    UserCount = x.Users.Count(u => !u.IsDeleted),
                    PermissionCodes = x.RolePermissions
                        .Where(rp => !rp.Permission.IsObsolete && !rp.Permission.IsDeleted)
                        .Select(rp => rp.Permission.Code)
                        .ToList(),
                    CreatedDate = x.CreatedDate,
                    LastModifiedDate = x.LastModifiedDate
                })
                .FirstOrDefaultAsync(cancellationToken);

        private static BaseResponse<T> NotFound<T>()
            => BaseResponse<T>.Failure("Role not found.", statusCode: HttpStatusCode.NotFound);

        private static BaseResponse<T> Conflict<T>(string message)
            => BaseResponse<T>.Failure(message, statusCode: HttpStatusCode.Conflict);

        private static BaseResponse<T> UnknownPermissions<T>(List<string> unknown)
            => BaseResponse<T>.Failure(
                "One or more validation errors occurred",
                new Dictionary<string, List<string>>
                {
                    ["permissionCodes"] = [$"Unknown permission codes: {string.Join(", ", unknown)}."]
                });

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

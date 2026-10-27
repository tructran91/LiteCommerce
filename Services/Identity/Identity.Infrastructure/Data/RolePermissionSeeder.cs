using Identity.Core.Entities;
using Identity.Infrastructure.Data.Configurations;
using LiteCommerce.Shared.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Identity.Infrastructure.Data
{
    // Grants for the seeded roles. Runs after PermissionSyncer.
    public static class RolePermissionSeeder
    {
        public static async Task SeedAsync(IServiceProvider services, CancellationToken cancellationToken = default)
        {
            var logger = services.GetRequiredService<ILoggerFactory>().CreateLogger("Identity.Permissions");

            using var scope = services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();

            var permissionIds = await db.Permissions
                .ToDictionaryAsync(x => x.Code, x => x.Id, StringComparer.Ordinal, cancellationToken);

            // Admin is reconciled on every start, not just the first: a permission added in a
            // later release must reach Admin, or a new feature would be unreachable by anyone.
            var granted = await EnsureGrantsAsync(
                db, RoleConfiguration.AdminId, AppPermissions.AllCodes, permissionIds, cancellationToken);

            // Staff is seeded once and then belongs to whoever administers the system.
            if (!await db.RolePermissions.AnyAsync(x => x.RoleId == RoleConfiguration.StaffId, cancellationToken))
            {
                var staffCodes = AppPermissions.All
                    .Where(x => x.Service == AppPermissions.Services.Catalog
                                && x.Code != AppPermissions.Database.Seed
                                && x.Code != AppPermissions.Database.ViewProfiles
                                && x.Code != AppPermissions.CatalogLog.ViewAudit
                                && x.Code != AppPermissions.CatalogLog.ViewActivity
                                && x.Code != AppPermissions.IdentityLog.ViewAudit
                                && x.Code != AppPermissions.IdentityLog.ViewActivity)
                    .Select(x => x.Code)
                    .Append(AppPermissions.BackOffice.Access)
                    .ToHashSet(StringComparer.Ordinal);

                granted += await EnsureGrantsAsync(
                    db, RoleConfiguration.StaffId, staffCodes, permissionIds, cancellationToken);
            }

            // Customer gets nothing: it is a storefront role with no back-office access.

            if (granted == 0)
                return;

            await db.SaveChangesAsync(cancellationToken);
            logger.LogInformation("Role permission seed: {Granted} grants added.", granted);
        }

        private static async Task<int> EnsureGrantsAsync(
            IdentityDbContext db,
            Guid roleId,
            IReadOnlySet<string> codes,
            IReadOnlyDictionary<string, Guid> permissionIds,
            CancellationToken cancellationToken)
        {
            if (!await db.Roles.AnyAsync(x => x.Id == roleId, cancellationToken))
                return 0;

            var existing = await db.RolePermissions
                .Where(x => x.RoleId == roleId)
                .Select(x => x.PermissionId)
                .ToListAsync(cancellationToken);
            var existingIds = existing.ToHashSet();

            var added = 0;
            foreach (var code in codes)
            {
                if (!permissionIds.TryGetValue(code, out var permissionId) || existingIds.Contains(permissionId))
                    continue;

                db.RolePermissions.Add(new RolePermission { RoleId = roleId, PermissionId = permissionId });
                added++;
            }

            return added;
        }
    }
}

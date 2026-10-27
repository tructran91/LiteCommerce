using Identity.Core.Entities;
using LiteCommerce.Shared.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Identity.Infrastructure.Data
{
    // Mirrors AppPermissions into the Permissions table on startup, so adding a permission in
    // code needs no migration. Must run before RolePermissionSeeder and before the first
    // request, since grants are foreign keys into this table.
    public static class PermissionSyncer
    {
        public static async Task SyncAsync(IServiceProvider services, CancellationToken cancellationToken = default)
        {
            var logger = services.GetRequiredService<ILoggerFactory>().CreateLogger("Identity.Permissions");

            using var scope = services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();

            // IgnoreQueryFilters so a previously soft-deleted row is revived rather than
            // duplicated against the unique index.
            var existing = await db.Permissions.IgnoreQueryFilters().ToListAsync(cancellationToken);
            var byCode = existing.ToDictionary(x => x.Code, StringComparer.Ordinal);

            var now = DateTime.UtcNow;
            int added = 0, updated = 0, obsoleted = 0;

            foreach (var descriptor in AppPermissions.All)
            {
                if (byCode.TryGetValue(descriptor.Code, out var row))
                {
                    if (row.Service == descriptor.Service
                        && row.GroupName == descriptor.GroupName
                        && row.DisplayName == descriptor.DisplayName
                        && row.Description == descriptor.Description
                        && !row.IsObsolete
                        && !row.IsDeleted)
                    {
                        continue;
                    }

                    row.Service = descriptor.Service;
                    row.GroupName = descriptor.GroupName;
                    row.DisplayName = descriptor.DisplayName;
                    row.Description = descriptor.Description;
                    row.IsObsolete = false;
                    row.IsDeleted = false;
                    row.LastModifiedDate = now;
                    updated++;
                }
                else
                {
                    db.Permissions.Add(new Permission
                    {
                        Id = Guid.NewGuid(),
                        Code = descriptor.Code,
                        Service = descriptor.Service,
                        GroupName = descriptor.GroupName,
                        DisplayName = descriptor.DisplayName,
                        Description = descriptor.Description,
                        CreatedDate = now
                    });
                    added++;
                }
            }

            // Flagged, not deleted: the grants pointing at it stay valid as foreign keys, and
            // audit history still resolves the code. Nothing checks for it, so it grants nothing.
            foreach (var row in existing.Where(x => !AppPermissions.AllCodes.Contains(x.Code) && !x.IsObsolete))
            {
                row.IsObsolete = true;
                row.LastModifiedDate = now;
                obsoleted++;
            }

            if (added + updated + obsoleted == 0)
                return;

            await db.SaveChangesAsync(cancellationToken);
            logger.LogInformation(
                "Permission sync: {Added} added, {Updated} updated, {Obsoleted} obsoleted.",
                added, updated, obsoleted);
        }
    }
}

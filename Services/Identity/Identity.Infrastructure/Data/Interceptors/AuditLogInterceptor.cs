using Identity.Application.Interfaces;
using Identity.Core.Entities;
using Identity.Core.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Identity.Infrastructure.Data.Interceptors
{
    // Adds one AuditLog row per changed User/Role to the same SaveChanges, so audit and data commit together.
    // Stricter than Catalog's: this database holds credentials, so secrets are masked and RefreshToken is never audited.
    public class AuditLogInterceptor : SaveChangesInterceptor
    {
        public const string MaskedValue = "***";
        public const string MaskedChangedValue = "*** (changed)";

        // Allowlist: RefreshToken (live session tokens) must never end up in the audit table.
        // RolePermission is audited because who granted which permission is the most
        // security-relevant change in this database.
        private static readonly HashSet<Type> AuditedTypes = [typeof(User), typeof(Role), typeof(RolePermission)];

        // Recorded as "changed" without the value.
        private static readonly HashSet<string> MaskedProperties = [nameof(User.PasswordHash)];

        // Set by hand on every save, or bumped by every failed login (already in the activity log): pure noise here.
        private static readonly HashSet<string> IgnoredProperties =
        [
            nameof(BaseEntity.CreatedDate),
            nameof(BaseEntity.LastModifiedDate),
            nameof(User.FailedLoginCount),
            nameof(User.LockoutEnd)
        ];

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            Converters = { new JsonStringEnumConverter() }
        };

        private readonly ICurrentUserService _currentUserService;

        public AuditLogInterceptor(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
        }

        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            AddAuditLogs(eventData.Context);
            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            AddAuditLogs(eventData.Context);
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private void AddAuditLogs(DbContext? context)
        {
            if (context is null) return;

            var timestamp = DateTime.UtcNow;
            var auditLogs = new List<AuditLog>();

            // ToList: adding AuditLog rows below must not modify the sequence being enumerated.
            var entries = context.ChangeTracker.Entries()
                .Where(e => AuditedTypes.Contains(e.Metadata.ClrType) &&
                            e.State is EntityState.Added or EntityState.Modified or EntityState.Deleted)
                .ToList();

            foreach (var entry in entries)
            {
                var action = ResolveAction(entry);
                var changes = GetChanges(entry, action);

                // A failed login only touches ignored columns; skip rows with nothing left to show.
                if (action == AuditAction.Modified && changes.Count == 0) continue;

                auditLogs.Add(new AuditLog
                {
                    EntityName = entry.Metadata.ClrType.Name,
                    EntityId = GetPrimaryKey(entry),
                    Action = action,
                    Changes = JsonSerializer.Serialize(changes, JsonOptions),
                    UserName = _currentUserService.UserName,
                    CorrelationId = _currentUserService.CorrelationId,
                    Timestamp = timestamp
                });
            }

            if (auditLogs.Count > 0)
            {
                context.Set<AuditLog>().AddRange(auditLogs);
            }
        }

        private static AuditAction ResolveAction(EntityEntry entry)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    return AuditAction.Created;
                case EntityState.Deleted:
                    return AuditAction.Deleted;
            }

            // RolePermission is not a BaseEntity, so it has no IsDeleted to inspect.
            if (entry.Metadata.FindProperty(nameof(BaseEntity.IsDeleted)) is not null)
            {
                var isDeleted = entry.Property(nameof(BaseEntity.IsDeleted));
                if (isDeleted.OriginalValue is false && isDeleted.CurrentValue is true)
                {
                    return AuditAction.SoftDeleted;
                }
            }

            return AuditAction.Modified;
        }

        private static List<Change> GetChanges(EntityEntry entry, AuditAction action)
        {
            var properties = entry.Properties.Where(p => !IgnoredProperties.Contains(p.Metadata.Name));

            return action switch
            {
                AuditAction.Created => properties
                    .Where(p => p.CurrentValue is not null)
                    .Select(p => new Change(p.Metadata.Name, null, Mask(p, p.CurrentValue)))
                    .ToList(),

                // Full snapshot, so a removed row can still be identified from the log alone.
                AuditAction.Deleted => properties
                    .Select(p => new Change(p.Metadata.Name, Mask(p, p.OriginalValue), null))
                    .ToList(),

                AuditAction.SoftDeleted => properties
                    .Select(p => new Change(p.Metadata.Name, Mask(p, p.OriginalValue), Mask(p, p.CurrentValue)))
                    .ToList(),

                // Compare the real values, then mask. The new value gets its own marker so the change
                // still reads as a change ("*** → *** (changed)") instead of two equal strings.
                _ => properties
                    .Where(p => !Equals(p.OriginalValue, p.CurrentValue))
                    .Select(p => new Change(p.Metadata.Name, Mask(p, p.OriginalValue), Mask(p, p.CurrentValue, MaskedChangedValue)))
                    .ToList()
            };
        }

        private static object? Mask(PropertyEntry property, object? value, string mask = MaskedValue)
            => value is not null && MaskedProperties.Contains(property.Metadata.Name) ? mask : value;

        private static string GetPrimaryKey(EntityEntry entry)
        {
            var keyProperties = entry.Metadata.FindPrimaryKey()!.Properties;
            return string.Join(",", keyProperties.Select(p => entry.Property(p.Name).CurrentValue?.ToString()));
        }

        // Serialized into AuditLog.Changes; LogQueryService reads the same property names back.
        private sealed record Change(string Property, object? OldValue, object? NewValue);
    }
}

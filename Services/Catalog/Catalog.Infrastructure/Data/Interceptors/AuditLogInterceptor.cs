using Catalog.Application.Services;
using Catalog.Core.DTOs;
using Catalog.Core.Entities;
using Catalog.Core.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Catalog.Infrastructure.Data.Interceptors
{
    // Adds one AuditLog row per changed entity to the same SaveChanges, so audit and data commit together.
    public class AuditLogInterceptor : SaveChangesInterceptor
    {
        // Stamped on every save by AuditableEntityInterceptor; recording them would only add noise.
        private static readonly HashSet<string> IgnoredProperties =
        [
            nameof(BaseEntity.CreatedDate),
            nameof(BaseEntity.LastModifiedDate)
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
            if (context is not CatalogContext { IsAuditEnabled: true }) return;

            var timestamp = DateTime.UtcNow;
            var auditLogs = new List<AuditLog>();

            // ToList: adding AuditLog rows below must not modify the sequence being enumerated.
            var entries = context.ChangeTracker.Entries()
                .Where(e => e.Entity is not AuditLog and not ActivityLog &&
                            e.State is EntityState.Added or EntityState.Modified or EntityState.Deleted)
                .ToList();

            foreach (var entry in entries)
            {
                var action = ResolveAction(entry);
                var changes = GetChanges(entry, action);

                // BaseRepository.UpdateAsync marks every property Modified; skip rows whose values did not change.
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

            if (entry.Entity is BaseEntity)
            {
                var isDeleted = entry.Property(nameof(BaseEntity.IsDeleted));
                if (isDeleted.OriginalValue is false && isDeleted.CurrentValue is true)
                {
                    return AuditAction.SoftDeleted;
                }
            }

            return AuditAction.Modified;
        }

        private static List<AuditPropertyChange> GetChanges(EntityEntry entry, AuditAction action)
        {
            var properties = entry.Properties.Where(p => !IgnoredProperties.Contains(p.Metadata.Name));

            return action switch
            {
                AuditAction.Created => properties
                    .Where(p => p.CurrentValue is not null)
                    .Select(p => new AuditPropertyChange { Property = p.Metadata.Name, NewValue = p.CurrentValue })
                    .ToList(),

                // Full snapshot, so a removed row can still be identified from the log alone.
                AuditAction.Deleted => properties
                    .Select(p => new AuditPropertyChange { Property = p.Metadata.Name, OldValue = p.OriginalValue })
                    .ToList(),

                AuditAction.SoftDeleted => properties
                    .Select(p => new AuditPropertyChange { Property = p.Metadata.Name, OldValue = p.OriginalValue, NewValue = p.CurrentValue })
                    .ToList(),

                _ => properties
                    .Where(p => !Equals(p.OriginalValue, p.CurrentValue))
                    .Select(p => new AuditPropertyChange { Property = p.Metadata.Name, OldValue = p.OriginalValue, NewValue = p.CurrentValue })
                    .ToList()
            };
        }

        private static string GetPrimaryKey(EntityEntry entry)
        {
            var keyProperties = entry.Metadata.FindPrimaryKey()!.Properties;
            return string.Join(",", keyProperties.Select(p => entry.Property(p.Name).CurrentValue?.ToString()));
        }
    }
}

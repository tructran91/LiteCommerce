using Catalog.Core.Enums;

namespace Catalog.Core.Entities
{
    // Not a BaseEntity: activity rows are append-only and never soft-deleted.
    public class ActivityLog
    {
        public Guid Id { get; set; }

        public ActivityAction Action { get; set; }

        public string EntityName { get; set; }

        public string? EntityId { get; set; }

        public string? EntityDisplayName { get; set; }

        public string Description { get; set; }

        // MediatR request type that produced the activity, e.g. UpdateBrandCommand.
        public string RequestName { get; set; }

        public string UserName { get; set; }

        // Same value as AuditLog.CorrelationId for the rows changed by this request.
        public string CorrelationId { get; set; }

        public DateTime Timestamp { get; set; }
    }
}

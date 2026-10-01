using Catalog.Core.Enums;

namespace Catalog.Core.Entities
{
    // Not a BaseEntity: audit rows are append-only, never soft-deleted and never audited themselves.
    public class AuditLog
    {
        public Guid Id { get; set; }

        public string EntityName { get; set; }

        // Primary key value(s) of the audited row, comma-separated for composite keys.
        public string EntityId { get; set; }

        public AuditAction Action { get; set; }

        // JSON array of AuditPropertyChange.
        public string Changes { get; set; }

        public string UserName { get; set; }

        public string CorrelationId { get; set; }

        public DateTime Timestamp { get; set; }
    }
}

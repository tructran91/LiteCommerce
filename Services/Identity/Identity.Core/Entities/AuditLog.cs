using Identity.Core.Enums;

namespace Identity.Core.Entities
{
    // Not a BaseEntity: audit rows are append-only, never soft-deleted and never audited themselves.
    public class AuditLog
    {
        public Guid Id { get; set; }

        public string EntityName { get; set; } = string.Empty;

        // Primary key value(s) of the audited row, comma-separated for composite keys.
        public string EntityId { get; set; } = string.Empty;

        public AuditAction Action { get; set; }

        // JSON array of { Property, OldValue, NewValue }. Secret columns are masked before they get here.
        public string Changes { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;

        public string CorrelationId { get; set; } = string.Empty;

        public DateTime Timestamp { get; set; }
    }
}

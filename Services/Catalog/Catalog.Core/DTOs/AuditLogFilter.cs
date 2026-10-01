using Catalog.Core.Enums;

namespace Catalog.Core.DTOs
{
    public record AuditLogFilter
    {
        public string? EntityName { get; set; }

        public string? EntityId { get; set; }

        public AuditAction? Action { get; set; }

        public string? CorrelationId { get; set; }

        // Both bounds are inclusive and compared against the UTC timestamp.
        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }
    }
}

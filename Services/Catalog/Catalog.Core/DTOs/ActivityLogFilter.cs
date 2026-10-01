using Catalog.Core.Enums;

namespace Catalog.Core.DTOs
{
    public record ActivityLogFilter
    {
        public string? EntityName { get; set; }

        public string? EntityId { get; set; }

        public ActivityAction? Action { get; set; }

        public string? CorrelationId { get; set; }

        // Matches EntityDisplayName or Description.
        public string? Search { get; set; }

        // Both bounds are inclusive and compared against the UTC timestamp.
        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }
    }
}

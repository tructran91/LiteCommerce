using Identity.Core.Enums;
using LiteCommerce.Shared.Constants;

namespace Identity.Application.DTOs
{
    // Same shape as Catalog's log endpoints, so the Admin UI uses one client model for both services.
    public record GetAuditLogsQuery
    {
        public int PageSize { get; set; } = PaginationSetting.DefaultPageSize;

        public int CurrentPage { get; set; } = PaginationSetting.DefaultCurrentPage;

        public string? EntityName { get; set; }

        public string? EntityId { get; set; }

        public AuditAction? Action { get; set; }

        public string? CorrelationId { get; set; }

        // UTC, inclusive.
        public DateTime? FromDate { get; set; }

        // UTC, inclusive.
        public DateTime? ToDate { get; set; }
    }

    public record GetActivityLogsQuery
    {
        public int PageSize { get; set; } = PaginationSetting.DefaultPageSize;

        public int CurrentPage { get; set; } = PaginationSetting.DefaultCurrentPage;

        public ActivityAction? Action { get; set; }

        public bool? IsSuccess { get; set; }

        // Matches Email or Description.
        public string? Search { get; set; }

        public string? CorrelationId { get; set; }

        // UTC, inclusive.
        public DateTime? FromDate { get; set; }

        // UTC, inclusive.
        public DateTime? ToDate { get; set; }
    }

    public record AuditLogResponse
    {
        public Guid Id { get; set; }

        public string EntityName { get; set; } = string.Empty;

        public string EntityId { get; set; } = string.Empty;

        public string Action { get; set; } = string.Empty;

        public List<AuditPropertyChangeResponse> Changes { get; set; } = new();

        public string UserName { get; set; } = string.Empty;

        public string CorrelationId { get; set; } = string.Empty;

        public DateTime Timestamp { get; set; }
    }

    // Values are flattened to display strings; the stored JSON keeps the original types.
    public record AuditPropertyChangeResponse
    {
        public string Property { get; set; } = string.Empty;

        public string? OldValue { get; set; }

        public string? NewValue { get; set; }
    }

    public record ActivityLogResponse
    {
        public Guid Id { get; set; }

        public string Action { get; set; } = string.Empty;

        public bool IsSuccess { get; set; }

        public string? FailureReason { get; set; }

        public Guid? UserId { get; set; }

        public string? Email { get; set; }

        public string Description { get; set; } = string.Empty;

        public string? IpAddress { get; set; }

        public string? UserAgent { get; set; }

        public string UserName { get; set; } = string.Empty;

        public string CorrelationId { get; set; } = string.Empty;

        public DateTime Timestamp { get; set; }
    }
}

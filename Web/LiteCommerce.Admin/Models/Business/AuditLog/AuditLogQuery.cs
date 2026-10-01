namespace LiteCommerce.Admin.Models.Business.AuditLog
{
    // Filter state of the Audit Logs page. Dates are local calendar days; the page converts them to a UTC range.
    public class AuditLogQuery
    {
        public string? EntityName { get; set; }

        public string? Action { get; set; }

        public string? CorrelationId { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }
    }
}

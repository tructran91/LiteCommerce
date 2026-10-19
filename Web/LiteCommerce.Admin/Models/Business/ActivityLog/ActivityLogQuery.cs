namespace LiteCommerce.Admin.Models.Business.ActivityLog
{
    // Filter state of the Activity Logs page. Dates are local calendar days; the page converts them to a UTC range.
    public class ActivityLogQuery
    {
        public string? Search { get; set; }

        // Catalog only.
        public string? EntityName { get; set; }

        public string? Action { get; set; }

        // Identity only: null = all, true = succeeded, false = failed.
        public bool? IsSuccess { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }
    }
}

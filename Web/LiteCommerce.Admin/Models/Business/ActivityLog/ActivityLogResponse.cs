namespace LiteCommerce.Admin.Models.Business.ActivityLog
{
    public class ActivityLogResponse
    {
        public string Id { get; set; }

        // Create, Update, Delete, Upload or BulkUpdate.
        public string Action { get; set; }

        public string EntityName { get; set; }

        public string? EntityId { get; set; }

        public string? EntityDisplayName { get; set; }

        public string Description { get; set; }

        public string RequestName { get; set; }

        public string UserName { get; set; }

        public string CorrelationId { get; set; }

        // UTC.
        public DateTime Timestamp { get; set; }
    }
}

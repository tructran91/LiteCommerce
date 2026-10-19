namespace LiteCommerce.Admin.Models.Business.ActivityLog
{
    // One model for both services. Catalog fills the entity fields; Identity fills the security-event fields.
    public class ActivityLogResponse
    {
        public string Id { get; set; }

        // Catalog: Create, Update, Delete, Upload, BulkUpdate.
        // Identity: Register, Login, Logout, ChangePassword, AccountLocked, TokenReuseDetected.
        public string Action { get; set; }

        public string? EntityName { get; set; }

        public string? EntityId { get; set; }

        public string? EntityDisplayName { get; set; }

        public string Description { get; set; }

        public string? RequestName { get; set; }

        public string UserName { get; set; }

        public string CorrelationId { get; set; }

        // UTC.
        public DateTime Timestamp { get; set; }

        // Identity only. Catalog logs successful actions only, so these stay null there.
        public bool? IsSuccess { get; set; }

        public string? FailureReason { get; set; }

        public string? UserId { get; set; }

        // The email the event is about (UserName is "anonymous" for login/register).
        public string? Email { get; set; }

        public string? IpAddress { get; set; }

        public string? UserAgent { get; set; }
    }
}

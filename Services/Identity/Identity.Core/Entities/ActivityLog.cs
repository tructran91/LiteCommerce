using Identity.Core.Enums;

namespace Identity.Core.Entities
{
    // Security event log. Unlike Catalog's activity log it records failures too,
    // because failed logins and token reuse are the events worth investigating.
    public class ActivityLog
    {
        public Guid Id { get; set; }

        public ActivityAction Action { get; set; }

        public bool IsSuccess { get; set; }

        public ActivityFailureReason? FailureReason { get; set; }

        // Null when the email did not match a user.
        public Guid? UserId { get; set; }

        // The email the event is about (the one typed on login/register), normalised.
        public string? Email { get; set; }

        public string Description { get; set; } = string.Empty;

        public string? IpAddress { get; set; }

        public string? UserAgent { get; set; }

        // Who made the request: the signed-in email, "anonymous" for login/register, "system" outside a request.
        public string UserName { get; set; } = string.Empty;

        // Same value as AuditLog.CorrelationId for the rows changed by this request.
        public string CorrelationId { get; set; } = string.Empty;

        public DateTime Timestamp { get; set; }
    }
}

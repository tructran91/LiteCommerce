namespace LiteCommerce.Admin.Models.Business.AuditLog
{
    public class AuditLogResponse
    {
        public string Id { get; set; }

        public string EntityName { get; set; }

        public string EntityId { get; set; }

        // Created, Modified, SoftDeleted or Deleted.
        public string Action { get; set; }

        public List<AuditPropertyChangeResponse> Changes { get; set; } = new();

        public string UserName { get; set; }

        public string CorrelationId { get; set; }

        // UTC.
        public DateTime Timestamp { get; set; }
    }
}

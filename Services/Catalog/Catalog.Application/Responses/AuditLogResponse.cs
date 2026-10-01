namespace Catalog.Application.Responses
{
    public class AuditLogResponse
    {
        public Guid Id { get; set; }

        public string EntityName { get; set; }

        public string EntityId { get; set; }

        public string Action { get; set; }

        public List<AuditPropertyChangeResponse> Changes { get; set; } = new();

        public string UserName { get; set; }

        public string CorrelationId { get; set; }

        public DateTime Timestamp { get; set; }
    }
}

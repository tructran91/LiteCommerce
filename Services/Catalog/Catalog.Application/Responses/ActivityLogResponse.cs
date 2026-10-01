namespace Catalog.Application.Responses
{
    public class ActivityLogResponse
    {
        public Guid Id { get; set; }

        public string Action { get; set; }

        public string EntityName { get; set; }

        public string? EntityId { get; set; }

        public string? EntityDisplayName { get; set; }

        public string Description { get; set; }

        public string RequestName { get; set; }

        public string UserName { get; set; }

        public string CorrelationId { get; set; }

        public DateTime Timestamp { get; set; }
    }
}

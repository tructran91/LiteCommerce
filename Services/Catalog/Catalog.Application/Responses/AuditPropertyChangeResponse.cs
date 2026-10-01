namespace Catalog.Application.Responses
{
    // Values are flattened to display strings; the stored JSON keeps the original types.
    public class AuditPropertyChangeResponse
    {
        public string Property { get; set; }

        public string? OldValue { get; set; }

        public string? NewValue { get; set; }
    }
}

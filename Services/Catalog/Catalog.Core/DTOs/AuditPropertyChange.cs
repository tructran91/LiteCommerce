namespace Catalog.Core.DTOs
{
    // One element of AuditLog.Changes.
    public record AuditPropertyChange
    {
        public string Property { get; set; }

        public object? OldValue { get; set; }

        public object? NewValue { get; set; }
    }
}

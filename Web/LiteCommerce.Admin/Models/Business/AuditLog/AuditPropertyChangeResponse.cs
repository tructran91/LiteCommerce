namespace LiteCommerce.Admin.Models.Business.AuditLog
{
    public class AuditPropertyChangeResponse
    {
        public string Property { get; set; }

        public string? OldValue { get; set; }

        public string? NewValue { get; set; }

        public bool IsChanged => OldValue != NewValue;
    }
}

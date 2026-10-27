namespace Identity.Core.Entities
{
    // Rows are owned by AppPermissions, not by hand: PermissionSyncer upserts them on startup.
    public class Permission : BaseEntity
    {
        public string Code { get; set; } = string.Empty;

        public string Service { get; set; } = string.Empty;

        public string GroupName { get; set; } = string.Empty;

        public string DisplayName { get; set; } = string.Empty;

        public string? Description { get; set; }

        // Code no longer declared in AppPermissions. Kept so audit history still resolves,
        // but hidden from the role editor and dropped from issued tokens.
        public bool IsObsolete { get; set; }

        public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }
}

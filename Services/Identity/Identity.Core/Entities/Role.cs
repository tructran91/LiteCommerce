namespace Identity.Core.Entities
{
    public class Role : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        // Seeded role the app depends on by name. Cannot be renamed, deleted, or have its
        // permissions changed, so the system can never be left without an administrator.
        public bool IsSystem { get; set; }

        public ICollection<User> Users { get; set; } = new List<User>();

        public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }
}

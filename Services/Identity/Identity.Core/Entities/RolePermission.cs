namespace Identity.Core.Entities
{
    // Not a BaseEntity: a grant is either there or it is not, and updates replace the whole
    // set, so a soft-deleted grant would only be a way to get the filter wrong.
    public class RolePermission
    {
        public Guid RoleId { get; set; }

        public Role Role { get; set; } = null!;

        public Guid PermissionId { get; set; }

        public Permission Permission { get; set; } = null!;
    }
}

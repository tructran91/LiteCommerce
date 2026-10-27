namespace LiteCommerce.Admin.Models.Business.Role
{
    public record RoleDetailResponse
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public bool IsSystem { get; set; }

        public int UserCount { get; set; }

        public List<string> PermissionCodes { get; set; } = [];

        public DateTime CreatedDate { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }

    public record PermissionResponse
    {
        public string Code { get; set; } = string.Empty;

        public string DisplayName { get; set; } = string.Empty;

        public string? Description { get; set; }
    }

    public record PermissionGroupResponse
    {
        public string Service { get; set; } = string.Empty;

        public string GroupName { get; set; } = string.Empty;

        public List<PermissionResponse> Permissions { get; set; } = [];
    }

    public record CreateRoleRequest
    {
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public List<string> PermissionCodes { get; set; } = [];
    }

    public record UpdateRoleRequest
    {
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }
    }

    public record UpdateRolePermissionsRequest
    {
        public List<string> PermissionCodes { get; set; } = [];
    }

    public class RoleFormModel
    {
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }
    }
}

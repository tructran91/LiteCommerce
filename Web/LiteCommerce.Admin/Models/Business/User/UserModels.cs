using System.ComponentModel.DataAnnotations;

namespace LiteCommerce.Admin.Models.Business.User
{
    public record AdminUserResponse
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public Guid RoleId { get; set; }
        public string Role { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public bool IsLocked { get; set; }
        public DateTime? LockoutEnd { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }

    public record RoleResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }

    public class UserQuery
    {
        public string? Search { get; set; }
        public string? Role { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsLocked { get; set; }
    }

    public class CreateUserRequest
    {
        [Required, EmailAddress, MaxLength(256)]
        public string Email { get; set; } = string.Empty;

        [Required, MinLength(8), MaxLength(100)]
        public string Password { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public Guid RoleId { get; set; }

        public bool IsActive { get; set; } = true;
    }

    public class UpdateUserRequest
    {
        [Required, MaxLength(100)]
        public string FullName { get; set; } = string.Empty;
    }

    public record UpdateUserStatusRequest(bool IsActive);
    public record UpdateUserRoleRequest(Guid RoleId);
}

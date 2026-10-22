using LiteCommerce.Shared.Constants;

namespace Identity.Application.DTOs
{
    public record GetUsersQuery
    {
        public int PageSize { get; set; } = PaginationSetting.DefaultPageSize;

        public int CurrentPage { get; set; } = PaginationSetting.DefaultCurrentPage;

        public string? Search { get; set; }

        public string? Role { get; set; }

        public bool? IsActive { get; set; }

        public bool? IsLocked { get; set; }
    }

    public record CreateUserRequest
    {
        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public Guid RoleId { get; set; }

        public bool IsActive { get; set; } = true;
    }

    public record UpdateUserRequest
    {
        public string FullName { get; set; } = string.Empty;
    }

    public record UpdateUserStatusRequest
    {
        public bool IsActive { get; set; }
    }

    public record UpdateUserRoleRequest
    {
        public Guid RoleId { get; set; }
    }

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
}

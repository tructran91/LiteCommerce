using System.ComponentModel.DataAnnotations;

namespace LiteCommerce.Admin.Models.Business.Profile
{
    public record UserProfileResponse
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }

    public class UpdateProfileRequest
    {
        [Required, MaxLength(100)]
        public string FullName { get; set; } = string.Empty;
    }

    public record UserSessionResponse
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime Expires { get; set; }
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
    }
}

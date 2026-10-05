namespace Identity.Core.Entities
{
    public class User : BaseEntity
    {
        public string Email { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        // Brute-force protection: consecutive failed logins, and the time until which login is blocked.
        public int FailedLoginCount { get; set; }

        public DateTime? LockoutEnd { get; set; }

        public Guid RoleId { get; set; }

        public Role Role { get; set; } = null!;

        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}

namespace Identity.Core.Entities
{
    public class RefreshToken : BaseEntity
    {
        public Guid UserId { get; set; }

        public User User { get; set; } = null!;

        public string Token { get; set; } = string.Empty;

        public DateTime Expires { get; set; }

        public DateTime? Revoked { get; set; }

        public string? ReplacedByToken { get; set; }

        // Concurrency token: two parallel refreshes of the same token can't both succeed.
        public byte[] RowVersion { get; set; } = null!;

        public bool IsExpired => DateTime.UtcNow >= Expires;

        public bool IsActive => Revoked == null && !IsExpired;
    }
}

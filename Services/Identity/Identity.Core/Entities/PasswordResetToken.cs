namespace Identity.Core.Entities
{
    public class PasswordResetToken : BaseEntity
    {
        public Guid UserId { get; set; }

        public User User { get; set; } = null!;

        public string TokenHash { get; set; } = string.Empty;

        public DateTime Expires { get; set; }

        public DateTime? UsedDate { get; set; }

        public byte[] RowVersion { get; set; } = null!;
    }
}

using Identity.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.Data.Configurations
{
    public class PasswordResetTokenConfiguration : IEntityTypeConfiguration<PasswordResetToken>
    {
        public void Configure(EntityTypeBuilder<PasswordResetToken> builder)
        {
            builder.Property(x => x.TokenHash).IsRequired().HasMaxLength(64);
            builder.HasIndex(x => x.TokenHash).IsUnique();
            builder.Property(x => x.RowVersion).IsRowVersion();

            builder.HasOne(x => x.User)
                .WithMany(x => x.PasswordResetTokens)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

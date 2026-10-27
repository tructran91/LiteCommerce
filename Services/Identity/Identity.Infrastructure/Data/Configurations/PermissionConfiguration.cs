using Identity.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.Data.Configurations
{
    public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.Property(x => x.Code).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Service).IsRequired().HasMaxLength(50);
            builder.Property(x => x.GroupName).IsRequired().HasMaxLength(100);
            builder.Property(x => x.DisplayName).IsRequired().HasMaxLength(150);
            builder.Property(x => x.Description).HasMaxLength(500);

            builder.HasIndex(x => x.Code).IsUnique().HasFilter("[IsDeleted] = 0");

            // No HasData: PermissionSyncer owns these rows, so adding a permission needs
            // no migration.
        }
    }
}

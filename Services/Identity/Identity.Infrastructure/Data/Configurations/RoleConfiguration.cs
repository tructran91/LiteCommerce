using Identity.Core.Constants;
using Identity.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.Data.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public static readonly Guid AdminId = Guid.Parse("c2c20181-4df9-4881-85b7-32459baa8a93");
        public static readonly Guid StaffId = Guid.Parse("73a1904a-814c-4dcf-abe9-44d3a54f97ae");
        public static readonly Guid CustomerId = Guid.Parse("a561952f-bedf-49a6-8de6-074f0cf80163");

        public static readonly DateTime CreatedTime = new DateTime(2027, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.Property(x => x.Name).IsRequired().HasMaxLength(50);

            builder.HasIndex(x => x.Name).IsUnique().HasFilter("[IsDeleted] = 0");

            builder.HasData(
                new Role { Id = AdminId, Name = IdentityRoles.Admin, Description = "Full access", CreatedDate = CreatedTime },
                new Role { Id = StaffId, Name = IdentityRoles.Staff, Description = "Manage catalog", CreatedDate = CreatedTime },
                new Role { Id = CustomerId, Name = IdentityRoles.Customer, Description = "Shop customer", CreatedDate = CreatedTime });
        }
    }
}

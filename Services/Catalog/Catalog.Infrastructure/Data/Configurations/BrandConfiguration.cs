using Catalog.Core.Constants;
using Catalog.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Infrastructure.Data.Configurations
{
    public class BrandConfiguration : IEntityTypeConfiguration<Brand>
    {
        public void Configure(EntityTypeBuilder<Brand> builder)
        {
            builder.Property(x => x.Name).HasMaxLength(FieldLength.BrandName);
            builder.Property(x => x.Slug).HasMaxLength(FieldLength.BrandSlug);

            // Filtered so the name of a soft-deleted brand can be reused.
            builder.HasIndex(x => x.Name).IsUnique().HasFilter("[IsDeleted] = 0");
            builder.HasIndex(x => x.Slug).IsUnique().HasFilter("[IsDeleted] = 0");
        }
    }
}

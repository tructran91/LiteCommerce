using Catalog.Core.Constants;
using Catalog.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Infrastructure.Data.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(x => x.Slug).HasMaxLength(FieldLength.ProductSlug);

            builder.HasIndex(x => x.Slug).IsUnique().HasFilter("[IsDeleted] = 0");
        }
    }
}

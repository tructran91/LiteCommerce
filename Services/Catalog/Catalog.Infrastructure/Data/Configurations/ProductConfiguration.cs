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
            builder.Property(x => x.Sku).HasMaxLength(FieldLength.ProductSku);
            builder.Property(x => x.Gtin).HasMaxLength(FieldLength.ProductGtin);

            builder.Property(x => x.Price).HasPrecision(18, 2);
            builder.Property(x => x.PriceMax).HasPrecision(18, 2);
            builder.Property(x => x.OldPrice).HasPrecision(18, 2);
            builder.Property(x => x.SpecialPrice).HasPrecision(18, 2);

            builder.HasIndex(x => x.Slug).IsUnique().HasFilter("[IsDeleted] = 0");

            // Brand is optional: a product may have no brand.
            builder.HasOne(x => x.Brand)
                .WithMany()
                .HasForeignKey(x => x.BrandId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

using Catalog.Core.Constants;
using Catalog.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Infrastructure.Data.Configurations
{
    public class ProductVariantConfiguration : IEntityTypeConfiguration<ProductVariant>
    {
        public void Configure(EntityTypeBuilder<ProductVariant> builder)
        {
            builder.Property(x => x.NormalizedName)
                .IsRequired()
                .HasMaxLength(FieldLength.ProductVariantNormalizedName);

            builder.Property(x => x.Name)
                .HasMaxLength(FieldLength.ProductVariantName);

            builder.Property(x => x.Sku).HasMaxLength(FieldLength.ProductSku);
            builder.Property(x => x.Gtin).HasMaxLength(FieldLength.ProductGtin);

            builder.Property(x => x.Price).HasPrecision(18, 2);
            builder.Property(x => x.OldPrice).HasPrecision(18, 2);
            builder.Property(x => x.SpecialPrice).HasPrecision(18, 2);

            builder.HasIndex(x => new { x.ProductId, x.NormalizedName })
                .IsUnique()
                .HasFilter("[IsDeleted] = 0");

            // Excludes NULLs as well: a SKU is optional, so many variants may have none,
            // but two variants of one product may not share the same code.
            builder.HasIndex(x => new { x.ProductId, x.Sku })
                .IsUnique()
                .HasFilter("[IsDeleted] = 0 AND [Sku] IS NOT NULL");

            builder.HasOne(x => x.Product)
                .WithMany(p => p.Variants)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // Media rows outlive what points at them, as with Product.ThumbnailImage.
            builder.HasOne(x => x.ThumbnailImage)
                .WithMany()
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

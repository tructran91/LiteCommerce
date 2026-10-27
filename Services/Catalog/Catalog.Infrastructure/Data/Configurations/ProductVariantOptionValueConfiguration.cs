using Catalog.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Infrastructure.Data.Configurations
{
    public class ProductVariantOptionValueConfiguration : IEntityTypeConfiguration<ProductVariantOptionValue>
    {
        public void Configure(EntityTypeBuilder<ProductVariantOptionValue> builder)
        {
            builder.HasIndex(x => new { x.VariantId, x.OptionId })
                .IsUnique()
                .HasFilter("[IsDeleted] = 0");

            builder.HasOne(x => x.Variant)
                .WithMany(v => v.OptionValues)
                .HasForeignKey(x => x.VariantId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Option)
                .WithMany()
                .HasForeignKey(x => x.OptionId)
                .OnDelete(DeleteBehavior.Restrict);

            // Restrict, not Cascade: Product already cascades into both ProductOptionValue
            // and ProductVariant, and SQL Server rejects the second path into this table.
            builder.HasOne(x => x.ProductOptionValueItem)
                .WithMany(i => i.VariantOptionValues)
                .HasForeignKey(x => x.ProductOptionValueItemId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

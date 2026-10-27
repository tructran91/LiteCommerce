using Catalog.Core.Constants;
using Catalog.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Infrastructure.Data.Configurations
{
    public class ProductOptionValueConfiguration : IEntityTypeConfiguration<ProductOptionValue>
    {
        public void Configure(EntityTypeBuilder<ProductOptionValue> builder)
        {
            builder.Property(x => x.DisplayType)
                .HasMaxLength(FieldLength.ProductOptionDisplayType);

            builder.HasIndex(x => new { x.ProductId, x.OptionId })
                .IsUnique()
                .HasFilter("[IsDeleted] = 0");

            builder.HasOne(x => x.Product)
                .WithMany(p => p.OptionValues)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // Restrict keeps DeleteProductOptionHandler the single place that decides
            // whether an option still in use may go.
            builder.HasOne(x => x.Option)
                .WithMany()
                .HasForeignKey(x => x.OptionId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

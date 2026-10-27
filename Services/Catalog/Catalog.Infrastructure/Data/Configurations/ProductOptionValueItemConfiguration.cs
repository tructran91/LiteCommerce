using Catalog.Core.Constants;
using Catalog.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Infrastructure.Data.Configurations
{
    public class ProductOptionValueItemConfiguration : IEntityTypeConfiguration<ProductOptionValueItem>
    {
        public void Configure(EntityTypeBuilder<ProductOptionValueItem> builder)
        {
            builder.Property(x => x.Key)
                .IsRequired()
                .HasMaxLength(FieldLength.ProductOptionValueItemKey);

            builder.Property(x => x.Display)
                .IsRequired()
                .HasMaxLength(FieldLength.ProductOptionValueItemDisplay);

            builder.HasIndex(x => new { x.ProductOptionValueId, x.Key })
                .IsUnique()
                .HasFilter("[IsDeleted] = 0");

            builder.HasOne(x => x.ProductOptionValue)
                .WithMany(p => p.Items)
                .HasForeignKey(x => x.ProductOptionValueId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

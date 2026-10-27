using Catalog.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Infrastructure.Data.Configurations
{
    public class ProductPriceHistoryConfiguration : IEntityTypeConfiguration<ProductPriceHistory>
    {
        public void Configure(EntityTypeBuilder<ProductPriceHistory> builder)
        {
            // Match the precision used on Product and ProductVariant, otherwise the
            // snapshots are silently truncated to the SQL Server default scale.
            builder.Property(x => x.Price).HasPrecision(18, 2);
            builder.Property(x => x.OldPrice).HasPrecision(18, 2);
            builder.Property(x => x.SpecialPrice).HasPrecision(18, 2);
        }
    }
}

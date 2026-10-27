using Catalog.Core.Constants;
using Catalog.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Infrastructure.Data.Configurations
{
    public class ProductOptionConfiguration : IEntityTypeConfiguration<ProductOption>
    {
        public void Configure(EntityTypeBuilder<ProductOption> builder)
        {
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(FieldLength.ProductOptionName);

            builder.HasIndex(x => x.Name).IsUnique().HasFilter("[IsDeleted] = 0");
        }
    }
}

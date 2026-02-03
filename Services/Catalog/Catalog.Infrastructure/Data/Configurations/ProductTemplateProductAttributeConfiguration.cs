using Catalog.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Infrastructure.Data.Configurations
{
    public class ProductTemplateProductAttributeConfiguration : IEntityTypeConfiguration<ProductTemplateProductAttribute>
    {
        public void Configure(EntityTypeBuilder<ProductTemplateProductAttribute> builder)
        {
            builder.HasKey(pt => new { pt.ProductTemplateId, pt.ProductAttributeId });

            builder
                .HasOne(pt => pt.ProductTemplate)
                .WithMany(p => p.ProductAttributes)
                .HasForeignKey(pt => pt.ProductTemplateId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(pt => pt.ProductAttribute)
                .WithMany(t => t.ProductTemplates)
                .HasForeignKey(pt => pt.ProductAttributeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

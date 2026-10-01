using Catalog.Core.Constants;
using Catalog.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Infrastructure.Data.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.Property(x => x.Name).HasMaxLength(FieldLength.CategoryName);
            builder.Property(x => x.Slug).HasMaxLength(FieldLength.CategorySlug);

            // Unique per parent only: "Gaming" exists under both "Phones" and "Computers".
            builder.HasIndex(x => new { x.ParentId, x.Name }).IsUnique().HasFilter("[IsDeleted] = 0");
        }
    }
}

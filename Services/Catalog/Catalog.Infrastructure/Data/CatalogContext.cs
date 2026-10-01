using Catalog.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Catalog.Infrastructure.Data
{
    public class CatalogContext : DbContext
    {
        public CatalogContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Brand> Brands { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Media> Medias { get; set; }

        public DbSet<ProductAttribute> ProductAttributes { get; set; }

        public DbSet<ProductAttributeGroup> ProductAttributeGroups { get; set; }

        public DbSet<ProductAttributeValue> ProductAttributeValues { get; set; }

        public DbSet<ProductCategory> ProductCategories { get; set; }

        public DbSet<ProductLink> ProductLinks { get; set; }

        public DbSet<ProductMedia> ProductMedias { get; set; }

        public DbSet<ProductOption> ProductOptions { get; set; }

        public DbSet<ProductOptionValue> ProductOptionValues { get; set; }

        public DbSet<ProductPriceHistory> ProductPriceHistories { get; set; }

        public DbSet<ProductTemplate> ProductTemplates { get; set; }

        public DbSet<ProductTemplateProductAttribute> ProductTemplateProductAttributes { get; set; }

        public DbSet<AuditLog> AuditLogs { get; set; }

        public DbSet<ActivityLog> ActivityLogs { get; set; }

        // Read by AuditLogInterceptor; the seed turns it off. Dates are stamped by AuditableEntityInterceptor.
        public bool IsAuditEnabled { get; set; } = true;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CatalogContext).Assembly);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                {
                    var parameter = Expression.Parameter(entityType.ClrType, "e");
                    var property = Expression.Property(parameter, nameof(BaseEntity.IsDeleted));
                    var condition = Expression.Equal(property, Expression.Constant(false));
                    var lambda = Expression.Lambda(condition, parameter);
                    entityType.SetQueryFilter(lambda);
                }
            }

            base.OnModelCreating(modelBuilder);
        }
    }
}

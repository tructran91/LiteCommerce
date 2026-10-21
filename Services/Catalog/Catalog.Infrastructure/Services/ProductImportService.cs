using Catalog.Application.Extensions;
using Catalog.Application.Products.Import;
using Catalog.Application.Products.Import.Models;
using Catalog.Core.Entities;
using Catalog.Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Services
{
    public class ProductImportService : IProductImportService
    {
        private readonly CatalogContext _context;

        public ProductImportService(CatalogContext context)
        {
            _context = context;
        }

        public async Task<ProductImportCommitResult> ImportAsync(IReadOnlyList<ProductImportItem> items, CancellationToken cancellationToken = default)
        {
            var createdBrands = new List<string>();
            var imported = 0;

            // A user transaction must run inside the execution strategy, or a retrying strategy
            // (EnableRetryOnFailure) throws. Each attempt starts from a clean change tracker.
            var strategy = _context.Database.CreateExecutionStrategy();

            // Two concurrent imports can both miss the same brand; the second insert then fails on
            // the unique index. Re-run once: the retry sees the brand committed by the other request.
            const int maxAttempts = 2;
            for (var attempt = 1; ; attempt++)
            {
                try
                {
                    await strategy.ExecuteAsync(async () =>
                    {
                        _context.ChangeTracker.Clear();
                        createdBrands.Clear();
                        imported = 0;

                        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

                        // Dedupe by name or slug (same rule as the resolver), then reuse an existing brand
                        // if one appeared since the preview (race).
                        var brandsByName = new Dictionary<string, Brand>(StringComparer.OrdinalIgnoreCase);
                        var brandsBySlug = new Dictionary<string, Brand>(StringComparer.OrdinalIgnoreCase);
                        var existingBrands = await _context.Brands.ToListAsync(cancellationToken);
                        foreach (var brand in existingBrands)
                        {
                            brandsByName.TryAdd(brand.Name.Trim(), brand);
                            brandsBySlug.TryAdd(brand.Slug, brand);
                        }

                        Brand? FindBrand(string name)
                            => brandsByName.GetValueOrDefault(name) ?? brandsBySlug.GetValueOrDefault(name.Slugify());

                        var newBrands = new List<Brand>();
                        foreach (var item in items)
                        {
                            if (!NeedsBrand(item))
                            {
                                continue;
                            }

                            var name = item.BrandName.Trim();
                            if (FindBrand(name) is not null)
                            {
                                continue;
                            }

                            var brand = new Brand
                            {
                                Id = Guid.NewGuid(),
                                Name = name,
                                Slug = CreateBrandSlug(name, brandsBySlug.Keys),
                                IsPublished = true
                            };

                            brandsByName[name] = brand;
                            brandsBySlug[brand.Slug] = brand;
                            newBrands.Add(brand);
                        }

                        if (newBrands.Count > 0)
                        {
                            _context.Brands.AddRange(newBrands);
                            createdBrands.AddRange(newBrands.Select(b => b.Name));
                        }

                        foreach (var item in items)
                        {
                            var product = item.Product;

                            if (NeedsBrand(item) && FindBrand(item.BrandName.Trim()) is { } brand)
                            {
                                product.Brand = brand;
                                product.BrandId = brand.Id;
                            }
                            _context.Products.Add(product);
                        }

                        await _context.SaveChangesAsync(cancellationToken);
                        await transaction.CommitAsync(cancellationToken);
                        imported = items.Count;
                    });

                    break;
                }
                catch (DbUpdateException e) when (attempt < maxAttempts && IsUniqueViolation(e))
                {
                    // A unique index rejected the batch (most likely a brand created concurrently).
                    // Clearing happens at the start of the next attempt.
                }
            }

            return new ProductImportCommitResult(imported, createdBrands);
        }

        // A blank Brand cell means "no brand": nothing to resolve or create.
        private static bool NeedsBrand(ProductImportItem item)
            => !string.IsNullOrWhiteSpace(item.BrandName)
                && (item.CreateBrandIfMissing || !item.BrandId.HasValue);

        private static bool IsUniqueViolation(DbUpdateException exception)
            => exception.InnerException is SqlException { Number: 2601 or 2627 };

        private static string CreateBrandSlug(string name, IEnumerable<string> usedSlugs)
        {
            var baseSlug = string.IsNullOrWhiteSpace(name) ? "brand" : name.Slugify();
            if (string.IsNullOrEmpty(baseSlug))
            {
                baseSlug = "brand";
            }

            var used = new HashSet<string>(usedSlugs, StringComparer.OrdinalIgnoreCase);
            var slug = baseSlug;
            var suffix = 2;
            while (used.Contains(slug))
            {
                slug = $"{baseSlug}-{suffix++}";
            }

            return slug;
        }
    }
}

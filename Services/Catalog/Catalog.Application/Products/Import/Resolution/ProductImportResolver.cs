using Catalog.Application.Extensions;
using Catalog.Application.Products.Import.Models;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;

namespace Catalog.Application.Products.Import.Resolution
{
    // Turns raw rows into rows that reference real brand/category ids.
    // Reads only: preview and commit both call this, so commit re-validates from scratch.
    public class ProductImportResolver
    {
        private readonly IBaseRepository<Brand> _brandRepository;
        private readonly IBaseRepository<Category> _categoryRepository;
        private readonly IProductRepository _productRepository;

        public ProductImportResolver(
            IBaseRepository<Brand> brandRepository,
            IBaseRepository<Category> categoryRepository,
            IProductRepository productRepository)
        {
            _brandRepository = brandRepository;
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
        }

        public async Task<ProductImportAnalysis> ResolveAsync(ParsedImportFile file, ImportOptions options, CancellationToken cancellationToken = default)
        {
            var brands = await _brandRepository.GetAllAsync(disableTracking: true);
            var categories = await _categoryRepository.GetAllAsync(disableTracking: true);

            var brandByName = new Dictionary<string, Brand>(StringComparer.OrdinalIgnoreCase);
            var brandBySlug = new Dictionary<string, Brand>(StringComparer.OrdinalIgnoreCase);
            foreach (var brand in brands)
            {
                brandByName.TryAdd(brand.Name.Trim(), brand);
                brandBySlug.TryAdd(brand.Slug, brand);
            }

            var categoryLookup = BuildCategoryLookup(categories);

            var resolvedRows = new List<ResolvedProductRow>(file.Rows.Count);

            // Slugs and skus are checked per file, then against the database in one round trip.
            var slugCounts = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            var skuCounts = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            foreach (var row in file.Rows)
            {
                var slug = string.IsNullOrWhiteSpace(row.Slug) ? row.Name.Slugify() : row.Slug.Trim().Slugify();
                slugCounts[slug] = slugCounts.GetValueOrDefault(slug) + 1;

                if (!string.IsNullOrWhiteSpace(row.Sku))
                {
                    var sku = row.Sku.Trim();
                    skuCounts[sku] = skuCounts.GetValueOrDefault(sku) + 1;
                }
            }

            var existingSlugs = await _productRepository.GetExistingSlugsAsync(slugCounts.Keys.ToList(), cancellationToken);
            var existingSkus = await _productRepository.GetExistingSkusAsync(skuCounts.Keys.ToList(), cancellationToken);
            foreach (var row in file.Rows)
            {
                var slugWasGenerated = string.IsNullOrWhiteSpace(row.Slug);
                var resolved = new ResolvedProductRow
                {
                    Source = row,
                    Slug = slugWasGenerated ? row.Name.Slugify() : row.Slug!.Trim().Slugify()
                };

                resolved.Errors.AddRange(row.Errors);

                if (string.IsNullOrWhiteSpace(resolved.Slug))
                {
                    resolved.Errors.Add(new ProductImportError(ProductImportTemplate.Slug,
                        "Slug could not be generated. Provide Name or Slug."));
                }
                else if (slugWasGenerated && !string.IsNullOrWhiteSpace(row.Name))
                {
                    resolved.Warnings.Add($"Slug was generated automatically: '{resolved.Slug}'.");
                }

                ResolveBrand(row, options, brandByName, brandBySlug, resolved);
                ResolveCategories(row, categoryLookup, resolved);
                ValidateUniqueness(row, resolved, slugCounts, skuCounts, existingSlugs, existingSkus);

                resolvedRows.Add(resolved);
            }

            // Only brands referenced by a row that can actually be imported will be created.
            // Names that slugify alike ("Apple Inc", "apple inc.") become one brand, as in ProductImportService.
            var newBrands = resolvedRows
                .Where(r => r.IsValid && r.BrandStatus == BrandResolutionStatus.New)
                .Select(r => r.BrandName.Trim())
                .DistinctBy(name => name.Slugify(), StringComparer.OrdinalIgnoreCase)
                .ToList();

            return new ProductImportAnalysis(file, resolvedRows, newBrands);
        }

        private static void ResolveBrand(
            ParsedProductRow row,
            ImportOptions options,
            Dictionary<string, Brand> brandByName,
            Dictionary<string, Brand> brandBySlug,
            ResolvedProductRow resolved)
        {
            // Brand is optional: an empty cell is not an error, the product just has no brand.
            if (string.IsNullOrWhiteSpace(row.Brand))
            {
                resolved.BrandStatus = BrandResolutionStatus.None;
                return;
            }

            // Compare slugs too: "Apple Inc" must match the existing "Apple, Inc." (slug apple-inc)
            // instead of creating a near-duplicate brand.
            var name = row.Brand.Trim();
            if (brandByName.TryGetValue(name, out var existing) || brandBySlug.TryGetValue(name.Slugify(), out existing))
            {
                resolved.BrandStatus = BrandResolutionStatus.Existing;
                resolved.BrandId = existing.Id;
                return;
            }

            if (!options.CreateMissingBrands)
            {
                resolved.BrandStatus = BrandResolutionStatus.NotFound;
                resolved.Errors.Add(new ProductImportError(ProductImportTemplate.Brand,
                    $"Brand '{name}' does not exist. Turn on 'Create missing brands' to create it."));
                return;
            }

            resolved.BrandStatus = BrandResolutionStatus.New;
        }

        private static void ResolveCategories(ParsedProductRow row, Dictionary<string, Category> lookup, ResolvedProductRow resolved)
        {
            if (string.IsNullOrWhiteSpace(row.Category))
            {
                return;
            }

            var paths = row.Category.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            foreach (var path in paths)
            {
                var segments = path.Split('/', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                if (segments.Length == 0)
                {
                    continue;
                }

                Guid? parentId = null;
                Category? current = null;
                var resolvedPath = new List<string>();

                foreach (var segment in segments)
                {
                    if (!lookup.TryGetValue(CategoryKey(parentId, segment), out current))
                    {
                        resolved.Errors.Add(new ProductImportError(ProductImportTemplate.Category,
                            $"Category '{path}' does not exist."));
                        current = null;
                        break;
                    }

                    parentId = current.Id;
                    resolvedPath.Add(current.Name);
                }

                if (current is null)
                {
                    continue;
                }

                if (resolved.Categories.All(c => c.Id != current.Id))
                {
                    resolved.Categories.Add(new ResolvedCategory(current.Id, string.Join(" / ", resolvedPath)));
                }
            }
        }

        private static void ValidateUniqueness(
            ParsedProductRow row,
            ResolvedProductRow resolved,
            Dictionary<string, int> slugCounts,
            Dictionary<string, int> skuCounts,
            HashSet<string> existingSlugs,
            HashSet<string> existingSkus)
        {
            if (slugCounts.GetValueOrDefault(resolved.Slug) > 1)
            {
                resolved.Errors.Add(new ProductImportError(ProductImportTemplate.Slug,
                    $"Slug '{resolved.Slug}' is duplicated in the file."));
            }
            else if (existingSlugs.Contains(resolved.Slug))
            {
                resolved.Errors.Add(new ProductImportError(ProductImportTemplate.Slug,
                    $"A product with slug '{resolved.Slug}' already exists."));
            }

            if (!string.IsNullOrWhiteSpace(row.Sku))
            {
                var sku = row.Sku.Trim();
                if (skuCounts.GetValueOrDefault(sku) > 1)
                {
                    resolved.Errors.Add(new ProductImportError(ProductImportTemplate.Sku,
                        $"Sku '{sku}' is duplicated in the file."));
                }
                else if (existingSkus.Contains(sku))
                {
                    resolved.Errors.Add(new ProductImportError(ProductImportTemplate.Sku,
                        $"A product with sku '{sku}' already exists."));
                }
            }
        }

        private static Dictionary<string, Category> BuildCategoryLookup(IEnumerable<Category> categories)
        {
            var lookup = new Dictionary<string, Category>(StringComparer.OrdinalIgnoreCase);

            foreach (var category in categories)
            {
                lookup.TryAdd(CategoryKey(category.ParentId, category.Name), category);
            }

            return lookup;
        }

        private static string CategoryKey(Guid? parentId, string name)
            => $"{parentId?.ToString() ?? "root"}|{name.Trim()}";
    }
}

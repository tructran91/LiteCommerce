using Catalog.Application.Products.Import.Models;
using Catalog.Application.Services;
using Catalog.Core.Entities;

namespace Catalog.Application.Products.Import
{
    // Builds Product entities from resolved rows. Used by the commit handler.
    public class ProductImportFactory
    {
        private readonly IProductService _productService;

        public ProductImportFactory(IProductService productService)
        {
            _productService = productService;
        }

        public ProductImportItem CreateItem(ResolvedProductRow row)
        {
            var source = row.Source;

            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = source.Name,
                Slug = row.Slug,
                ShortDescription = source.ShortDescription,
                Description = source.Description,
                IsPublished = source.IsPublished,
                Price = source.Price ?? 0,
                // The importer never creates variants, so the product's own price is also
                // the top of its range. Keeps range rendering consistent for every product.
                PriceMax = source.Price ?? 0,
                OldPrice = source.OldPrice,
                SpecialPrice = source.SpecialPrice,
                SpecialPriceStart = source.SpecialPriceStart,
                SpecialPriceEnd = source.SpecialPriceEnd,
                IsFeatured = source.IsFeatured,
                IsCallForPricing = source.IsCallForPricing,
                IsAllowToOrder = source.IsAllowToOrder,
                StockQuantity = source.StockQuantity,
                Sku = source.Sku,
                Gtin = source.Gtin,
                DisplayOrder = source.DisplayOrder,
                MetaTitle = source.MetaTitle,
                MetaKeywords = source.MetaKeywords,
                MetaDescription = source.MetaDescription,
                CanonicalUrl = source.CanonicalUrl,
                OgTitle = source.OgTitle,
                OgDescription = source.OgDescription,
                OgImage = source.OgImage,
                OgUrl = source.OgUrl,
                SchemaJson = source.SchemaJson
            };

            if (row.BrandId.HasValue)
            {
                product.BrandId = row.BrandId.Value;
            }

            foreach (var category in row.Categories)
            {
                product.AddCategory(new ProductCategory { CategoryId = category.Id });
            }

            product.PriceHistories.Add(_productService.CreatePriceHistory(product));

            return new ProductImportItem
            {
                Product = product,
                BrandName = row.BrandName,
                BrandId = row.BrandId,
                CreateBrandIfMissing = row.BrandStatus == BrandResolutionStatus.New,
                RowNumber = row.RowNumber
            };
        }
    }
}

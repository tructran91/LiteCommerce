using Catalog.Application.Products.Import.Models;
using Catalog.Application.Responses;

namespace Catalog.Application.Products.Import
{
    public static class ProductImportResponseMapper
    {
        public static ProductImportPreviewResponse ToPreviewResponse(ProductImportAnalysis analysis, ProductImportMode mode)
        {
            var rows = analysis.Rows.Select(ToRowResult).ToList();
            var validRows = rows.Count(r => r.Status == ProductImportRowStatus.Valid);
            var invalidRows = rows.Count - validRows;

            var canCommit = analysis.File.IsValid &&
                (mode == ProductImportMode.AllOrNothing ? invalidRows == 0 && validRows > 0 : validRows > 0);

            return new ProductImportPreviewResponse
            {
                TotalRows = rows.Count,
                ValidRows = validRows,
                InvalidRows = invalidRows,
                Rows = rows,
                NewBrands = analysis.NewBrands.ToList(),
                FileErrors = analysis.File.Errors.Select(ToErrorResponse).ToList(),
                CanCommit = canCommit
            };
        }

        public static ProductImportRowResult ToRowResult(ResolvedProductRow row)
        {
            return new ProductImportRowResult
            {
                RowNumber = row.RowNumber,
                Status = row.IsValid ? ProductImportRowStatus.Valid : ProductImportRowStatus.Invalid,
                Name = row.Name,
                Brand = row.BrandName,
                BrandStatus = row.BrandStatus.ToString(),
                BrandId = row.BrandId,
                Category = row.Source.Category,
                Categories = row.Categories
                    .Select(c => new ProductImportCategoryResult { Id = c.Id, Path = c.Path })
                    .ToList(),
                Price = row.Source.Price,
                Errors = row.Errors.Select(ToErrorResponse).ToList(),
                Warnings = row.Warnings.ToList()
            };
        }

        public static ProductImportErrorResponse ToErrorResponse(ProductImportError error)
            => new() { Column = error.Column, Message = error.Message };

        public static ProductImportRowFailure ToRowFailure(ResolvedProductRow row)
        {
            return new ProductImportRowFailure
            {
                RowNumber = row.RowNumber,
                Name = row.Name,
                Errors = row.Errors.Select(ToErrorResponse).ToList()
            };
        }
    }
}

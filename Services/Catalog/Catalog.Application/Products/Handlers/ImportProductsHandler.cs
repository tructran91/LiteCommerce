using Catalog.Application.Products.Commands;
using Catalog.Application.Products.Import;
using Catalog.Application.Products.Import.Models;
using Catalog.Application.Products.Import.Parsing;
using Catalog.Application.Products.Import.Resolution;
using Catalog.Application.Responses;
using LiteCommerce.Shared.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace Catalog.Application.Products.Handlers
{
    public class ImportProductsHandler : IRequestHandler<ImportProductsCommand, BaseResponse<ProductImportResultResponse>>
    {
        private readonly ExcelProductImportParser _parser;
        private readonly ProductImportResolver _resolver;
        private readonly ProductImportFactory _factory;
        private readonly IProductImportService _importService;
        private readonly ILogger<ImportProductsHandler> _logger;

        public ImportProductsHandler(
            ExcelProductImportParser parser,
            ProductImportResolver resolver,
            ProductImportFactory factory,
            IProductImportService importService,
            ILogger<ImportProductsHandler> logger)
        {
            _parser = parser;
            _resolver = resolver;
            _factory = factory;
            _importService = importService;
            _logger = logger;
        }

        public async Task<BaseResponse<ProductImportResultResponse>> Handle(ImportProductsCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("ImportProductsHandler: {Request}", JsonSerializer.Serialize(request));

            var mode = ProductImportModes.Parse(request.Payload.Mode);

            ParsedImportFile file;
            await using (var stream = request.Payload.File!.OpenReadStream())
            {
                file = _parser.Parse(stream);
            }

            if (file.Errors.Count > 0)
            {
                var errors = file.Errors
                    .GroupBy(e => e.Column)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.Message).Distinct().ToList());

                return BaseResponse<ProductImportResultResponse>.Failure(
                    "The file could not be imported.",
                    errors,
                    statusCode: HttpStatusCode.BadRequest);
            }

            var options = new ImportOptions
            {
                CreateMissingBrands = request.Payload.CreateMissingBrands,
                Mode = mode
            };

            // Commit never trusts the preview: resolve and validate the whole file again.
            var analysis = await _resolver.ResolveAsync(file, options, cancellationToken);

            var invalidRows = analysis.Rows.Where(r => !r.IsValid).ToList();
            if (mode == ProductImportMode.AllOrNothing && invalidRows.Count > 0)
            {
                var response = BuildAllOrNothingFailure(analysis, invalidRows);
                return BaseResponse<ProductImportResultResponse>.Failure(
                    "The file contains invalid rows. Nothing was imported.",
                    statusCode: HttpStatusCode.BadRequest,
                    data: response);
            }

            var rowsToImport = analysis.Rows.Where(r => r.IsValid).ToList();
            if (rowsToImport.Count == 0)
            {
                var response = BuildAllOrNothingFailure(analysis, invalidRows);
                return BaseResponse<ProductImportResultResponse>.Failure(
                    "The file does not contain any valid row. Nothing was imported.",
                    statusCode: HttpStatusCode.BadRequest,
                    data: response);
            }

            var items = rowsToImport.Select(_factory.CreateItem).ToList();
            var commitResult = await _importService.ImportAsync(items, cancellationToken);

            request.EntityDisplayName = $"{commitResult.ImportedCount} product(s) from {request.Payload.File!.FileName}";

            var result = new ProductImportResultResponse
            {
                TotalRows = analysis.Rows.Count,
                Imported = commitResult.ImportedCount,
                Skipped = analysis.Rows.Count - commitResult.ImportedCount,
                CreatedBrands = commitResult.CreatedBrands.ToList(),
                Errors = invalidRows.Select(ProductImportResponseMapper.ToRowFailure).ToList()
            };

            return BaseResponse<ProductImportResultResponse>.Success(result,
                $"Imported {result.Imported} product(s), skipped {result.Skipped} row(s).");
        }

        private static ProductImportResultResponse BuildAllOrNothingFailure(
            ProductImportAnalysis analysis,
            IReadOnlyList<ResolvedProductRow> invalidRows)
        {
            return new ProductImportResultResponse
            {
                TotalRows = analysis.Rows.Count,
                Imported = 0,
                Skipped = analysis.Rows.Count,
                Errors = invalidRows.Select(ProductImportResponseMapper.ToRowFailure).ToList()
            };
        }
    }
}

using Catalog.Application.Extensions;
using Catalog.Application.Products.Import;
using Catalog.Application.Products.Import.Models;
using Catalog.Application.Products.Import.Parsing;
using Catalog.Application.Products.Import.Reporting;
using Catalog.Application.Products.Import.Resolution;
using Catalog.Application.Products.Queries;
using Catalog.Application.Responses;
using LiteCommerce.Shared.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace Catalog.Application.Products.Handlers
{
    public class BuildProductImportReportHandler : IRequestHandler<BuildProductImportReportQuery, BaseResponse<ProductImportErrorReportResponse>>
    {
        private readonly ExcelProductImportParser _parser;
        private readonly ProductImportResolver _resolver;
        private readonly ProductImportErrorReportBuilder _reportBuilder;
        private readonly ILogger<BuildProductImportReportHandler> _logger;

        public BuildProductImportReportHandler(
            ExcelProductImportParser parser,
            ProductImportResolver resolver,
            ProductImportErrorReportBuilder reportBuilder,
            ILogger<BuildProductImportReportHandler> logger)
        {
            _parser = parser;
            _resolver = resolver;
            _reportBuilder = reportBuilder;
            _logger = logger;
        }

        public async Task<BaseResponse<ProductImportErrorReportResponse>> Handle(BuildProductImportReportQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("BuildProductImportReportHandler: {Request}", JsonSerializer.Serialize(request));

            var originalBytes = await request.Payload.File!.ReadAllBytesAsync(cancellationToken);

            ParsedImportFile file;
            using (var stream = new MemoryStream(originalBytes))
            {
                file = _parser.Parse(stream);
            }

            if (file.Errors.Count > 0)
            {
                return BaseResponse<ProductImportErrorReportResponse>.Failure(
                    "The file could not be parsed, so no error report can be generated.",
                    statusCode: HttpStatusCode.BadRequest);
            }

            var mode = ProductImportModes.Parse(request.Payload.Mode);

            var options = new ImportOptions
            {
                CreateMissingBrands = request.Payload.CreateMissingBrands,
                Mode = mode
            };

            var analysis = await _resolver.ResolveAsync(file, options, cancellationToken);
            var rowsByNumber = analysis.Rows.ToDictionary(r => r.RowNumber);

            byte[] report;
            using (var stream = new MemoryStream(originalBytes))
            {
                report = _reportBuilder.Build(stream, file, rowsByNumber);
            }

            var originalName = Path.GetFileNameWithoutExtension(request.Payload.File!.FileName);
            var response = new ProductImportErrorReportResponse(report, $"{originalName}-review.xlsx", ProductImportTemplate.ExcelContentType);

            return BaseResponse<ProductImportErrorReportResponse>.Success(response);
        }
    }
}

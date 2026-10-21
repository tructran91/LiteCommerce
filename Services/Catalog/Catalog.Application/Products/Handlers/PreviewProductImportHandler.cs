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
    public class PreviewProductImportHandler : IRequestHandler<PreviewProductImportCommand, BaseResponse<ProductImportPreviewResponse>>
    {
        private readonly ExcelProductImportParser _parser;
        private readonly ProductImportResolver _resolver;
        private readonly ILogger<PreviewProductImportHandler> _logger;

        public PreviewProductImportHandler(
            ExcelProductImportParser parser,
            ProductImportResolver resolver,
            ILogger<PreviewProductImportHandler> logger)
        {
            _parser = parser;
            _resolver = resolver;
            _logger = logger;
        }

        public async Task<BaseResponse<ProductImportPreviewResponse>> Handle(PreviewProductImportCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("PreviewProductImportHandler: {Request}", JsonSerializer.Serialize(request));

            var mode = ProductImportModes.Parse(request.Payload.Mode);

            ParsedImportFile file;
            await using (var stream = request.Payload.File!.OpenReadStream())
            {
                file = _parser.Parse(stream);
            }

            if (file.Errors.Count > 0)
            {
                var fileErrorResponse = new ProductImportPreviewResponse
                {
                    FileErrors = file.Errors.Select(ProductImportResponseMapper.ToErrorResponse).ToList(),
                    CanCommit = false
                };

                return BaseResponse<ProductImportPreviewResponse>.Failure(
                    "The file could not be imported.",
                    statusCode: HttpStatusCode.BadRequest,
                    data: fileErrorResponse);
            }
            var options = new ImportOptions
            {
                CreateMissingBrands = request.Payload.CreateMissingBrands,
                Mode = mode
            };

            var analysis = await _resolver.ResolveAsync(file, options, cancellationToken);
            var response = ProductImportResponseMapper.ToPreviewResponse(analysis, mode);

            return BaseResponse<ProductImportPreviewResponse>.Success(response);
        }
    }
}

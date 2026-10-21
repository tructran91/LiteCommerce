using Catalog.Application.Requests;
using Catalog.Application.Responses;
using LiteCommerce.Shared.Models;
using MediatR;

namespace Catalog.Application.Products.Commands
{
    // Read-only: parses and resolves the file, never writes to the database.
    public class PreviewProductImportCommand : IRequest<BaseResponse<ProductImportPreviewResponse>>
    {
        public PreviewProductImportRequest Payload { get; set; }

        public PreviewProductImportCommand(PreviewProductImportRequest payload)
        {
            Payload = payload;
        }
    }
}

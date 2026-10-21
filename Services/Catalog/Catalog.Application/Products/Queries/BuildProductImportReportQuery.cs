using Catalog.Application.Requests;
using Catalog.Application.Responses;
using LiteCommerce.Shared.Models;
using MediatR;

namespace Catalog.Application.Products.Queries
{
    public class BuildProductImportReportQuery : IRequest<BaseResponse<ProductImportErrorReportResponse>>
    {
        public PreviewProductImportRequest Payload { get; set; }

        public BuildProductImportReportQuery(PreviewProductImportRequest payload)
        {
            Payload = payload;
        }
    }

    public record ProductImportErrorReportResponse(byte[] Content, string FileName, string ContentType);
}

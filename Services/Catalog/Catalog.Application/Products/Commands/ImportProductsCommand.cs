using Catalog.Application.Behaviors;
using Catalog.Application.Requests;
using Catalog.Application.Responses;
using Catalog.Core.Enums;
using LiteCommerce.Shared.Models;
using MediatR;

namespace Catalog.Application.Products.Commands
{
    public class ImportProductsCommand : IRequest<BaseResponse<ProductImportResultResponse>>, IActivityLoggable
    {
        public ImportProductsRequest Payload { get; set; }

        // Set by the handler once the import count is known, for the activity log description.
        public string? EntityDisplayName { get; set; }

        public ImportProductsCommand(ImportProductsRequest payload)
        {
            Payload = payload;
        }

        ActivityAction IActivityLoggable.ActivityAction => ActivityAction.Import;

        string IActivityLoggable.ActivityEntityName => "Product";

        string? IActivityLoggable.ActivityEntityId => null;

        string? IActivityLoggable.ActivityEntityDisplayName => EntityDisplayName;
    }
}

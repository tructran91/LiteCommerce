using Catalog.Application.Behaviors;
using Catalog.Application.Requests;
using Catalog.Application.Responses;
using Catalog.Core.Enums;
using LiteCommerce.Shared.Models;
using MediatR;

namespace Catalog.Application.Products.Commands
{
    public class CreateProductCommand : IRequest<BaseResponse<ProductResponse>>, IActivityLoggable
    {
        public CreateProductRequest Payload { get; set; }

        public CreateProductCommand(CreateProductRequest payload)
        {
            Payload = payload;
        }

        ActivityAction IActivityLoggable.ActivityAction => ActivityAction.Create;

        string IActivityLoggable.ActivityEntityName => "Product";

        string? IActivityLoggable.ActivityEntityId => null;

        string? IActivityLoggable.ActivityEntityDisplayName => Payload.Product?.Name;
    }
}

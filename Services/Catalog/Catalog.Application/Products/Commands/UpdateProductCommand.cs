using Catalog.Application.Behaviors;
using Catalog.Application.Requests;
using Catalog.Application.Responses;
using Catalog.Core.Enums;
using LiteCommerce.Shared.Models;
using MediatR;

namespace Catalog.Application.Products.Commands
{
    public class UpdateProductCommand : IRequest<BaseResponse<ProductResponse>>, IActivityLoggable
    {
        public string Id { get; set; }

        public UpdateProductRequest Payload { get; set; }

        public UpdateProductCommand(string id, UpdateProductRequest payload)
        {
            Id = id;
            Payload = payload;
        }

        ActivityAction IActivityLoggable.ActivityAction => ActivityAction.Update;

        string IActivityLoggable.ActivityEntityName => "Product";

        string? IActivityLoggable.ActivityEntityId => Id;

        string? IActivityLoggable.ActivityEntityDisplayName => Payload.Product?.Name;
    }
}

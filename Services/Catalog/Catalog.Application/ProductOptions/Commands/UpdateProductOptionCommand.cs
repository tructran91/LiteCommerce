using Catalog.Application.Behaviors;
using Catalog.Application.Requests;
using Catalog.Application.Responses;
using Catalog.Core.Enums;
using LiteCommerce.Shared.Models;
using MediatR;

namespace Catalog.Application.ProductOptions.Commands
{
    public class UpdateProductOptionCommand : IRequest<BaseResponse<ProductOptionResponse>>, IActivityLoggable
    {
        public string Id { get; set; }

        public UpdateProductOptionRequest Payload { get; set; }

        public UpdateProductOptionCommand(string id, UpdateProductOptionRequest payload)
        {
            Id = id;
            Payload = payload;
        }

        ActivityAction IActivityLoggable.ActivityAction => ActivityAction.Update;

        string IActivityLoggable.ActivityEntityName => "ProductOption";

        string? IActivityLoggable.ActivityEntityId => Id;

        string? IActivityLoggable.ActivityEntityDisplayName => Payload.Name;
    }
}

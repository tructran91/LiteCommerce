using Catalog.Application.Behaviors;
using Catalog.Core.Enums;
using LiteCommerce.Shared.Models;
using MediatR;

namespace Catalog.Application.ProductOptions.Commands
{
    public class DeleteProductOptionCommand : IRequest<BaseResponse<bool>>, IActivityLoggable
    {
        public string Id { get; set; }

        // Set by the handler after it loads the entity, for the activity log description.
        public string? EntityDisplayName { get; set; }

        public DeleteProductOptionCommand(string id)
        {
            Id = id;
        }

        ActivityAction IActivityLoggable.ActivityAction => ActivityAction.Delete;

        string IActivityLoggable.ActivityEntityName => "ProductOption";

        string? IActivityLoggable.ActivityEntityId => Id;

        string? IActivityLoggable.ActivityEntityDisplayName => EntityDisplayName;
    }
}

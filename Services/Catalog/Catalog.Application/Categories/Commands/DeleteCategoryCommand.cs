using Catalog.Application.Behaviors;
using Catalog.Core.Enums;
using LiteCommerce.Shared.Models;
using MediatR;

namespace Catalog.Application.Categories.Commands
{
    public class DeleteCategoryCommand : IRequest<BaseResponse<bool>>, IActivityLoggable
    {
        public string Id { get; set; }

        // Set by the handler after it loads the entity, for the activity log description.
        public string? EntityDisplayName { get; set; }

        public DeleteCategoryCommand(string id)
        {
            Id = id;
        }

        ActivityAction IActivityLoggable.ActivityAction => ActivityAction.Delete;

        string IActivityLoggable.ActivityEntityName => "Category";

        string? IActivityLoggable.ActivityEntityId => Id;

        string? IActivityLoggable.ActivityEntityDisplayName => EntityDisplayName;
    }
}

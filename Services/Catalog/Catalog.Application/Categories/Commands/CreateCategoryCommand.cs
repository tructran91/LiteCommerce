using Catalog.Application.Behaviors;
using Catalog.Application.Requests;
using Catalog.Application.Responses;
using Catalog.Core.Enums;
using LiteCommerce.Shared.Models;
using MediatR;

namespace Catalog.Application.Categories.Commands
{
    public class CreateCategoryCommand : IRequest<BaseResponse<CategoryResponse>>, IActivityLoggable
    {
        public CreateCategoryRequest Payload { get; set; }

        public CreateCategoryCommand(CreateCategoryRequest payload)
        {
            Payload = payload;
        }

        ActivityAction IActivityLoggable.ActivityAction => ActivityAction.Create;

        string IActivityLoggable.ActivityEntityName => "Category";

        string? IActivityLoggable.ActivityEntityId => null;

        string? IActivityLoggable.ActivityEntityDisplayName => Payload.Name;
    }
}

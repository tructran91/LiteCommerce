using Catalog.Application.Behaviors;
using Catalog.Application.Requests;
using Catalog.Application.Responses;
using Catalog.Core.Enums;
using LiteCommerce.Shared.Models;
using MediatR;

namespace Catalog.Application.Categories.Commands
{
    public class UpdateCategoryCommand : IRequest<BaseResponse<CategoryResponse>>, IActivityLoggable
    {
        public string Id { get; set; }

        public UpdateCategoryRequest Payload { get; set; }

        public UpdateCategoryCommand(string id, UpdateCategoryRequest payload)
        {
            Id = id;
            Payload = payload;
        }

        ActivityAction IActivityLoggable.ActivityAction => ActivityAction.Update;

        string IActivityLoggable.ActivityEntityName => "Category";

        string? IActivityLoggable.ActivityEntityId => Id;

        string? IActivityLoggable.ActivityEntityDisplayName => Payload.Name;
    }
}

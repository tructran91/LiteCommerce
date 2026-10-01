using Catalog.Application.Behaviors;
using Catalog.Application.Requests;
using Catalog.Application.Responses;
using Catalog.Core.Enums;
using LiteCommerce.Shared.Models;
using MediatR;

namespace Catalog.Application.ProductTemplates.Commands
{
    public class UpdateProductTemplateCommand : IRequest<BaseResponse<ProductTemplateResponse>>, IActivityLoggable
    {
        public string Id { get; set; }

        public UpdateProductTemplateRequest Payload { get; set; }

        public UpdateProductTemplateCommand(string id, UpdateProductTemplateRequest payload)
        {
            Id = id;
            Payload = payload;
        }

        ActivityAction IActivityLoggable.ActivityAction => ActivityAction.Update;

        string IActivityLoggable.ActivityEntityName => "ProductTemplate";

        string? IActivityLoggable.ActivityEntityId => Id;

        string? IActivityLoggable.ActivityEntityDisplayName => Payload.Name;
    }
}

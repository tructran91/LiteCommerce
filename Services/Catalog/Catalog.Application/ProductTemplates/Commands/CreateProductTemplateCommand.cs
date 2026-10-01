using Catalog.Application.Behaviors;
using Catalog.Application.Requests;
using Catalog.Application.Responses;
using Catalog.Core.Enums;
using LiteCommerce.Shared.Models;
using MediatR;

namespace Catalog.Application.ProductTemplates.Commands
{
    public class CreateProductTemplateCommand : IRequest<BaseResponse<ProductTemplateResponse>>, IActivityLoggable
    {
        public CreateProductTemplateRequest Payload { get; set; }

        public CreateProductTemplateCommand(CreateProductTemplateRequest payload)
        {
            Payload = payload;
        }

        ActivityAction IActivityLoggable.ActivityAction => ActivityAction.Create;

        string IActivityLoggable.ActivityEntityName => "ProductTemplate";

        string? IActivityLoggable.ActivityEntityId => null;

        string? IActivityLoggable.ActivityEntityDisplayName => Payload.Name;
    }
}

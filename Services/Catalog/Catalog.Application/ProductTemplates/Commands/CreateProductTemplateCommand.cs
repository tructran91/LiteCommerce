using Catalog.Application.Requests;
using Catalog.Application.Responses;
using LiteCommerce.Shared.Models;
using MediatR;

namespace Catalog.Application.ProductTemplates.Commands
{
    public class CreateProductTemplateCommand : IRequest<BaseResponse<ProductTemplateResponse>>
    {
        public CreateProductTemplateRequest Payload { get; set; }

        public CreateProductTemplateCommand(CreateProductTemplateRequest payload)
        {
            Payload = payload;
        }
    }
}

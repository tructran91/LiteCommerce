using Catalog.Application.Requests;
using Catalog.Application.Responses;
using LiteCommerce.Shared.Models;
using MediatR;

namespace Catalog.Application.ProductTemplates.Commands
{
    public class UpdateProductTemplateCommand : IRequest<BaseResponse<ProductTemplateResponse>>
    {
        public UpdateProductTemplateRequest Payload { get; set; }

        public UpdateProductTemplateCommand(UpdateProductTemplateRequest payload)
        {
            Payload = payload;
        }
    }
}

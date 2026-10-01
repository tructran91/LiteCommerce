using Catalog.Application.Requests;
using Catalog.Application.Responses;
using LiteCommerce.Shared.Models;
using MediatR;

namespace Catalog.Application.ProductTemplates.Commands
{
    public class UpdateProductTemplateCommand : IRequest<BaseResponse<ProductTemplateResponse>>
    {
        public string Id { get; set; }

        public UpdateProductTemplateRequest Payload { get; set; }

        public UpdateProductTemplateCommand(string id, UpdateProductTemplateRequest payload)
        {
            Id = id;
            Payload = payload;
        }
    }
}

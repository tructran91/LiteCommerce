using LiteCommerce.Shared.Models;
using MediatR;

namespace Catalog.Application.ProductTemplates.Commands
{
    public class DeleteProductTemplateCommand : IRequest<BaseResponse<bool>>
    {
        public string Id { get; set; }

        public DeleteProductTemplateCommand(string id)
        {
            Id = id;
        }
    }
}

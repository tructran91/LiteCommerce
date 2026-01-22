using LiteCommerce.Shared.Models;
using MediatR;

namespace Catalog.Application.ProductAttributeGroups.Commands
{
    public class DeleteProductAttributeGroupCommand : IRequest<BaseResponse<bool>>
    {
        public string Id { get; set; }

        public DeleteProductAttributeGroupCommand(string id)
        {
            Id = id;
        }
    }
}

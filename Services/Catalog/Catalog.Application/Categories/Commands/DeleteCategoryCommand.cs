using Catalog.Application.Responses;
using LiteCommerce.Shared.Models;
using MediatR;

namespace Catalog.Application.Categories.Commands
{
    public class DeleteCategoryCommand : IRequest<BaseResponse<CategoryResponse>>
    {
        public string Id { get; set; }

        public DeleteCategoryCommand(string id)
        {
            Id = id;
        }
    }
}

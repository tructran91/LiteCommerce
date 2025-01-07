using Catalog.Application.Requests;
using Catalog.Application.Responses;
using LiteCommerce.Shared.Models;
using MediatR;

namespace Catalog.Application.Categories.Commands
{
    public class UpdateCategoryCommand : IRequest<BaseResponse<CategoryResponse>>
    {
        public UpdateCategoryRequest Payload { get; set; }

        public UpdateCategoryCommand(UpdateCategoryRequest payload)
        {
            Payload = payload;
        }
    }
}

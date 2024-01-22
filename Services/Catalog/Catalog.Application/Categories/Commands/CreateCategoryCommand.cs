using Catalog.Application.Requests;
using Catalog.Application.Responses;
using MediatR;

namespace Catalog.Application.Categories.Commands
{
    public class CreateCategoryCommand : IRequest<BaseResponse<CategoryResponse>>
    {
        public CreateCategoryRequest Payload { get; set; }

        public CreateCategoryCommand(CreateCategoryRequest payload)
        {
            Payload = payload;
        }
    }
}

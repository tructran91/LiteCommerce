using Catalog.Application.Responses;
using LiteCommerce.Shared.Models;
using MediatR;

namespace Catalog.Application.Categories.Queries
{
    public class GetCategoryQuery : IRequest<BaseResponse<CategoryResponse>>
    {
        public string Id { get; set; }

        public GetCategoryQuery(string id)
        {
            Id = id;
        }
    }
}

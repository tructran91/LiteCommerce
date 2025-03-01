using Catalog.Application.Responses;
using LiteCommerce.Shared.Models;
using MediatR;

namespace Catalog.Application.Categories.Queries
{
    public class GetAllBasicCategoriesQuery : IRequest<BaseResponse<List<BasicCategoryResponse>>>
    {
    }
}

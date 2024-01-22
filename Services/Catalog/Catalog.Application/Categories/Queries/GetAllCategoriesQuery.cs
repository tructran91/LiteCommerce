using Catalog.Application.Responses;
using MediatR;

namespace Catalog.Application.Brands.Queries
{
    public class GetAllCategoriesQuery : IRequest<BaseResponse<List<CategoryResponse>>>
    {
    }
}

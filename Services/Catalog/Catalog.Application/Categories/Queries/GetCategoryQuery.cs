using Catalog.Application.Responses;
using MediatR;

namespace Catalog.Application.Brands.Queries
{
    public class GetCategoryQuery : IRequest<BaseResponse<CategoryResponse>>
    {
        public Guid Id { get; set; }

        public GetCategoryQuery(Guid id)
        {
            Id = id;
        }
    }
}

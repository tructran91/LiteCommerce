using Catalog.Application.Responses;
using LiteCommerce.Shared.Models;
using MediatR;

namespace Catalog.Application.Brands.Queries
{
    public class GetBrandQuery : IRequest<BaseResponse<BrandResponse>>
    {
        public string Id { get; set; }

        public GetBrandQuery(string id)
        {
            Id = id;
        }
    }
}

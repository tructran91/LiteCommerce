using Catalog.Application.Responses;
using LiteCommerce.Shared.Models;
using MediatR;

namespace Catalog.Application.ProductOptions.Queries
{
    public class GetProductOptionQuery : IRequest<BaseResponse<ProductOptionResponse>>
    {
        public string Id { get; set; }

        public GetProductOptionQuery(string id)
        {
            Id = id;
        }
    }
}

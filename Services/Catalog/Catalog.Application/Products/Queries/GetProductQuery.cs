using Catalog.Application.Responses;
using LiteCommerce.Shared.Models;
using MediatR;

namespace Catalog.Application.Products.Queries
{
    public class GetProductQuery : IRequest<BaseResponse<ProductResponse>>
    {
        public string Id { get; set; }

        public GetProductQuery(string id)
        {
            Id = id;
        }
    }
}

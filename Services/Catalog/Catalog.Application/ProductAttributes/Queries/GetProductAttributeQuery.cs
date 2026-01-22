using Catalog.Application.Responses;
using LiteCommerce.Shared.Models;
using MediatR;

namespace Catalog.Application.ProductAttributes.Queries
{
    public class GetProductAttributeQuery : IRequest<BaseResponse<ProductAttributeResponse>>
    {
        public string Id { get; set; }

        public GetProductAttributeQuery(string id)
        {
            Id = id;
        }
    }
}

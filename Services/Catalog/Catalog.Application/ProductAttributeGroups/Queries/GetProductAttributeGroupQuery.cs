using Catalog.Application.Responses;
using LiteCommerce.Shared.Models;
using MediatR;

namespace Catalog.Application.ProductAttributeGroups.Queries
{
    public class GetProductAttributeGroupQuery : IRequest<BaseResponse<ProductAttributeGroupResponse>>
    {
        public string Id { get; set; }

        public GetProductAttributeGroupQuery(string id)
        {
            Id = id;
        }
    }
}

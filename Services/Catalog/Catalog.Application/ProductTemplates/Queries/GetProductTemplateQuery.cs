using Catalog.Application.Responses;
using LiteCommerce.Shared.Models;
using MediatR;

namespace Catalog.Application.ProductTemplates.Queries
{
    public class GetProductTemplateQuery : IRequest<BaseResponse<ProductTemplateResponse>>
    {
        public string Id { get; set; }

        public GetProductTemplateQuery(string id)
        {
            Id = id;
        }
    }
}

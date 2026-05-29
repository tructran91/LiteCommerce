using Catalog.Application.ProductPrices.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Repositories;
using LiteCommerce.Shared.Models;
using MediatR;

namespace Catalog.Application.ProductPrices.Handlers
{
    public class GetProductPricingHandler : IRequestHandler<GetProductPricingQuery, BaseResponse<List<ProductPricingResponse>>>
    {
        private readonly IProductRepository _productRepository;

        public GetProductPricingHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<BaseResponse<List<ProductPricingResponse>>> Handle(GetProductPricingQuery request, CancellationToken cancellationToken)
        {
            var (products, totalCount) = await _productRepository.GetProductPricingAsync(request.CurrentPage, request.PageSize);

            var response = products.Select(p => new ProductPricingResponse
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                OldPrice = p.OldPrice,
                SpecialPrice = p.SpecialPrice,
                SpecialPriceStart = p.SpecialPriceStart,
                SpecialPriceEnd = p.SpecialPriceEnd
            }).ToList();

            var result = BaseResponse<List<ProductPricingResponse>>.Success(response);
            result.Pagination = new Pagination(totalCount, request.CurrentPage, request.PageSize);

            return result;
        }
    }
}

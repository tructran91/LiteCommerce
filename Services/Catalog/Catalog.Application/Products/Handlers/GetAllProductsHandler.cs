using AutoMapper;
using Catalog.Application.Products.Queries;
using Catalog.Application.Responses;
using Catalog.Core.DTOs;
using Catalog.Core.Repositories;
using LiteCommerce.Shared.Models;
using MediatR;

namespace Catalog.Application.Products.Handlers
{
    public class GetAllProductsHandler : IRequestHandler<GetAllProductsQuery, BaseResponse<List<BasicProductResponse>>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public GetAllProductsHandler(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse<List<BasicProductResponse>>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var filter = _mapper.Map<ProductListFilter>(request);
            var (products, totalCount) = await _productRepository.GetProductsAsync(
                filter,
                request.CurrentPage,
                request.PageSize,
                cancellationToken);

            var response = products.Select(p => new BasicProductResponse
            {
                Id = p.Id,
                Name = p.Name,
                IsFeatured = p.IsFeatured,
                IsAllowToOrder = p.IsAllowToOrder,
                IsCallForPricing = p.IsCallForPricing,
                IsPublished = p.IsPublished
            }).ToList();

            var result = BaseResponse<List<BasicProductResponse>>.Success(response);
            result.Pagination = new Pagination(totalCount, request.CurrentPage, request.PageSize);

            return result;
        }
    }
}

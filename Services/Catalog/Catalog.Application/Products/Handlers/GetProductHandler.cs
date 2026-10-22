using Catalog.Application.Products.Queries;
using Catalog.Application.Responses;
using Catalog.Application.Services;
using Catalog.Core.Repositories;
using LiteCommerce.Shared.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Catalog.Application.Products.Handlers
{
    public class GetProductHandler : IRequestHandler<GetProductQuery, BaseResponse<ProductResponse>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductService _productService;
        private readonly ILogger<GetProductHandler> _logger;

        public GetProductHandler(IProductRepository productRepository,
            IProductService productService,
            ILogger<GetProductHandler> logger)
        {
            _productRepository = productRepository;
            _productService = productService;
            _logger = logger;
        }

        public async Task<BaseResponse<ProductResponse>> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetProductAsync(Guid.Parse(request.Id));
            if (product is null)
            {
                return BaseResponse<ProductResponse>.Failure("Product does not exist.", statusCode: HttpStatusCode.NotFound);
            }

            _logger.LogInformation("GetProductHandler: Loading product Id={ProductId}, Name={ProductName}", product.Id, product.Name);

            return BaseResponse<ProductResponse>.Success(_productService.BuildProductResponse(product));
        }
    }
}

using AutoMapper;
using Catalog.Application.ProductPrices.Commands;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using LiteCommerce.Shared.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace Catalog.Application.ProductPrices.Handlers
{
    public class UpdateProductPricingHandler : IRequestHandler<UpdateProductPricingCommand, BaseResponse<List<ProductPricingResponse>>>
    {
        private readonly IBaseRepository<Product> _productRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateProductPricingHandler> _logger;

        public UpdateProductPricingHandler(IBaseRepository<Product> productRepository, IMapper mapper, ILogger<UpdateProductPricingHandler> logger)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<List<ProductPricingResponse>>> Handle(UpdateProductPricingCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"UpdateProductPricingHandler: {JsonSerializer.Serialize(request.Payload)}");

            var requestedIds = request.Payload.Items
                .Select(x => Guid.Parse(x.Id))
                .Distinct()
                .ToList();

            var existingProducts = _productRepository.Query()
                .Where(p => requestedIds.Contains(p.Id))
                .ToList();

            var existingProductIds = existingProducts.Select(p => p.Id).ToHashSet();
            var notFoundIds = requestedIds.Where(id => !existingProductIds.Contains(id)).ToList();

            if (notFoundIds.Any())
            {
                var notFoundIdStrings = notFoundIds.Select(id => id.ToString()).ToList();
                var errors = new Dictionary<string, List<string>>
                {
                    { "NotFoundProductIds", notFoundIdStrings }
                };
                return BaseResponse<List<ProductPricingResponse>>.Failure(
                    $"The following product IDs were not found: {string.Join(", ", notFoundIdStrings)}",
                    errors: errors,
                    statusCode: HttpStatusCode.NotFound);
            }

            var productDict = existingProducts.ToDictionary(p => p.Id);

            foreach (var item in request.Payload.Items)
            {
                if (!item.NewOldPrice.HasValue && !item.NewPrice.HasValue)
                {
                    continue;
                }

                var product = productDict[Guid.Parse(item.Id)];

                var productPriceHistory = new ProductPriceHistory
                {
                    Product = product,
                    OldPrice = product.OldPrice,
                    Price = product.Price,
                    SpecialPrice = product.SpecialPrice,
                    SpecialPriceStart = product.SpecialPriceStart,
                    SpecialPriceEnd = product.SpecialPriceEnd
                };

                product.OldPrice = item.NewOldPrice;
                productPriceHistory.OldPrice = item.NewOldPrice;

                if (item.NewPrice.HasValue)
                {
                    product.Price = item.NewPrice.Value;
                    productPriceHistory.Price = item.NewPrice.Value;
                }

                product.PriceHistories.Add(productPriceHistory);
            }

            foreach (var product in existingProducts)
            {
                await _productRepository.UpdateAsync(product);
            }

            var response = _mapper.Map<List<ProductPricingResponse>>(existingProducts);

            return BaseResponse<List<ProductPricingResponse>>.Success(response);
        }
    }
}

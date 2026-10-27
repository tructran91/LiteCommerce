using Catalog.Application.Products.Commands;
using Catalog.Core.Repositories;
using LiteCommerce.Shared.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace Catalog.Application.Products.Handlers
{
    public class DeleteProductHandler : IRequestHandler<DeleteProductCommand, BaseResponse<bool>>
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<DeleteProductHandler> _logger;

        public DeleteProductHandler(IProductRepository productRepository, ILogger<DeleteProductHandler> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        public async Task<BaseResponse<bool>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("DeleteProductHandler: {Request}", JsonSerializer.Serialize(request));

            var productId = Guid.Parse(request.Id);
            // The full aggregate, not GetByIdAsync: the variants and their option values
            // have to be tracked so the soft delete below reaches them too.
            var existingProduct = await _productRepository.GetProductAsync(productId);
            if (existingProduct == null)
            {
                return BaseResponse<bool>.Failure("Product does not exist.", statusCode: HttpStatusCode.NotFound);
            }

            // The command only carries the id; give the activity log a readable name.
            request.EntityDisplayName = existingProduct.Name;

            // Both directions: this product's related/cross-sell links, and other products' links pointing at it.
            var links = await _productRepository.GetLinksInvolvingAsync(productId);
            foreach (var link in links)
            {
                link.IsDeleted = true;
            }

            // Without this the variants stay visible to queries that target them directly,
            // and their unique indexes keep blocking a re-created combination.
            foreach (var variant in existingProduct.Variants)
            {
                foreach (var optionValue in variant.OptionValues)
                {
                    optionValue.IsDeleted = true;
                }
                variant.IsDeleted = true;
            }

            foreach (var optionValue in existingProduct.OptionValues)
            {
                foreach (var item in optionValue.Items)
                {
                    item.IsDeleted = true;
                }
                optionValue.IsDeleted = true;
            }

            existingProduct.IsDeleted = true;

            // Links, variants, options and product are tracked by the same context,
            // so this is a single SaveChanges.
            await _productRepository.UpdateAsync(existingProduct);

            _logger.LogInformation(
                "DeleteProductHandler: {ProductId} deleted with {LinkCount} link(s) and {VariantCount} variant(s)",
                productId, links.Count, existingProduct.Variants.Count);

            return BaseResponse<bool>.Success(true);
        }
    }
}

using Catalog.Application.Products.Commands;
using Catalog.Core.Repositories;
using LiteCommerce.Shared.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Net;

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
            _logger.LogInformation("DeleteProductHandler: {ProductId}", request.Id);

            var productId = Guid.Parse(request.Id);
            var existingProduct = await _productRepository.GetByIdAsync(productId);
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

            existingProduct.IsDeleted = true;

            // Links and product are tracked by the same context, so this is a single SaveChanges.
            await _productRepository.UpdateAsync(existingProduct);

            _logger.LogInformation("DeleteProductHandler: {ProductId} deleted with {LinkCount} link(s)", productId, links.Count);

            return BaseResponse<bool>.Success(true);
        }
    }
}

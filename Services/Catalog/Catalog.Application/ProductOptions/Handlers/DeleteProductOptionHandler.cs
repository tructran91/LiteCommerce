using Catalog.Application.ProductOptions.Commands;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using LiteCommerce.Shared.Constants;
using LiteCommerce.Shared.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace Catalog.Application.ProductOptions.Handlers
{
    public class DeleteProductOptionHandler : IRequestHandler<DeleteProductOptionCommand, BaseResponse<bool>>
    {
        private readonly IBaseRepository<ProductOption> _productOptionRepository;
        private readonly IBaseRepository<Product> _productRepository;
        private readonly ILogger<DeleteProductOptionHandler> _logger;

        public DeleteProductOptionHandler(IBaseRepository<ProductOption> productOptionRepository,
            IBaseRepository<Product> productRepository,
            ILogger<DeleteProductOptionHandler> logger)
        {
            _productOptionRepository = productOptionRepository;
            _productRepository = productRepository;
            _logger = logger;
        }

        public async Task<BaseResponse<bool>> Handle(DeleteProductOptionCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"DeleteProductOptionHandler: {JsonSerializer.Serialize(request)}");

            var existingProductOption = await _productOptionRepository
                .GetByIdAsync(Guid.Parse(request.Id));
            if (existingProductOption == null)
            {
                return BaseResponse<bool>.Failure("Product Option does not exist.", statusCode: HttpStatusCode.NotFound);
            }

            // The command only carries the id; give the activity log a readable name.
            request.EntityDisplayName = existingProductOption.Name;

            var productCount = await _productRepository
                .CountAsync(p => p.OptionValues.Any(o => o.OptionId == existingProductOption.Id));
            if (productCount > 0)
            {
                return BaseResponse<bool>.Failure(
                    ErrorMessages.CannotDeleteInUse("product option", existingProductOption.Name, productCount, "product", "products"),
                    statusCode: HttpStatusCode.Conflict);
            }

            existingProductOption.IsDeleted = true;
            existingProductOption.LastModifiedDate = DateTime.UtcNow;
            await _productOptionRepository.UpdateAsync(existingProductOption);

            return BaseResponse<bool>.Success(true);
        }
    }
}

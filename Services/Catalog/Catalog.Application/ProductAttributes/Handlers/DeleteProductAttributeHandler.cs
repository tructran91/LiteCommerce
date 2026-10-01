using Catalog.Application.ProductAttributes.Commands;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using LiteCommerce.Shared.Constants;
using LiteCommerce.Shared.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace Catalog.Application.ProductAttributes.Handlers
{
    public class DeleteProductAttributeHandler : IRequestHandler<DeleteProductAttributeCommand, BaseResponse<bool>>
    {
        private readonly IBaseRepository<ProductAttribute> _productAttributeRepository;
        private readonly IBaseRepository<ProductTemplateProductAttribute> _templateAttributeRepository;
        private readonly IBaseRepository<Product> _productRepository;
        private readonly ILogger<DeleteProductAttributeHandler> _logger;

        public DeleteProductAttributeHandler(
            IBaseRepository<ProductAttribute> productAttributeRepository,
            IBaseRepository<ProductTemplateProductAttribute> templateAttributeRepository,
            IBaseRepository<Product> productRepository,
            ILogger<DeleteProductAttributeHandler> logger)
        {
            _productAttributeRepository = productAttributeRepository;
            _templateAttributeRepository = templateAttributeRepository;
            _productRepository = productRepository;
            _logger = logger;
        }

        public async Task<BaseResponse<bool>> Handle(DeleteProductAttributeCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"DeleteProductAttributeHandler: {JsonSerializer.Serialize(request)}");

            var existingProductAttribute = await _productAttributeRepository
                .GetByIdAsync(Guid.Parse(request.Id));
            if (existingProductAttribute == null)
            {
                return BaseResponse<bool>.Failure("Product Attribute does not exist.", statusCode: HttpStatusCode.NotFound);
            }

            // The command only carries the id; give the activity log a readable name.
            request.EntityDisplayName = existingProductAttribute.Name;

            var productCount = await _productRepository
                .CountAsync(p => p.AttributeValues.Any(av => av.AttributeId == existingProductAttribute.Id));
            if (productCount > 0)
            {
                return BaseResponse<bool>.Failure(
                    ErrorMessages.CannotDeleteInUse("product attribute", existingProductAttribute.Name, productCount, "product", "products"),
                    statusCode: HttpStatusCode.Conflict);
            }

            var templateCount = await _templateAttributeRepository
                .CountAsync(ta => ta.ProductAttributeId == existingProductAttribute.Id);
            if (templateCount > 0)
            {
                return BaseResponse<bool>.Failure(
                    ErrorMessages.CannotDeleteInUse("product attribute", existingProductAttribute.Name, templateCount, "product template", "product templates"),
                    statusCode: HttpStatusCode.Conflict);
            }

            existingProductAttribute.IsDeleted = true;
            existingProductAttribute.LastModifiedDate = DateTime.UtcNow;
            await _productAttributeRepository.UpdateAsync(existingProductAttribute);

            return BaseResponse<bool>.Success(true);
        }
    }
}

using Catalog.Application.ProductAttributes.Commands;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
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
        private readonly ILogger<DeleteProductAttributeHandler> _logger;

        public DeleteProductAttributeHandler(
            IBaseRepository<ProductAttribute> productAttributeRepository,
            IBaseRepository<ProductTemplateProductAttribute> templateAttributeRepository,
            ILogger<DeleteProductAttributeHandler> logger)
        {
            _productAttributeRepository = productAttributeRepository;
            _templateAttributeRepository = templateAttributeRepository;
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

            var isUsedInTemplates = await _templateAttributeRepository
                .AnyAsync(ta => ta.ProductAttributeId == existingProductAttribute.Id);
            if (isUsedInTemplates)
            {
                return BaseResponse<bool>.Failure(
                    "Cannot delete Product Attribute because it is being used by one or more Product Templates.",
                    statusCode: HttpStatusCode.BadRequest);
            }

            existingProductAttribute.IsDeleted = true;
            existingProductAttribute.LastModifiedDate = DateTime.UtcNow;
            await _productAttributeRepository.UpdateAsync(existingProductAttribute);

            return BaseResponse<bool>.Success(true);
        }
    }
}

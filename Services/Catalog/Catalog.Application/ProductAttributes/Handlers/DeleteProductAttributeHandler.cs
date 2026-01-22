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
        private readonly ILogger<DeleteProductAttributeHandler> _logger;

        public DeleteProductAttributeHandler(IBaseRepository<ProductAttribute> productAttributeRepository, ILogger<DeleteProductAttributeHandler> logger)
        {
            _productAttributeRepository = productAttributeRepository;
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

            existingProductAttribute.IsDeleted = true;
            existingProductAttribute.LastModifiedDate = DateTime.UtcNow;
            await _productAttributeRepository.UpdateAsync(existingProductAttribute);

            return BaseResponse<bool>.Success(true);
        }
    }
}

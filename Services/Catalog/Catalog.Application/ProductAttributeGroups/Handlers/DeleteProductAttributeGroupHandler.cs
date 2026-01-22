using Catalog.Application.ProductAttributeGroups.Commands;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using LiteCommerce.Shared.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace Catalog.Application.ProductAttributeGroups.Handlers
{
    public class DeleteProductAttributeGroupHandler : IRequestHandler<DeleteProductAttributeGroupCommand, BaseResponse<bool>>
    {
        private readonly IBaseRepository<ProductAttributeGroup> _productAttributeGroupRepository;
        private readonly ILogger<DeleteProductAttributeGroupHandler> _logger;

        public DeleteProductAttributeGroupHandler(IBaseRepository<ProductAttributeGroup> productAttributeGroupRepository, ILogger<DeleteProductAttributeGroupHandler> logger)
        {
            _productAttributeGroupRepository = productAttributeGroupRepository;
            _logger = logger;
        }

        public async Task<BaseResponse<bool>> Handle(DeleteProductAttributeGroupCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"DeleteProductAttributeGroupHandler: {JsonSerializer.Serialize(request)}");

            var existingProductAttributeGroup = await _productAttributeGroupRepository
                .GetByIdAsync(Guid.Parse(request.Id));
            if (existingProductAttributeGroup == null)
            {
                return BaseResponse<bool>.Failure("Product Attribute Group does not exist.", statusCode: HttpStatusCode.NotFound);
            }

            existingProductAttributeGroup.IsDeleted = true;
            existingProductAttributeGroup.LastModifiedDate = DateTime.UtcNow;
            await _productAttributeGroupRepository.UpdateAsync(existingProductAttributeGroup);

            return BaseResponse<bool>.Success(true);
        }
    }
}

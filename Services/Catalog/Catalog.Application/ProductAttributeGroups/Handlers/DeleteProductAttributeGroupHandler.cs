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
        private readonly IBaseRepository<ProductAttribute> _productAttributeRepository;
        private readonly ILogger<DeleteProductAttributeGroupHandler> _logger;

        public DeleteProductAttributeGroupHandler(
            IBaseRepository<ProductAttributeGroup> productAttributeGroupRepository,
            IBaseRepository<ProductAttribute> productAttributeRepository,
            ILogger<DeleteProductAttributeGroupHandler> logger)
        {
            _productAttributeGroupRepository = productAttributeGroupRepository;
            _productAttributeRepository = productAttributeRepository;
            _logger = logger;
        }

        public async Task<BaseResponse<bool>> Handle(DeleteProductAttributeGroupCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"DeleteProductAttributeGroupHandler: {JsonSerializer.Serialize(request)}");

            var existingProductAttributeGroup = await _productAttributeGroupRepository
                .GetByIdAsync(Guid.Parse(request.Id));
            if (existingProductAttributeGroup == null)
            {
                return BaseResponse<bool>.Failure("Attribute Group does not exist.", statusCode: HttpStatusCode.NotFound);
            }

            var productAttributesInGroup = await _productAttributeRepository
                .AnyAsync(pa => pa.GroupId == existingProductAttributeGroup.Id);
            if (productAttributesInGroup)
            {
                return BaseResponse<bool>.Failure(
                    "Cannot delete Attribute Group because it is being used by one or more Attributes.",
                    statusCode: HttpStatusCode.BadRequest);
            }

            existingProductAttributeGroup.IsDeleted = true;
            existingProductAttributeGroup.LastModifiedDate = DateTime.UtcNow;
            await _productAttributeGroupRepository.UpdateAsync(existingProductAttributeGroup);

            return BaseResponse<bool>.Success(true);
        }
    }
}

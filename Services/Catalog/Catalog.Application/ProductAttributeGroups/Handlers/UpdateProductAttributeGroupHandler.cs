using AutoMapper;
using Catalog.Application.ProductAttributeGroups.Commands;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using LiteCommerce.Shared.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace Catalog.Application.ProductAttributeGroups.Handlers
{
    public class UpdateProductAttributeGroupHandler : IRequestHandler<UpdateProductAttributeGroupCommand, BaseResponse<ProductAttributeGroupResponse>>
    {
        private readonly IBaseRepository<ProductAttributeGroup> _productAttributeGroupRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateProductAttributeGroupHandler> _logger;

        public UpdateProductAttributeGroupHandler(IBaseRepository<ProductAttributeGroup> productAttributeGroupRepository, IMapper mapper, ILogger<UpdateProductAttributeGroupHandler> logger)
        {
            _productAttributeGroupRepository = productAttributeGroupRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<ProductAttributeGroupResponse>> Handle(UpdateProductAttributeGroupCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"UpdateProductAttributeGroupHandler: {JsonSerializer.Serialize(request.Payload)}");

            var existingProductAttributeGroup = await _productAttributeGroupRepository
                .GetByIdAsync(Guid.Parse(request.Payload.Id));
            if (existingProductAttributeGroup == null)
            {
                return BaseResponse<ProductAttributeGroupResponse>.Failure("Product Attribute Group does not exist.", statusCode: HttpStatusCode.NotFound);
            }

            var existingProductAttributeGroupByName = await _productAttributeGroupRepository
                .AnyAsync(t => t.Name.ToLower() == request.Payload.Name.ToLower() && t.Id.ToString() != request.Payload.Id);
            if (existingProductAttributeGroupByName)
            {
                return BaseResponse<ProductAttributeGroupResponse>.Failure("Product Attribute Group already exists.", statusCode: HttpStatusCode.Conflict);
            }

            _mapper.Map(request.Payload, existingProductAttributeGroup);

            await _productAttributeGroupRepository.UpdateAsync(existingProductAttributeGroup);
            var response = _mapper.Map<ProductAttributeGroupResponse>(existingProductAttributeGroup);

            return BaseResponse<ProductAttributeGroupResponse>.Success(response);
        }
    }
}

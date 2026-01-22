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
    public class CreateProductAttributeGroupHandler : IRequestHandler<CreateProductAttributeGroupCommand, BaseResponse<ProductAttributeGroupResponse>>
    {
        private readonly IBaseRepository<ProductAttributeGroup> _ProductAttributeGroupRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateProductAttributeGroupHandler> _logger;

        public CreateProductAttributeGroupHandler(IBaseRepository<ProductAttributeGroup> ProductAttributeGroupRepository, IMapper mapper, ILogger<CreateProductAttributeGroupHandler> logger)
        {
            _ProductAttributeGroupRepository = ProductAttributeGroupRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<ProductAttributeGroupResponse>> Handle(CreateProductAttributeGroupCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"CreateProductAttributeGroupHandler: {JsonSerializer.Serialize(request.Payload)}");

            var isExistingProductAttributeGroup = await _ProductAttributeGroupRepository.AnyAsync(t => t.Name.ToLower() == request.Payload.Name.ToLower());
            if (isExistingProductAttributeGroup)
            {
                return BaseResponse<ProductAttributeGroupResponse>.Failure("Product Option already exists.", statusCode: HttpStatusCode.Conflict);
            }

            var newProductAttributeGroup = _mapper.Map<ProductAttributeGroup>(request.Payload);

            var createdProductAttributeGroup = await _ProductAttributeGroupRepository.AddAsync(newProductAttributeGroup);
            var responseMapping = _mapper.Map<ProductAttributeGroupResponse>(createdProductAttributeGroup);

            return BaseResponse<ProductAttributeGroupResponse>.Success(responseMapping);
        }
    }
}

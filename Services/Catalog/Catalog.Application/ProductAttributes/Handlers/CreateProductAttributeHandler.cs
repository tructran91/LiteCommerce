using AutoMapper;
using Catalog.Application.ProductAttributes.Commands;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using LiteCommerce.Shared.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace Catalog.Application.ProductAttributes.Handlers
{
    public class CreateProductAttributeHandler : IRequestHandler<CreateProductAttributeCommand, BaseResponse<ProductAttributeResponse>>
    {
        private readonly IBaseRepository<ProductAttribute> _ProductAttributeRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateProductAttributeHandler> _logger;

        public CreateProductAttributeHandler(IBaseRepository<ProductAttribute> ProductAttributeRepository, IMapper mapper, ILogger<CreateProductAttributeHandler> logger)
        {
            _ProductAttributeRepository = ProductAttributeRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<ProductAttributeResponse>> Handle(CreateProductAttributeCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"CreateProductAttributeHandler: {JsonSerializer.Serialize(request.Payload)}");

            var isExistingProductAttribute = await _ProductAttributeRepository.AnyAsync(t => t.Name.ToLower() == request.Payload.Name.ToLower());
            if (isExistingProductAttribute)
            {
                return BaseResponse<ProductAttributeResponse>.Failure("Product Attribute already exists.", statusCode: HttpStatusCode.Conflict);
            }

            var newProductAttribute = _mapper.Map<ProductAttribute>(request.Payload);

            var createdProductAttribute = await _ProductAttributeRepository.AddAsync(newProductAttribute);
            var responseMapping = _mapper.Map<ProductAttributeResponse>(createdProductAttribute);

            return BaseResponse<ProductAttributeResponse>.Success(responseMapping);
        }
    }
}

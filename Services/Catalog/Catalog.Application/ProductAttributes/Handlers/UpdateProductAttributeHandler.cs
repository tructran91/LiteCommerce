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
    public class UpdateProductAttributeHandler : IRequestHandler<UpdateProductAttributeCommand, BaseResponse<ProductAttributeResponse>>
    {
        private readonly IBaseRepository<ProductAttribute> _productAttributeRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateProductAttributeHandler> _logger;

        public UpdateProductAttributeHandler(IBaseRepository<ProductAttribute> productAttributeRepository, IMapper mapper, ILogger<UpdateProductAttributeHandler> logger)
        {
            _productAttributeRepository = productAttributeRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<ProductAttributeResponse>> Handle(UpdateProductAttributeCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"UpdateProductAttributeHandler: {JsonSerializer.Serialize(request.Payload)}");

            var existingProductAttribute = await _productAttributeRepository
                .GetByIdAsync(Guid.Parse(request.Payload.Id));
            if (existingProductAttribute == null)
            {
                return BaseResponse<ProductAttributeResponse>.Failure("Product Attribute does not exist.", statusCode: HttpStatusCode.NotFound);
            }

            var existingProductAttributeByName = await _productAttributeRepository
                .AnyAsync(t => t.Name.ToLower() == request.Payload.Name.ToLower() && t.Id.ToString() != request.Payload.Id);
            if (existingProductAttributeByName)
            {
                return BaseResponse<ProductAttributeResponse>.Failure("Product Attribute already exists.", statusCode: HttpStatusCode.Conflict);
            }

            _mapper.Map(request.Payload, existingProductAttribute);

            await _productAttributeRepository.UpdateAsync(existingProductAttribute);
            var response = _mapper.Map<ProductAttributeResponse>(existingProductAttribute);

            return BaseResponse<ProductAttributeResponse>.Success(response);
        }
    }
}

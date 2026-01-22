using AutoMapper;
using Catalog.Application.ProductOptions.Commands;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using LiteCommerce.Shared.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace Catalog.Application.ProductOptions.Handlers
{
    public class CreateProductOptionHandler : IRequestHandler<CreateProductOptionCommand, BaseResponse<ProductOptionResponse>>
    {
        private readonly IBaseRepository<ProductOption> _productOptionRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateProductOptionHandler> _logger;

        public CreateProductOptionHandler(IBaseRepository<ProductOption> productOptionRepository, IMapper mapper, ILogger<CreateProductOptionHandler> logger)
        {
            _productOptionRepository = productOptionRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<ProductOptionResponse>> Handle(CreateProductOptionCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"CreateProductOptionHandler: {JsonSerializer.Serialize(request.Payload)}");

            var isExistingProductOption = await _productOptionRepository.AnyAsync(t => t.Name.ToLower() == request.Payload.Name.ToLower());
            if (isExistingProductOption)
            {
                return BaseResponse<ProductOptionResponse>.Failure("Product Option already exists.", statusCode: HttpStatusCode.Conflict);
            }

            var newProductOption = _mapper.Map<ProductOption>(request.Payload);

            var createdProductOption = await _productOptionRepository.AddAsync(newProductOption);
            var responseMapping = _mapper.Map<ProductOptionResponse>(createdProductOption);

            return BaseResponse<ProductOptionResponse>.Success(responseMapping);
        }
    }
}

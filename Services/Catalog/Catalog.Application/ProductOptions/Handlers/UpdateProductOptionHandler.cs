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
    public class UpdateProductOptionHandler : IRequestHandler<UpdateProductOptionCommand, BaseResponse<ProductOptionResponse>>
    {
        private readonly IBaseRepository<ProductOption> _productOptionRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateProductOptionHandler> _logger;

        public UpdateProductOptionHandler(IBaseRepository<ProductOption> productOptionRepository, IMapper mapper, ILogger<UpdateProductOptionHandler> logger)
        {
            _productOptionRepository = productOptionRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<ProductOptionResponse>> Handle(UpdateProductOptionCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"UpdateProductOptionHandler: {JsonSerializer.Serialize(request.Payload)}");

            var existingProductOption = await _productOptionRepository
                .GetByIdAsync(Guid.Parse(request.Payload.Id));
            if (existingProductOption == null)
            {
                return BaseResponse<ProductOptionResponse>.Failure("Product Option does not exist.", statusCode: HttpStatusCode.NotFound);
            }

            var existingProductOptionByName = await _productOptionRepository
                .AnyAsync(t => t.Name.ToLower() == request.Payload.Name.ToLower() && t.Id.ToString() != request.Payload.Id);
            if (existingProductOptionByName)
            {
                return BaseResponse<ProductOptionResponse>.Failure("Product Option already exists.", statusCode: HttpStatusCode.Conflict);
            }

            _mapper.Map(request.Payload, existingProductOption);

            await _productOptionRepository.UpdateAsync(existingProductOption);
            var response = _mapper.Map<ProductOptionResponse>(existingProductOption);

            return BaseResponse<ProductOptionResponse>.Success(response);
        }
    }
}

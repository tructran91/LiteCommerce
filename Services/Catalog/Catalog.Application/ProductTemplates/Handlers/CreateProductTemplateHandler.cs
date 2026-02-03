using AutoMapper;
using Catalog.Application.ProductTemplates.Commands;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using LiteCommerce.Shared.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace Catalog.Application.ProductTemplates.Handlers
{
    public class CreateProductTemplateHandler : IRequestHandler<CreateProductTemplateCommand, BaseResponse<ProductTemplateResponse>>
    {
        private readonly IBaseRepository<ProductTemplate> _productTemplateRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateProductTemplateHandler> _logger;

        public CreateProductTemplateHandler(IBaseRepository<ProductTemplate> productTemplateRepository, IMapper mapper, ILogger<CreateProductTemplateHandler> logger)
        {
            _productTemplateRepository = productTemplateRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<ProductTemplateResponse>> Handle(CreateProductTemplateCommand request, CancellationToken cancellationToken)
        {
            var payload = request.Payload;
            _logger.LogInformation($"CreateProductTemplateHandler: {JsonSerializer.Serialize(payload)}");

            var isExistingTemplate = await _productTemplateRepository.AnyAsync(t => t.Name.ToLower() == payload.Name.ToLower());
            if (isExistingTemplate)
            {
                return BaseResponse<ProductTemplateResponse>.Failure("Product Template already exists.", statusCode: HttpStatusCode.Conflict);
            }

            var newTemplate = _mapper.Map<ProductTemplate>(payload);

            foreach (var attribute in payload.ProductAttributes)
            {
                newTemplate.ProductAttributes.Add(new ProductTemplateProductAttribute
                {
                    Id = Guid.NewGuid(),
                    ProductAttributeId = Guid.Parse(attribute.Id),
                    CreatedDate = DateTime.UtcNow
                });
            }

            var createdTemplate = await _productTemplateRepository.AddAsync(newTemplate);
            var responseTemplate = new ProductTemplateResponse
            {
                Id = createdTemplate.Id,
                Name = createdTemplate.Name,
                ProductAttributes = payload.ProductAttributes
            };

            return BaseResponse<ProductTemplateResponse>.Success(responseTemplate);
        }
    }
}

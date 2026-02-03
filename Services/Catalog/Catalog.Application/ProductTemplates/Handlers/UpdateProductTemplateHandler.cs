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
    public class UpdateProductTemplateHandler : IRequestHandler<UpdateProductTemplateCommand, BaseResponse<ProductTemplateResponse>>
    {
        private readonly IBaseRepository<ProductTemplate> _productTemplateRepository;
        private readonly ILogger<UpdateProductTemplateHandler> _logger;

        public UpdateProductTemplateHandler(
            IBaseRepository<ProductTemplate> productTemplateRepository,
            ILogger<UpdateProductTemplateHandler> logger)
        {
            _productTemplateRepository = productTemplateRepository;
            _logger = logger;
        }

        public async Task<BaseResponse<ProductTemplateResponse>> Handle(UpdateProductTemplateCommand request, CancellationToken cancellationToken)
        {
            var payload = request.Payload;
            _logger.LogInformation($"UpdateProductTemplateHandler: {JsonSerializer.Serialize(payload)}");

            var existingTemplates = await _productTemplateRepository.GetAsync(
                predicate: t => t.Id == Guid.Parse(payload.Id) && !t.IsDeleted,
                includeString: "ProductAttributes.ProductAttribute",
                disableTracking: false);
            var existingTemplate = existingTemplates.FirstOrDefault();
            if (existingTemplate == null)
            {
                return BaseResponse<ProductTemplateResponse>.Failure("Product Template does not exist.", statusCode: HttpStatusCode.NotFound);
            }

            var existingTemplateByName = await _productTemplateRepository
                .AnyAsync(t => t.Name.ToLower() == payload.Name.ToLower() && t.Id.ToString() != payload.Id);
            if (existingTemplateByName)
            {
                return BaseResponse<ProductTemplateResponse>.Failure("Product Template already exists.", statusCode: HttpStatusCode.Conflict);
            }

            existingTemplate.Name = payload.Name;
            foreach (var attr in payload.ProductAttributes)
            {
                if (existingTemplate.ProductAttributes.Any(x => x.ProductAttributeId == Guid.Parse(attr.Id)))
                {
                    continue;
                }

                existingTemplate.ProductAttributes.Add(new ProductTemplateProductAttribute
                {
                    Id = Guid.NewGuid(),
                    ProductAttributeId = Guid.Parse(attr.Id),
                    CreatedDate = DateTime.UtcNow
                });
            }

            var payloadAttributeIds = payload.ProductAttributes.Select(x => Guid.Parse(x.Id)).ToList();
            var deletedAttributes = existingTemplate.ProductAttributes
                .Where(attr => !payloadAttributeIds.Contains(attr.ProductAttributeId))
                .ToList();

            foreach (var attr in deletedAttributes)
            {
                existingTemplate.ProductAttributes.Remove(attr);
            }

            await _productTemplateRepository.UpdateAsync(existingTemplate);
            var responseTemplate = new ProductTemplateResponse
            {
                Id = existingTemplate.Id,
                Name = existingTemplate.Name,
                ProductAttributes = payload.ProductAttributes
            };

            return BaseResponse<ProductTemplateResponse>.Success(responseTemplate);
        }
    }
}

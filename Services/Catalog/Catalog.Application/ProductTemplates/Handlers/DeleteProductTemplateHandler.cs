using Catalog.Application.ProductTemplates.Commands;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using LiteCommerce.Shared.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace Catalog.Application.ProductTemplates.Handlers
{
    public class DeleteProductTemplateHandler : IRequestHandler<DeleteProductTemplateCommand, BaseResponse<bool>>
    {
        private readonly IBaseRepository<ProductTemplate> _productTemplateRepository;
        private readonly IBaseRepository<ProductTemplateProductAttribute> _productTemplateProductAttributeRepository;
        private readonly ILogger<DeleteProductTemplateHandler> _logger;

        public DeleteProductTemplateHandler(
            IBaseRepository<ProductTemplate> productTemplateRepository,
            IBaseRepository<ProductTemplateProductAttribute> productTemplateProductAttributeRepository,
            ILogger<DeleteProductTemplateHandler> logger)
        {
            _productTemplateRepository = productTemplateRepository;
            _productTemplateProductAttributeRepository = productTemplateProductAttributeRepository;
            _logger = logger;
        }

        public async Task<BaseResponse<bool>> Handle(DeleteProductTemplateCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"DeleteProductTemplateHandler: {JsonSerializer.Serialize(request)}");

            var existingTemplate = await _productTemplateRepository
                .GetByIdAsync(Guid.Parse(request.Id));
            if (existingTemplate == null)
            {
                return BaseResponse<bool>.Failure("Product Template does not exist.", statusCode: HttpStatusCode.NotFound);
            }

            // Hard delete all related ProductTemplateProductAttribute records
            var relatedAttributes = await _productTemplateProductAttributeRepository
                .GetAsync(x => x.ProductTemplateId == existingTemplate.Id);
            
            foreach (var attribute in relatedAttributes)
            {
                await _productTemplateProductAttributeRepository.DeleteAsync(attribute);
            }

            existingTemplate.IsDeleted = true;
            existingTemplate.LastModifiedDate = DateTime.UtcNow;
            await _productTemplateRepository.UpdateAsync(existingTemplate);

            return BaseResponse<bool>.Success(true);
        }
    }
}

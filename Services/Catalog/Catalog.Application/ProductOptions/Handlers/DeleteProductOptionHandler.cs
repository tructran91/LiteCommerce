using Catalog.Application.ProductOptions.Commands;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using LiteCommerce.Shared.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace Catalog.Application.ProductOptions.Handlers
{
    public class DeleteProductOptionHandler : IRequestHandler<DeleteProductOptionCommand, BaseResponse<bool>>
    {
        private readonly IBaseRepository<ProductOption> _productOptionRepository;
        private readonly ILogger<UpdateProductOptionHandler> _logger;

        public DeleteProductOptionHandler(IBaseRepository<ProductOption> productOptionRepository, ILogger<UpdateProductOptionHandler> logger)
        {
            _productOptionRepository = productOptionRepository;
            _logger = logger;
        }

        public async Task<BaseResponse<bool>> Handle(DeleteProductOptionCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"DeleteProductOptionHandler: {JsonSerializer.Serialize(request)}");

            var existingProductOption = await _productOptionRepository
                .GetByIdAsync(Guid.Parse(request.Id));
            if (existingProductOption == null)
            {
                return BaseResponse<bool>.Failure("Product Option does not exist.", statusCode: HttpStatusCode.NotFound);
            }

            existingProductOption.IsDeleted = true;
            existingProductOption.LastModifiedDate = DateTime.UtcNow;
            await _productOptionRepository.UpdateAsync(existingProductOption);

            return BaseResponse<bool>.Success(true);
        }
    }
}

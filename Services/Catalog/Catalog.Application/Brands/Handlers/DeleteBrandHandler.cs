using Catalog.Application.Brands.Commands;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using LiteCommerce.Shared.Constants;
using LiteCommerce.Shared.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Catalog.Application.Brands.Handlers
{
    public class DeleteBrandHandler : IRequestHandler<DeleteBrandCommand, BaseResponse<bool>>
    {
        private readonly IBaseRepository<Brand> _brandRepository;
        private readonly IBaseRepository<Product> _productRepository;
        private readonly ILogger<DeleteBrandHandler> _logger;

        public DeleteBrandHandler(IBaseRepository<Brand> brandRepository,
            IBaseRepository<Product> productRepository,
            ILogger<DeleteBrandHandler> logger)
        {
            _brandRepository = brandRepository;
            _productRepository = productRepository;
            _logger = logger;
        }

        public async Task<BaseResponse<bool>> Handle(DeleteBrandCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("DeleteBrandHandler: {BrandId}", request.Id);

            var brandId = Guid.Parse(request.Id);
            var existingBrand = await _brandRepository.GetByIdAsync(brandId);
            if (existingBrand == null)
            {
                return BaseResponse<bool>.Failure("Brand does not exist.", statusCode: HttpStatusCode.NotFound);
            }

            // The command only carries the id; give the activity log a readable name.
            request.EntityDisplayName = existingBrand.Name;

            var productCount = await _productRepository.CountAsync(p => p.BrandId == brandId);
            if (productCount > 0)
            {
                return BaseResponse<bool>.Failure(
                    ErrorMessages.CannotDeleteInUse("brand", existingBrand.Name, productCount, "product", "products"),
                    statusCode: HttpStatusCode.Conflict);
            }

            existingBrand.IsDeleted = true;
            await _brandRepository.UpdateAsync(existingBrand);

            return BaseResponse<bool>.Success(true);
        }
    }
}

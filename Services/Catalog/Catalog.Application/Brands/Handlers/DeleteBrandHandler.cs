using Catalog.Application.Brands.Commands;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using LiteCommerce.Shared.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace Catalog.Application.Brands.Handlers
{
    public class DeleteBrandHandler : IRequestHandler<DeleteBrandCommand, BaseResponse<bool>>
    {
        private readonly IBaseRepository<Brand> _brandRepository;
        private readonly ILogger<DeleteBrandHandler> _logger;

        public DeleteBrandHandler(IBaseRepository<Brand> brandRepository, ILogger<DeleteBrandHandler> logger)
        {
            _brandRepository = brandRepository;
            _logger = logger;
        }

        public async Task<BaseResponse<bool>> Handle(DeleteBrandCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"DeleteBrandHandler: {JsonSerializer.Serialize(request)}");

            var existingBrand = await _brandRepository
                .GetByIdAsync(Guid.Parse(request.Id));
            if (existingBrand == null)
            {
                return BaseResponse<bool>.Failure("Brand does not exist.", statusCode: HttpStatusCode.NotFound);
            }

            existingBrand.IsDeleted = true;
            existingBrand.LastModifiedDate = DateTime.UtcNow;
            await _brandRepository.UpdateAsync(existingBrand);

            return BaseResponse<bool>.Success(true);
        }
    }
}

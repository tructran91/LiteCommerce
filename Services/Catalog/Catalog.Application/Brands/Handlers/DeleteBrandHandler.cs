using AutoMapper;
using Catalog.Application.Brands.Commands;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using LiteCommerce.Shared.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace Catalog.Application.Brands.Handlers
{
    public class DeleteBrandHandler : IRequestHandler<DeleteBrandCommand, BaseResponse<BrandResponse>>
    {
        private readonly IBaseRepository<Brand> _brandRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateBrandHandler> _logger;

        public DeleteBrandHandler(IBaseRepository<Brand> brandRepository, IMapper mapper, ILogger<UpdateBrandHandler> logger)
        {
            _brandRepository = brandRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<BrandResponse>> Handle(DeleteBrandCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"DeleteBrandHandler: {JsonSerializer.Serialize(request)}");

            var existingBrand = await _brandRepository
                .GetByIdAsync(Guid.Parse(request.Id));
            if (existingBrand == null)
            {
                return BaseResponse<BrandResponse>.Failure("Brand does not exist.", statusCode: HttpStatusCode.NotFound);
            }

            existingBrand.IsDeleted = true;
            existingBrand.LastModifiedDate = DateTime.UtcNow;
            await _brandRepository.UpdateAsync(existingBrand);

            var response = _mapper.Map<BrandResponse>(existingBrand);

            return BaseResponse<BrandResponse>.Success(response);
        }
    }
}

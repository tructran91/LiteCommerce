using AutoMapper;
using Catalog.Application.Brands.Commands;
using Catalog.Application.Extensions;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Catalog.Application.Brands.Handlers
{
    public class UpdateBrandHandler : IRequestHandler<UpdateBrandCommand, BaseResponse<BrandResponse>>
    {
        private readonly IBaseRepository<Brand> _brandRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateBrandHandler> _logger;

        public UpdateBrandHandler(IBaseRepository<Brand> brandRepository, IMapper mapper, ILogger<UpdateBrandHandler> logger)
        {
            _brandRepository = brandRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<BrandResponse>> Handle(UpdateBrandCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"UpdateBrandHandler: {JsonSerializer.Serialize(request.Payload)}");

            var existingBrand = await _brandRepository.GetByIdAsync(Guid.Parse(request.Payload.Id));
            if (existingBrand == null)
            {
                return BaseResponse<BrandResponse>.Failure("Brand does not exist.", null);
            }

            _mapper.Map(request.Payload, existingBrand);
            existingBrand.Slug = existingBrand.Name.Slugify();

            await _brandRepository.UpdateAsync(existingBrand);

            var response = _mapper.Map<BrandResponse>(existingBrand);

            return BaseResponse<BrandResponse>.Success(response);
        }
    }
}

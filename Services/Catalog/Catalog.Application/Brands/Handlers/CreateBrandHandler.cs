using AutoMapper;
using Catalog.Application.Brands.Commands;
using Catalog.Application.Extensions;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Brands.Handlers
{
    public class CreateBrandHandler : IRequestHandler<CreateBrandCommand, BaseResponse<BrandResponse>>
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IMapper _mapper;

        public CreateBrandHandler(IBrandRepository brandRepository, IMapper mapper)
        {
            _brandRepository = brandRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse<BrandResponse>> Handle(CreateBrandCommand request, CancellationToken cancellationToken)
        {
            var isExistingBrand = await _brandRepository.AnyAsync(t => t.Name.ToLower() == request.Payload.Name.ToLower());
            if (isExistingBrand)
            {
                return BaseResponse<BrandResponse>.Failure("Brand already exists.", null);
            }

            var brand = _mapper.Map<Brand>(request.Payload);
            brand.Slug = brand.Name.Slugify();

            var createdBrand = await _brandRepository.AddAsync(brand);
            var response = _mapper.Map<BrandResponse>(createdBrand);

            return BaseResponse<BrandResponse>.Success(response);
        }
    }
}

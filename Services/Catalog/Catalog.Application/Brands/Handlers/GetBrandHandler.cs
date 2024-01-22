using AutoMapper;
using Catalog.Application.Brands.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Brands.Handlers
{
    public class GetBrandHandler : IRequestHandler<GetBrandQuery, BaseResponse<BrandResponse>>
    {
        private readonly IRepository<Brand> _brandRepository;
        private readonly IMapper _mapper;

        public GetBrandHandler(IRepository<Brand> brandRepository, IMapper mapper)
        {
            _brandRepository = brandRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse<BrandResponse>> Handle(GetBrandQuery request, CancellationToken cancellationToken)
        {
            var brand = await _brandRepository.GetByIdAsync(request.Id);
            if (brand is null)
            {
                return BaseResponse<BrandResponse>.Failure("Brand does not exist.", null);
            }

            var brandMapping = _mapper.Map<BrandResponse>(brand);
            return BaseResponse<BrandResponse>.Success(brandMapping);
        }
    }
}

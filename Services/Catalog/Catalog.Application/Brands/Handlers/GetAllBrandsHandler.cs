using AutoMapper;
using Catalog.Application.Brands.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Brands.Handlers
{
    public class GetAllBrandsHandler : IRequestHandler<GetAllBrandsQuery, BaseResponse<List<BrandResponse>>>
    {
        private readonly IRepository<Brand> _brandRepository;
        private readonly IMapper _mapper;

        public GetAllBrandsHandler(IRepository<Brand> brandRepository, IMapper mapper)
        {
            _brandRepository = brandRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse<List<BrandResponse>>> Handle(GetAllBrandsQuery request, CancellationToken cancellationToken)
        {
            var brands = await _brandRepository.GetAllAsync();
            var brandsMapping = _mapper.Map<List<BrandResponse>>(brands);
            return BaseResponse<List<BrandResponse>>.Success(brandsMapping);
        }
    }
}

using AutoMapper;
using Catalog.Application.Brands.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Brands.Handlers
{
    public class GetBrandHandler : IRequestHandler<GetBrandQuery, BaseResponse<BrandResponse>>
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IMapper _mapper;

        public GetBrandHandler(IBrandRepository brandRepository, IMapper mapper)
        {
            _brandRepository = brandRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse<BrandResponse>> Handle(GetBrandQuery request, CancellationToken cancellationToken)
        {
            var brand = await _brandRepository.GetByIdAsync(request.Id);
            var brandMapping = _mapper.Map<BrandResponse>(brand);
            return BaseResponse<BrandResponse>.Success(brandMapping);
        }
    }
}

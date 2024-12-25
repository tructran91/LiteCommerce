using AutoMapper;
using Catalog.Application.Brands.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using MediatR;
using System.Net;

namespace Catalog.Application.Brands.Handlers
{
    public class GetBrandHandler : IRequestHandler<GetBrandQuery, BaseResponse<BrandResponse>>
    {
        private readonly IBaseRepository<Brand> _brandRepository;
        private readonly IMapper _mapper;

        public GetBrandHandler(IBaseRepository<Brand> brandRepository, IMapper mapper)
        {
            _brandRepository = brandRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse<BrandResponse>> Handle(GetBrandQuery request, CancellationToken cancellationToken)
        {
            var brand = await _brandRepository.GetByIdAsync(Guid.Parse(request.Id));
            if (brand is null)
            {
                return BaseResponse<BrandResponse>.Failure("Brand does not exist.", statusCode: HttpStatusCode.NotFound);
            }

            var brandMapping = _mapper.Map<BrandResponse>(brand);
            return BaseResponse<BrandResponse>.Success(brandMapping);
        }
    }
}

using AutoMapper;
using Catalog.Application.Brands.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using LiteCommerce.Shared.Models;
using MediatR;

namespace Catalog.Application.Brands.Handlers
{
    public class GetAllBrandsHandler : IRequestHandler<GetAllBrandsQuery, BaseResponse<List<BrandResponse>>>
    {
        private readonly IBaseRepository<Brand> _brandRepository;
        private readonly IMapper _mapper;

        public GetAllBrandsHandler(IBaseRepository<Brand> brandRepository, IMapper mapper)
        {
            _brandRepository = brandRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse<List<BrandResponse>>> Handle(GetAllBrandsQuery request, CancellationToken cancellationToken)
        {
            var brands = await _brandRepository.GetAsync(
                predicate: t => !t.IsDeleted,
                orderBy: x => x.OrderBy(y=>y.Name),
                pageNumber: request.CurrentPage,
                pageSize: request.PageSize);
            var totalRecords = await _brandRepository.CountAsync(t => !t.IsDeleted);
            var brandResponses = _mapper.Map<List<BrandResponse>>(brands);
            var response = BaseResponse<List<BrandResponse>>.Success(brandResponses);
            response.Pagination = new Pagination(totalRecords, request.CurrentPage, request.PageSize);

            return response;
        }
    }
}

using AutoMapper;
using Catalog.Application.ProductAttributeGroups.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using LiteCommerce.Shared.Models;
using MediatR;

namespace Catalog.Application.ProductAttributeGroups.Handlers
{
    public class GetAllProductAttributeGroupsHandler : IRequestHandler<GetAllProductAttributeGroupsQuery, BaseResponse<List<ProductAttributeGroupResponse>>>
    {
        private readonly IBaseRepository<ProductAttributeGroup> _productAttributeGroupRepository;
        private readonly IMapper _mapper;

        public GetAllProductAttributeGroupsHandler(IBaseRepository<ProductAttributeGroup> productAttributeGroupRepository, IMapper mapper)
        {
            _productAttributeGroupRepository = productAttributeGroupRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse<List<ProductAttributeGroupResponse>>> Handle(GetAllProductAttributeGroupsQuery request, CancellationToken cancellationToken)
        {
            var ProductAttributeGroups = await _productAttributeGroupRepository.GetAsync(
                predicate: t => !t.IsDeleted,
                orderBy: x => x.OrderBy(y=>y.Name),
                pageNumber: request.CurrentPage,
                pageSize: request.PageSize);
            var totalRecords = await _productAttributeGroupRepository.CountAsync(t => !t.IsDeleted);
            var ProductAttributeGroupResponses = _mapper.Map<List<ProductAttributeGroupResponse>>(ProductAttributeGroups);
            var response = BaseResponse<List<ProductAttributeGroupResponse>>.Success(ProductAttributeGroupResponses);
            response.Pagination = new Pagination(totalRecords, request.CurrentPage, request.PageSize);

            return response;
        }
    }
}

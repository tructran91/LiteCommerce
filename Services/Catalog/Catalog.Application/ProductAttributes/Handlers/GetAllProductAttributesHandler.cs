using AutoMapper;
using Catalog.Application.ProductAttributes.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using LiteCommerce.Shared.Models;
using MediatR;

namespace Catalog.Application.ProductAttributes.Handlers
{
    public class GetAllProductAttributesHandler : IRequestHandler<GetAllProductAttributesQuery, BaseResponse<List<ProductAttributeResponse>>>
    {
        private readonly IBaseRepository<ProductAttribute> _productAttributeRepository;
        private readonly IMapper _mapper;

        public GetAllProductAttributesHandler(IBaseRepository<ProductAttribute> productAttributeRepository, IMapper mapper)
        {
            _productAttributeRepository = productAttributeRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse<List<ProductAttributeResponse>>> Handle(GetAllProductAttributesQuery request, CancellationToken cancellationToken)
        {
            var ProductAttributes = await _productAttributeRepository.GetAsync(
                predicate: t => !t.IsDeleted,
                orderBy: x => x.OrderBy(y=>y.Name),
                pageNumber: request.CurrentPage,
                pageSize: request.PageSize);
            var totalRecords = await _productAttributeRepository.CountAsync(t => !t.IsDeleted);
            var productAttributeResponses = _mapper.Map<List<ProductAttributeResponse>>(ProductAttributes);
            var response = BaseResponse<List<ProductAttributeResponse>>.Success(productAttributeResponses);
            response.Pagination = new Pagination(totalRecords, request.CurrentPage, request.PageSize);

            return response;
        }
    }
}

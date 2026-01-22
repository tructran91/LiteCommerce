using AutoMapper;
using Catalog.Application.ProductOptions.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using LiteCommerce.Shared.Models;
using MediatR;

namespace Catalog.Application.ProductOptions.Handlers
{
    public class GetAllProductOptionsHandler : IRequestHandler<GetAllProductOptionsQuery, BaseResponse<List<ProductOptionResponse>>>
    {
        private readonly IBaseRepository<ProductOption> _productOptionRepository;
        private readonly IMapper _mapper;

        public GetAllProductOptionsHandler(IBaseRepository<ProductOption> productOptionRepository, IMapper mapper)
        {
            _productOptionRepository = productOptionRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse<List<ProductOptionResponse>>> Handle(GetAllProductOptionsQuery request, CancellationToken cancellationToken)
        {
            var productOptions = await _productOptionRepository.GetAsync(
                predicate: t => !t.IsDeleted,
                orderBy: x => x.OrderBy(y=>y.Name),
                pageNumber: request.CurrentPage,
                pageSize: request.PageSize);
            var totalRecords = await _productOptionRepository.CountAsync(t => !t.IsDeleted);
            var productOptionResponses = _mapper.Map<List<ProductOptionResponse>>(productOptions);
            var response = BaseResponse<List<ProductOptionResponse>>.Success(productOptionResponses);
            response.Pagination = new Pagination(totalRecords, request.CurrentPage, request.PageSize);

            return response;
        }
    }
}

using AutoMapper;
using Catalog.Application.ProductTemplates.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using LiteCommerce.Shared.Models;
using MediatR;

namespace Catalog.Application.ProductTemplates.Handlers
{
    public class GetAllProductTemplatesHandler : IRequestHandler<GetAllProductTemplatesQuery, BaseResponse<List<ProductTemplateResponse>>>
    {
        private readonly IBaseRepository<ProductTemplate> _productTemplateRepository;
        private readonly IMapper _mapper;

        public GetAllProductTemplatesHandler(IBaseRepository<ProductTemplate> productTemplateRepository, IMapper mapper)
        {
            _productTemplateRepository = productTemplateRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse<List<ProductTemplateResponse>>> Handle(GetAllProductTemplatesQuery request, CancellationToken cancellationToken)
        {
            var templates = await _productTemplateRepository.GetAsync(
                predicate: t => !t.IsDeleted,
                orderBy: x => x.OrderBy(y=>y.Name),
                pageNumber: request.CurrentPage,
                pageSize: request.PageSize);
            var totalRecords = await _productTemplateRepository.CountAsync(t => !t.IsDeleted);
            var templateResponses = _mapper.Map<List<ProductTemplateResponse>>(templates);
            var response = BaseResponse<List<ProductTemplateResponse>>.Success(templateResponses);
            response.Pagination = new Pagination(totalRecords, request.CurrentPage, request.PageSize);

            return response;
        }
    }
}

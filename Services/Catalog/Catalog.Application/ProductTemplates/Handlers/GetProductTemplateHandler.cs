using AutoMapper;
using Catalog.Application.ProductTemplates.Queries;
using Catalog.Application.Responses;
using Catalog.Application.ViewModels;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using LiteCommerce.Shared.Models;
using MediatR;
using System.Net;

namespace Catalog.Application.ProductTemplates.Handlers
{
    public class GetProductTemplateHandler : IRequestHandler<GetProductTemplateQuery, BaseResponse<ProductTemplateResponse>>
    {
        private readonly IBaseRepository<ProductTemplate> _productTemplateRepository;
        private readonly IMapper _mapper;

        public GetProductTemplateHandler(IBaseRepository<ProductTemplate> productTemplateRepository, IMapper mapper)
        {
            _productTemplateRepository = productTemplateRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse<ProductTemplateResponse>> Handle(GetProductTemplateQuery request, CancellationToken cancellationToken)
        {
            var templates = await _productTemplateRepository.GetAsync(
                predicate: t => t.Id == Guid.Parse(request.Id) && !t.IsDeleted,
                includeString: "ProductAttributes.ProductAttribute");

            var template = templates.FirstOrDefault();
            if (template is null)
            {
                return BaseResponse<ProductTemplateResponse>.Failure("Product Template does not exist.", statusCode: HttpStatusCode.NotFound);
            }

            var templateMapping = _mapper.Map<ProductTemplateResponse>(template);
            templateMapping.ProductAttributes = template.ProductAttributes
                .Select(pa => _mapper.Map<ProductAttributeOverviewViewModel>(pa.ProductAttribute))
                .ToList();

            return BaseResponse<ProductTemplateResponse>.Success(templateMapping);
        }
    }
}

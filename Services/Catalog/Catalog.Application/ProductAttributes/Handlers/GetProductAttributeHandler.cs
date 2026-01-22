using AutoMapper;
using Catalog.Application.ProductAttributes.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using LiteCommerce.Shared.Models;
using MediatR;
using System.Net;

namespace Catalog.Application.ProductAttributes.Handlers
{
    public class GetProductAttributeHandler : IRequestHandler<GetProductAttributeQuery, BaseResponse<ProductAttributeResponse>>
    {
        private readonly IBaseRepository<ProductAttribute> _productAttributeRepository;
        private readonly IMapper _mapper;

        public GetProductAttributeHandler(IBaseRepository<ProductAttribute> productAttributeRepository, IMapper mapper)
        {
            _productAttributeRepository = productAttributeRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse<ProductAttributeResponse>> Handle(GetProductAttributeQuery request, CancellationToken cancellationToken)
        {
            var ProductAttribute = await _productAttributeRepository.GetByIdAsync(Guid.Parse(request.Id));
            if (ProductAttribute is null)
            {
                return BaseResponse<ProductAttributeResponse>.Failure("Product Attribute does not exist.", statusCode: HttpStatusCode.NotFound);
            }

            var ProductAttributeMapping = _mapper.Map<ProductAttributeResponse>(ProductAttribute);
            return BaseResponse<ProductAttributeResponse>.Success(ProductAttributeMapping);
        }
    }
}

using AutoMapper;
using Catalog.Application.ProductAttributeGroups.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using LiteCommerce.Shared.Models;
using MediatR;
using System.Net;

namespace Catalog.Application.ProductAttributeGroups.Handlers
{
    public class GetProductAttributeGroupHandler : IRequestHandler<GetProductAttributeGroupQuery, BaseResponse<ProductAttributeGroupResponse>>
    {
        private readonly IBaseRepository<ProductAttributeGroup> _productAttributeGroupRepository;
        private readonly IMapper _mapper;

        public GetProductAttributeGroupHandler(IBaseRepository<ProductAttributeGroup> productAttributeGroupRepository, IMapper mapper)
        {
            _productAttributeGroupRepository = productAttributeGroupRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse<ProductAttributeGroupResponse>> Handle(GetProductAttributeGroupQuery request, CancellationToken cancellationToken)
        {
            var ProductAttributeGroup = await _productAttributeGroupRepository.GetByIdAsync(Guid.Parse(request.Id));
            if (ProductAttributeGroup is null)
            {
                return BaseResponse<ProductAttributeGroupResponse>.Failure("Product Attribute Group does not exist.", statusCode: HttpStatusCode.NotFound);
            }

            var ProductAttributeGroupMapping = _mapper.Map<ProductAttributeGroupResponse>(ProductAttributeGroup);
            return BaseResponse<ProductAttributeGroupResponse>.Success(ProductAttributeGroupMapping);
        }
    }
}

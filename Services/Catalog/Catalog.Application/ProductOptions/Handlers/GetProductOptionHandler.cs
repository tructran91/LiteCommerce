using AutoMapper;
using Catalog.Application.ProductOptions.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using LiteCommerce.Shared.Models;
using MediatR;
using System.Net;

namespace Catalog.Application.ProductOptions.Handlers
{
    public class GetProductOptionHandler : IRequestHandler<GetProductOptionQuery, BaseResponse<ProductOptionResponse>>
    {
        private readonly IBaseRepository<ProductOption> _productOptionRepository;
        private readonly IMapper _mapper;

        public GetProductOptionHandler(IBaseRepository<ProductOption> productOptionRepository, IMapper mapper)
        {
            _productOptionRepository = productOptionRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse<ProductOptionResponse>> Handle(GetProductOptionQuery request, CancellationToken cancellationToken)
        {
            var productOption = await _productOptionRepository.GetByIdAsync(Guid.Parse(request.Id));
            if (productOption is null)
            {
                return BaseResponse<ProductOptionResponse>.Failure("Product Option does not exist.", statusCode: HttpStatusCode.NotFound);
            }

            var productOptionMapping = _mapper.Map<ProductOptionResponse>(productOption);
            return BaseResponse<ProductOptionResponse>.Success(productOptionMapping);
        }
    }
}

using AutoMapper;
using Catalog.Application.Brands.Commands;
using Catalog.Application.Extensions;
using Catalog.Application.Responses;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Brands.Handlers
{
    public class UpdateBrandHandler : IRequestHandler<UpdateBrandCommand, BaseResponse<BrandResponse>>
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IMapper _mapper;

        public UpdateBrandHandler(IBrandRepository brandRepository, IMapper mapper)
        {
            _brandRepository = brandRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse<BrandResponse>> Handle(UpdateBrandCommand request, CancellationToken cancellationToken)
        {
            var existingBrand = await _brandRepository.GetByIdAsync(Guid.Parse(request.Payload.Id));
            if (existingBrand == null)
            {
                return BaseResponse<BrandResponse>.Failure("Brand does not exist.", null);
            }

            _mapper.Map(request.Payload, existingBrand);
            existingBrand.Slug = existingBrand.Name.Slugify();

            await _brandRepository.UpdateAsync(existingBrand);

            var response = _mapper.Map<BrandResponse>(existingBrand);

            return BaseResponse<BrandResponse>.Success(response);
        }
    }
}

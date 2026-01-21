using AutoMapper;
using Catalog.Application.Brands.Queries;
using Catalog.Application.Responses;
using Catalog.Application.Services;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using LiteCommerce.Shared.Models;
using MediatR;
using System.Net;

namespace Catalog.Application.Categories.Handlers
{
    public class GetCategoryHandler : IRequestHandler<GetCategoryQuery, BaseResponse<CategoryResponse>>
    {
        private readonly IBaseRepository<Category> _categoryRepository;
        private readonly IMediaService _mediaService;
        private readonly IMapper _mapper;

        public GetCategoryHandler(
            IBaseRepository<Category> categoryRepository,
            IMediaService mediaService,
            IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mediaService = mediaService;
            _mapper = mapper;
        }

        public async Task<BaseResponse<CategoryResponse>> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
        {
            var existingCategory = await _categoryRepository.GetAsync(
                predicate: t => t.Id == Guid.Parse(request.Id) && !t.IsDeleted,
                includeString: "ThumbnailImage");

            var category = existingCategory.FirstOrDefault();
            if (category is null)
            {
                return BaseResponse<CategoryResponse>.Failure("Category does not exist.", statusCode: HttpStatusCode.NotFound);
            }

            var categoryMapping = _mapper.Map<CategoryResponse>(category);
            categoryMapping.ThumbnailImageUrl = _mediaService.GetMediaUrl(category.ThumbnailImage?.FileName);

            return BaseResponse<CategoryResponse>.Success(categoryMapping);
        }
    }
}

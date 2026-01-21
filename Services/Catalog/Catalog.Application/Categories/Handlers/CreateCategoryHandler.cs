using AutoMapper;
using Catalog.Application.Categories.Commands;
using Catalog.Application.Extensions;
using Catalog.Application.Responses;
using Catalog.Application.Services;
using Catalog.Core.Entities;
using Catalog.Core.Enums;
using Catalog.Core.Repositories;
using LiteCommerce.Shared.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace Catalog.Application.Categories.Handlers
{
    public class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, BaseResponse<CategoryResponse>>
    {
        private readonly IBaseRepository<Category> _categoryRepository;
        private readonly IMediaService _mediaService;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateCategoryHandler> _logger;

        public CreateCategoryHandler(
            IBaseRepository<Category> categoryRepository,
            IMediaService mediaService,
            IMapper mapper,
            ILogger<CreateCategoryHandler> logger)
        {
            _categoryRepository = categoryRepository;
            _mediaService = mediaService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<CategoryResponse>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var payload = request.Payload;
            _logger.LogInformation($"CreateCategoryHandler: {JsonSerializer.Serialize(payload)}");

            var isExistingCategory = await _categoryRepository.AnyAsync(t => t.Name.ToLower() == payload.Name.ToLower());
            if (isExistingCategory)
            {
                return BaseResponse<CategoryResponse>.Failure("Category already exists.", statusCode: HttpStatusCode.Conflict);
            }

            if (!string.IsNullOrEmpty(payload.ParentId))
            {
                var isExistingParentCategory = await _categoryRepository.GetByIdAsync(Guid.Parse(payload.ParentId));
                if (isExistingParentCategory is null)
                {
                    return BaseResponse<CategoryResponse>.Failure("Parent Category does not exist.", statusCode: HttpStatusCode.NotFound);
                }
            }

            var category = _mapper.Map<Category>(payload);
            category.Slug = category.Name.Slugify();

            if (payload.ThumbnailImage != null)
            {
                var fileName = await _mediaService.SaveMediaAsync(payload.ThumbnailImage);
                category.ThumbnailImage = new Media
                {
                    FileName = fileName,
                    MediaType = MediaType.Image,
                    CreatedDate = DateTime.UtcNow
                };
            }

            var createdCategory = await _categoryRepository.AddAsync(category);
            var response = _mapper.Map<CategoryResponse>(createdCategory);
            response.ThumbnailImageUrl = _mediaService.GetMediaUrl(createdCategory.ThumbnailImage?.FileName);

            return BaseResponse<CategoryResponse>.Success(response);
        }
    }
}

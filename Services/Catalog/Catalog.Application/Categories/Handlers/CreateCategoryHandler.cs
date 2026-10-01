using AutoMapper;
using Catalog.Application.Categories.Commands;
using Catalog.Application.Extensions;
using Catalog.Application.Responses;
using Catalog.Application.Services;
using Catalog.Core.Entities;
using Catalog.Core.Enums;
using Catalog.Core.Repositories;
using LiteCommerce.Shared.Constants;
using LiteCommerce.Shared.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Net;

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
            _logger.LogInformation("CreateCategoryHandler: {CategoryName} {ParentId}", payload.Name, payload.ParentId);

            Guid? parentId = string.IsNullOrEmpty(payload.ParentId) ? null : Guid.Parse(payload.ParentId);

            if (parentId.HasValue)
            {
                var isExistingParentCategory = await _categoryRepository.GetByIdAsync(parentId.Value);
                if (isExistingParentCategory is null)
                {
                    return BaseResponse<CategoryResponse>.Failure("Parent category does not exist.", statusCode: HttpStatusCode.NotFound);
                }
            }

            var isExistingCategory = await _categoryRepository
                .AnyAsync(t => t.ParentId == parentId && t.Name.ToLower() == payload.Name.ToLower());
            if (isExistingCategory)
            {
                return BaseResponse<CategoryResponse>.Failure("Category already exists.", statusCode: HttpStatusCode.Conflict);
            }

            var category = _mapper.Map<Category>(payload);
            category.Slug = category.Name.Slugify();

            string? newFileName = null;
            if (payload.ThumbnailImage != null)
            {
                newFileName = await _mediaService.SaveMediaAsync(payload.ThumbnailImage, StorageFolder.Category);
                category.ThumbnailImage = new Media
                {
                    FileName = newFileName,
                    MediaType = MediaType.Image
                };
            }

            Category createdCategory;
            try
            {
                createdCategory = await _categoryRepository.AddAsync(category);
            }
            catch
            {
                if (newFileName != null)
                    await _mediaService.DeleteMediaAsync(newFileName, StorageFolder.Category);
                throw;
            }

            var response = _mapper.Map<CategoryResponse>(createdCategory);

            return BaseResponse<CategoryResponse>.Success(response, statusCode: HttpStatusCode.Created);
        }
    }
}

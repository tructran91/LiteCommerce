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
    public class UpdateCategoryHandler : IRequestHandler<UpdateCategoryCommand, BaseResponse<CategoryResponse>>
    {
        private readonly IBaseRepository<Category> _categoryRepository;
        private readonly IMediaService _mediaService;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateCategoryHandler> _logger;

        public UpdateCategoryHandler(
            IBaseRepository<Category> categoryRepository,
            IMediaService mediaService,
            IMapper mapper,
            ILogger<UpdateCategoryHandler> logger)
        {
            _categoryRepository = categoryRepository;
            _mediaService = mediaService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<CategoryResponse>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var payload = request.Payload;
            _logger.LogInformation("UpdateCategoryHandler: {CategoryId} {CategoryName}", request.Id, payload.Name);

            var categoryId = Guid.Parse(request.Id);
            var existingCategory = await _categoryRepository.GetAsync(
                predicate: t => t.Id == categoryId,
                includeString: "ThumbnailImage",
                disableTracking: false);

            var updatedCategory = existingCategory.FirstOrDefault();
            if (updatedCategory is null)
            {
                return BaseResponse<CategoryResponse>.Failure("Category does not exist.", statusCode: HttpStatusCode.NotFound);
            }

            Guid? parentId = string.IsNullOrEmpty(payload.ParentId) ? null : Guid.Parse(payload.ParentId);

            var isDuplicateName = await _categoryRepository
                .AnyAsync(t => t.ParentId == parentId && t.Name.ToLower() == payload.Name.ToLower() && t.Id != updatedCategory.Id);
            if (isDuplicateName)
            {
                return BaseResponse<CategoryResponse>.Failure("Category already exists.", statusCode: HttpStatusCode.Conflict);
            }

            _mapper.Map(payload, updatedCategory);
            updatedCategory.Slug = updatedCategory.Name.Slugify();

            string? newFileName = null;
            string? oldFileName = null;

            if (payload.ThumbnailImage != null)
            {
                newFileName = await _mediaService.SaveMediaAsync(payload.ThumbnailImage, StorageFolder.Category);
                if (updatedCategory.ThumbnailImage != null)
                {
                    oldFileName = updatedCategory.ThumbnailImage.FileName;
                    updatedCategory.ThumbnailImage.FileName = newFileName;
                }
                else
                {
                    updatedCategory.ThumbnailImage = new Media
                    {
                        FileName = newFileName,
                        MediaType = MediaType.Image
                    };
                }
            }

            try
            {
                await _categoryRepository.UpdateAsync(updatedCategory);
            }
            catch
            {
                if (newFileName != null)
                    await _mediaService.DeleteMediaAsync(newFileName, StorageFolder.Category);
                throw;
            }

            // Delete the old file only after the DB no longer references it.
            if (oldFileName != null)
                await _mediaService.DeleteMediaAsync(oldFileName, StorageFolder.Category);

            var response = _mapper.Map<CategoryResponse>(updatedCategory);

            return BaseResponse<CategoryResponse>.Success(response);
        }
    }
}

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
            _logger.LogInformation($"UpdateCategoryHandler: {JsonSerializer.Serialize(payload)}");

            var existingCategory = await _categoryRepository.GetAsync(
                predicate: t => t.Id == Guid.Parse(payload.Id) && !t.IsDeleted,
                includeString: "ThumbnailImage",
                disableTracking: false);

            var updatedCategory = existingCategory.FirstOrDefault();
            if (updatedCategory is null)
            {
                return BaseResponse<CategoryResponse>.Failure("Category does not exist.", statusCode: HttpStatusCode.NotFound);
            }

            _mapper.Map(payload, updatedCategory);
            updatedCategory.Slug = updatedCategory.Name.Slugify();

            if (payload.ThumbnailImage != null)
            {
                var fileName = await _mediaService.SaveMediaAsync(payload.ThumbnailImage);
                if (updatedCategory.ThumbnailImage != null)
                {
                    await _mediaService.DeleteMediaAsync(updatedCategory.ThumbnailImage.FileName);

                    updatedCategory.ThumbnailImage.FileName = fileName;
                    updatedCategory.ThumbnailImage.LastModifiedDate = DateTime.UtcNow;
                }
                else
                {
                    updatedCategory.ThumbnailImage = new Media
                    {
                        FileName = fileName,
                        MediaType = MediaType.Image,
                        CreatedDate = DateTime.UtcNow
                    };
                }
            }

            await _categoryRepository.UpdateAsync(updatedCategory);

            var response = _mapper.Map<CategoryResponse>(updatedCategory);

            return BaseResponse<CategoryResponse>.Success(response);
        }
    }
}

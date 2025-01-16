using AutoMapper;
using Catalog.Application.Categories.Commands;
using Catalog.Application.Extensions;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
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
        private readonly IMapper _mapper;
        private readonly ILogger<CreateCategoryHandler> _logger;

        public CreateCategoryHandler(IBaseRepository<Category> categoryRepository, IMapper mapper, ILogger<CreateCategoryHandler> logger)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<CategoryResponse>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"CreateCategoryHandler: {JsonSerializer.Serialize(request.Payload)}");

            var isExistingCategory = await _categoryRepository.AnyAsync(t => t.Name.ToLower() == request.Payload.Name.ToLower());
            if (isExistingCategory)
            {
                return BaseResponse<CategoryResponse>.Failure("Category already exists.", statusCode: HttpStatusCode.Conflict);
            }

            if (!string.IsNullOrEmpty(request.Payload.ParentId))
            {
                var isExistingParentCategory = await _categoryRepository.GetByIdAsync(Guid.Parse(request.Payload.ParentId));
                if (isExistingParentCategory is null)
                {
                    return BaseResponse<CategoryResponse>.Failure("Parent Category does not exist.", statusCode: HttpStatusCode.NotFound);
                }
            }

            var category = _mapper.Map<Category>(request.Payload);
            category.Slug = category.Name.Slugify();

            var createdCategory = await _categoryRepository.AddAsync(category);
            var response = _mapper.Map<CategoryResponse>(createdCategory);

            return BaseResponse<CategoryResponse>.Success(response);
        }
    }
}

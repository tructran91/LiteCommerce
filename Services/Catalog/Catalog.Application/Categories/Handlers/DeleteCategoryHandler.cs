using AutoMapper;
using Catalog.Application.Categories.Commands;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using LiteCommerce.Shared.Constants;
using LiteCommerce.Shared.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace Catalog.Application.Categories.Handlers
{
    public class DeleteCategoryHandler : IRequestHandler<DeleteCategoryCommand, BaseResponse<bool>>
    {
        private readonly IBaseRepository<Category> _categoryRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<DeleteCategoryHandler> _logger;

        public DeleteCategoryHandler(IBaseRepository<Category> categoryRepository, IMapper mapper, ILogger<DeleteCategoryHandler> logger)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<bool>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"DeleteCategoryHandler: {JsonSerializer.Serialize(request)}");

            var existingCategories = await _categoryRepository.GetAsync(
                predicate: t => t.Id == Guid.Parse(request.Id) && !t.IsDeleted,
                includeString: "SubCategories");

            var existingCategory = existingCategories.FirstOrDefault();
            if (existingCategory == null)
            {
                return BaseResponse<bool>.Failure("Category does not exist.", statusCode: HttpStatusCode.NotFound);
            }

            // Check if category has active subcategories
            if (existingCategory.SubCategories?.Any(sc => !sc.IsDeleted) == true)
            {
                return BaseResponse<bool>.Failure("Cannot delete category. Please delete all subcategories first.", statusCode: HttpStatusCode.BadRequest);
            }

            existingCategory.IsDeleted = true;
            existingCategory.LastModifiedDate = DateTime.UtcNow;
            await _categoryRepository.UpdateAsync(existingCategory);

            return BaseResponse<bool>.Success(true);
        }
    }
}

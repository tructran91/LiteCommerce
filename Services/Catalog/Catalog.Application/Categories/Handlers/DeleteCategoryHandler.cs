using Catalog.Application.Categories.Commands;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using LiteCommerce.Shared.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Catalog.Application.Categories.Handlers
{
    public class DeleteCategoryHandler : IRequestHandler<DeleteCategoryCommand, BaseResponse<bool>>
    {
        private readonly IBaseRepository<Category> _categoryRepository;
        private readonly IBaseRepository<ProductCategory> _productCategoryRepository;
        private readonly ILogger<DeleteCategoryHandler> _logger;

        public DeleteCategoryHandler(IBaseRepository<Category> categoryRepository,
            IBaseRepository<ProductCategory> productCategoryRepository,
            ILogger<DeleteCategoryHandler> logger)
        {
            _categoryRepository = categoryRepository;
            _productCategoryRepository = productCategoryRepository;
            _logger = logger;
        }

        public async Task<BaseResponse<bool>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("DeleteCategoryHandler: {CategoryId}", request.Id);

            var categoryId = Guid.Parse(request.Id);
            var existingCategories = await _categoryRepository.GetAsync(
                predicate: t => t.Id == categoryId,
                includeString: "SubCategories");

            var existingCategory = existingCategories.FirstOrDefault();
            if (existingCategory == null)
            {
                return BaseResponse<bool>.Failure("Category does not exist.", statusCode: HttpStatusCode.NotFound);
            }

            // Check if category has active subcategories
            if (existingCategory.SubCategories?.Any(sc => !sc.IsDeleted) == true)
            {
                return BaseResponse<bool>.Failure("Cannot delete category. Please delete all subcategories first.", statusCode: HttpStatusCode.Conflict);
            }

            var productCount = await _productCategoryRepository.CountAsync(pc => pc.CategoryId == categoryId && !pc.Product.IsDeleted);
            if (productCount > 0)
            {
                return BaseResponse<bool>.Failure(
                    $"Cannot delete category. It is assigned to {productCount} product(s).",
                    statusCode: HttpStatusCode.Conflict);
            }

            existingCategory.IsDeleted = true;
            await _categoryRepository.UpdateAsync(existingCategory);

            return BaseResponse<bool>.Success(true);
        }
    }
}

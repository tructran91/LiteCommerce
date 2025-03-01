using AutoMapper;
using Catalog.Application.Categories.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using LiteCommerce.Shared.Constants;
using LiteCommerce.Shared.Models;
using MediatR;

namespace Catalog.Application.Categories.Handlers
{
    public class GetAllBasicCategoriesHandler : IRequestHandler<GetAllBasicCategoriesQuery, BaseResponse<List<BasicCategoryResponse>>>
    {
        private readonly IBaseRepository<Category> _categoryRepository;
        private readonly IMapper _mapper;

        public GetAllBasicCategoriesHandler(IBaseRepository<Category> brandRepository, IMapper mapper)
        {
            _categoryRepository = brandRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse<List<BasicCategoryResponse>>> Handle(GetAllBasicCategoriesQuery request, CancellationToken cancellationToken)
        {
            var categories = await _categoryRepository.GetAsync(
                predicate: t => !t.IsDeleted,
                orderBy: x => x.OrderBy(y => y.Name),
                includeString: "ParentCategory",
                pageNumber: 1,
                pageSize: PaginationSetting.MaxPageSize);
            var categoriesMapping = new List<BasicCategoryResponse>();

            foreach (var category in categories)
            {
                var categoryResponse = _mapper.Map<BasicCategoryResponse>(category);

                var parentCategory = category.ParentCategory;
                while (parentCategory != null)
                {
                    categoryResponse.DisplayName = $"{parentCategory.Name} >> {categoryResponse.DisplayName}";
                    parentCategory = parentCategory.ParentCategory;
                }

                categoriesMapping.Add(categoryResponse);
            }

            categoriesMapping = categoriesMapping.OrderBy(t => t.DisplayName).ToList();

            var response = BaseResponse<List<BasicCategoryResponse>>.Success(categoriesMapping);

            return response;
        }
    }
}

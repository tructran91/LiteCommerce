using AutoMapper;
using Catalog.Application.Brands.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using LiteCommerce.Shared.Models;
using MediatR;

namespace Catalog.Application.Categories.Handlers
{
    public class GetAllCategoriesHandler : IRequestHandler<GetAllCategoriesQuery, BaseResponse<List<CategoryResponse>>>
    {
        private readonly IBaseRepository<Category> _categoryRepository;
        private readonly IMapper _mapper;

        public GetAllCategoriesHandler(IBaseRepository<Category> brandRepository, IMapper mapper)
        {
            _categoryRepository = brandRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse<List<CategoryResponse>>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            var categories = await _categoryRepository.GetAsync(
                predicate: t => !t.IsDeleted,
                orderBy: x => x.OrderBy(y => y.Name),
                includeString: "Parent",
                pageNumber: request.CurrentPage,
                pageSize: request.PageSize);
            var totalRecords = await _categoryRepository.CountAsync(t => !t.IsDeleted);
            var categoriesMapping = new List<CategoryResponse>();

            foreach (var category in categories)
            {
                var categoryResponse = _mapper.Map<CategoryResponse>(category);

                var parentCategory = category.Parent;
                while (parentCategory != null)
                {
                    categoryResponse.DisplayName = $"{parentCategory.Name} >> {categoryResponse.DisplayName}";
                    parentCategory = parentCategory.Parent;
                }

                categoriesMapping.Add(categoryResponse);
            }

            categoriesMapping = categoriesMapping.OrderBy(t => t.DisplayName).ToList();

            var response = BaseResponse<List<CategoryResponse>>.Success(categoriesMapping);
            response.Pagination = new Pagination(totalRecords, request.CurrentPage, request.PageSize);

            return response;
        }
    }
}

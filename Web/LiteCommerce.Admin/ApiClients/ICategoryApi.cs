using LiteCommerce.Admin.Models.Business.Category;
using LiteCommerce.Admin.Models.Common;
using Refit;

namespace LiteCommerce.Admin.ApiClients
{
    public interface ICategoryApi
    {
        [Get("/api/admin/category/GetAllCategories")]
        Task<BaseResponse<List<CategoryResponse>>> GetCategoriesAsync(int currentPage, int pageSize);

        [Get("/api/admin/category/GetAllBasicCategories")]
        Task<BaseResponse<List<BasicCategoryResponse>>> GetBasicCategoriesAsync();

        [Get("/api/admin/category/GetCategoryById/{id}")]
        Task<BaseResponse<CategoryFormModel>> GetCategoryAsync(string id);

        [Post("/api/admin/category/CreateCategory")]
        Task<BaseResponse<CategoryResponse>> CreateCategoryAsync([Body] CategoryFormModel Category);

        [Put("/api/admin/category/UpdateCategory")]
        Task<BaseResponse<CategoryResponse>> UpdateCategoryAsync([Body] CategoryFormModel Category);

        [Delete("/api/admin/category/DeleteCategory/{id}")]
        Task<BaseResponse<CategoryResponse>> DeleteCategoryAsync(string id);
    }
}

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
        [Multipart]
        Task<BaseResponse<CategoryResponse>> CreateCategoryAsync(MultipartFormDataContent content);

        [Put("/api/admin/category/UpdateCategory")]
        [Multipart]
        Task<BaseResponse<CategoryResponse>> UpdateCategoryAsync(MultipartFormDataContent content);

        [Delete("/api/admin/category/DeleteCategory/{id}")]
        Task<BaseResponse<bool>> DeleteCategoryAsync(string id);
    }
}

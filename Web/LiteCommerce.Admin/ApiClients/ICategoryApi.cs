using LiteCommerce.Admin.Models.Business;
using LiteCommerce.Admin.Models.Common;
using Refit;

namespace LiteCommerce.Admin.ApiClients
{
    public interface ICategoryApi
    {
        [Get("/api/admin/category/GetAllCategories")]
        Task<BaseResponse<List<Category>>> GetCategoriesAsync(int currentPage, int pageSize);

        [Get("/api/admin/category/GetCategoryById/{id}")]
        Task<BaseResponse<Category>> GetCategoryAsync(string id);

        //[Post("/api/admin/category/CreateCategory")]
        //Task<BaseResponse<Category>> CreateCategoryAsync([Body] Category Category);

        //[Put("/api/admin/category/UpdateCategory")]
        //Task<BaseResponse<Category>> UpdateCategoryAsync([Body] Category Category);

        //[Delete("/api/admin/category/DeleteCategory/{id}")]
        //Task<BaseResponse<Category>> DeleteCategoryAsync(string id);
    }
}

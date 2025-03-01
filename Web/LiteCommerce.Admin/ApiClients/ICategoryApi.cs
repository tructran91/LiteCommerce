using LiteCommerce.Admin.Models.Business.Category;
using LiteCommerce.Admin.Models.Common;
using Refit;

namespace LiteCommerce.Admin.ApiClients
{
    public interface ICategoryApi
    {
        [Get("/api/admin/category/GetAllCategories")]
        Task<BaseResponse<List<CategoryModel>>> GetCategoriesAsync(int currentPage, int pageSize);

        [Get("/api/admin/category/GetAllBasicCategories")]
        Task<BaseResponse<List<BasicCategoryModel>>> GetBasicCategoriesAsync();

        [Get("/api/admin/category/GetCategoryById/{id}")]
        Task<BaseResponse<CategoryModel>> GetCategoryAsync(string id);

        //[Post("/api/admin/category/CreateCategory")]
        //Task<BaseResponse<Category>> CreateCategoryAsync([Body] Category Category);

        //[Put("/api/admin/category/UpdateCategory")]
        //Task<BaseResponse<Category>> UpdateCategoryAsync([Body] Category Category);

        //[Delete("/api/admin/category/DeleteCategory/{id}")]
        //Task<BaseResponse<Category>> DeleteCategoryAsync(string id);
    }
}

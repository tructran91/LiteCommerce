using LiteCommerce.Admin.Constants;
using LiteCommerce.Admin.Models.Business.Category;
using LiteCommerce.Admin.Models.Common;
using Refit;

namespace LiteCommerce.Admin.ApiClients
{
    public interface ICategoryApi
    {
        [Get(ApiRoutes.Category.GetAll)]
        Task<BaseResponse<List<CategoryResponse>>> GetCategoriesAsync(int currentPage, int pageSize);

        [Get(ApiRoutes.Category.GetAllBasic)]
        Task<BaseResponse<List<BasicCategoryResponse>>> GetBasicCategoriesAsync();

        [Get(ApiRoutes.Category.GetById)]
        Task<BaseResponse<CategoryFormModel>> GetCategoryAsync(string id);

        [Post(ApiRoutes.Category.Create)]
        [Multipart]
        Task<BaseResponse<CategoryResponse>> CreateCategoryAsync(MultipartFormDataContent content);

        [Put(ApiRoutes.Category.Update)]
        [Multipart]
        Task<BaseResponse<CategoryResponse>> UpdateCategoryAsync(MultipartFormDataContent content);

        [Delete(ApiRoutes.Category.Delete)]
        Task<BaseResponse<bool>> DeleteCategoryAsync(string id);
    }
}

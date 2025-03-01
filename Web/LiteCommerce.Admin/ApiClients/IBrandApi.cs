using LiteCommerce.Admin.Models.Business.Brand;
using LiteCommerce.Admin.Models.Common;
using Refit;

namespace LiteCommerce.Admin.ApiClients
{
    public interface IBrandApi
    {
        [Get("/api/admin/brand/GetAllBrands")]
        Task<BaseResponse<List<BrandModel>>> GetBrandsAsync(int currentPage, int pageSize);

        [Get("/api/admin/brand/GetBrandById/{id}")]
        Task<BaseResponse<BrandModel>> GetBrandAsync(string id);

        [Post("/api/admin/brand/CreateBrand")]
        Task<BaseResponse<BrandModel>> CreateBrandAsync([Body] BrandModel brand);

        [Put("/api/admin/brand/UpdateBrand")]
        Task<BaseResponse<BrandModel>> UpdateBrandAsync([Body] BrandModel brand);

        [Delete("/api/admin/brand/DeleteBrand/{id}")]
        Task<BaseResponse<BrandModel>> DeleteBrandAsync(string id);
    }
}

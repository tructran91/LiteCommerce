using LiteCommerce.Admin.Models.Business;
using LiteCommerce.Admin.Models.Common;
using Refit;

namespace LiteCommerce.Admin.ApiClients
{
    public interface IBrandApi
    {
        [Get("/api/admin/brand/GetAllBrands")]
        Task<BaseResponse<List<Brand>>> GetBrandsAsync();

        [Get("/api/admin/brand/GetBrandById/{id}")]
        Task<BaseResponse<Brand>> GetBrandAsync(string id);

        [Post("/api/admin/brand/CreateBrand")]
        Task<BaseResponse<Brand>> CreateBrandAsync([Body] Brand brand);

        [Put("/api/admin/brand/UpdateBrand")]
        Task<BaseResponse<Brand>> UpdateBrandAsync([Body] Brand brand);
    }
}

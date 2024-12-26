using LiteCommerce.Admin.Models;
using Refit;

namespace LiteCommerce.Admin.ApiClients
{
    public interface IBrandApi
    {
        [Get("/api/admin/brand/GetAllBrands")]
        Task<BaseResponse<List<BrandModel>>> GetBrands();
    }
}

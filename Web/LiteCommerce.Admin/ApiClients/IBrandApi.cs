using LiteCommerce.Admin.Models.Business;
using LiteCommerce.Admin.Models.Common;
using Refit;

namespace LiteCommerce.Admin.ApiClients
{
    public interface IBrandApi
    {
        [Get("/api/admin/brand/GetAllBrands")]
        Task<BaseResponse<List<Brand>>> GetBrands();
    }
}

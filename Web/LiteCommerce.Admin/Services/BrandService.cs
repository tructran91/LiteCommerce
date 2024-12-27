using LiteCommerce.Admin.ApiClients;
using LiteCommerce.Admin.Models.Business;

namespace LiteCommerce.Admin.Services
{
    public interface IBrandService
    {
        Task<List<Brand>> GetBrands();
    }

    public class BrandService : IBrandService
    {
        private readonly IBrandApi _brandApi;

        public BrandService(IBrandApi brandApi)
        {
            _brandApi = brandApi;
        }

        public async Task<List<Brand>> GetBrands()
        {
            var response = await _brandApi.GetBrands();
            return response.Data;
        }
    }
}

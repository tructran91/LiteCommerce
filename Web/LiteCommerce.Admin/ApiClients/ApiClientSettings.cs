using Refit;

namespace LiteCommerce.Admin.ApiClients
{
    public static class ApiClientSettings
    {
        // Error responses carry a BaseResponse JSON body: deserialize it instead of throwing ApiException.
        public static RefitSettings Create()
        {
            var settings = new RefitSettings();
            var defaultFactory = new DefaultApiExceptionFactory(settings);

            settings.ExceptionFactory = response =>
            {
                var isJson = response.Content.Headers.ContentType?.MediaType?.Contains("json") == true;
                return isJson ? Task.FromResult<Exception?>(null) : defaultFactory.CreateAsync(response);
            };

            return settings;
        }
    }
}

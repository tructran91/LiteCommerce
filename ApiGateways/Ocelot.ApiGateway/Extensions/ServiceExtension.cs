using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;

namespace Ocelot.ApiGateway.Extensions
{
    public static class ServiceExtension
    {
        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new() { Title = "API Gateway", Version = "v1" });
            });
        }

        public static void ConfigureOcelot(this WebApplicationBuilder builder)
        {
            builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
            builder.Services.AddOcelot(builder.Configuration)
                .AddCacheManager(x =>
                {
                    x.WithDictionaryHandle();
                });
        }
    }
}

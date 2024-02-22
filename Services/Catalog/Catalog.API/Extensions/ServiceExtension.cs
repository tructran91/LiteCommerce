using Catalog.API.Middlewares;
using Catalog.Application;
using Catalog.Application.Behaviors;
using Catalog.Application.Services;
using Catalog.Core.Repositories;
using Catalog.Infrastructure.Data;
using Catalog.Infrastructure.Repositories;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Extensions
{
    public static class ServiceExtension
    {
        public static void ConfigureCorsAllowAny(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });
        }

        public static void ConfigureSwagger(this IServiceCollection services)
        {
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new() { Title = "Catalog API", Version = "v1" });
            });
        }

        public static void AddLayerForApp(this WebApplicationBuilder builder)
        {
            // Add Services
            builder.Services.AddScoped<IMediaService, MediaService>();
            builder.Services.AddScoped<IStorageService, LocalStorageService>();
            builder.Services.AddScoped<IProductService, ProductService>();

            // Add Infrastructure Layers
            builder.Services.AddDbContext<CatalogContext>(c =>
                c.UseSqlServer(builder.Configuration.GetConnectionString("CatalogConnection")));
            builder.Services.AddTransient<ExceptionHandlingMiddleware>();
            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddScoped<IProductRepository, ProductRepository>();

            // Add Web Third parties
            builder.Services.AddAutoMapper(typeof(MappingProfile));
            builder.Services.AddMediatR(typeof(AssemblyReference).Assembly);

            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            builder.Services.AddValidatorsFromAssembly(typeof(AssemblyReference).Assembly);
        }
    }
}

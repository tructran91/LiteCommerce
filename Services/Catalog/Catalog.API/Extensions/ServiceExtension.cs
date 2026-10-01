using Catalog.API.Middlewares;
using Catalog.Application;
using Catalog.Application.Behaviors;
using Catalog.Application.Services;
using Catalog.Core.Repositories;
using Catalog.Infrastructure.Data;
using Catalog.Infrastructure.Repositories;
using FluentValidation;
using LiteCommerce.Shared.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Reflection;

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
            services.AddOpenApi("v1", options =>
            {
                options.AddDocumentTransformer((document, context, cancellationToken) =>
                {
                    document.Info = new()
                    {
                        Title = "Catalog API",
                        Version = "v1",
                        Description = "LiteCommerce Catalog API for managing products, categories, brands, and attributes",
                    };
                    return Task.CompletedTask;
                });
            });
        }

        // Model-binding errors never reach MediatR; return them as BaseResponse instead of ProblemDetails.
        public static void ConfigureBaseResponseErrors(this IMvcBuilder builder)
        {
            builder.ConfigureApiBehaviorOptions(options =>
            {
                // Leave 404/405/415 bodies empty so UseBaseResponseStatusCodePages writes them.
                options.SuppressMapClientErrors = true;

                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState
                        .Where(x => x.Value?.Errors.Count > 0)
                        .ToDictionary(
                            x => string.Join('.', x.Key.Split('.').Select(s => s.Length == 0 ? s : char.ToLowerInvariant(s[0]) + s[1..])),
                            x => x.Value!.Errors.Select(e => e.ErrorMessage).ToList());

                    var response = BaseResponse<object>.Failure("One or more validation errors occurred", errors);
                    return new JsonResult(response, ErrorJson.Options) { StatusCode = StatusCodes.Status400BadRequest };
                };
            });
        }

        // Framework-generated errors (unknown route, wrong method/media type) get a BaseResponse body too.
        public static void UseBaseResponseStatusCodePages(this WebApplication app)
        {
            app.UseStatusCodePages(async context =>
            {
                var statusCode = context.HttpContext.Response.StatusCode;
                var response = BaseResponse<object>.Failure(
                    ReasonPhrases.GetReasonPhrase(statusCode),
                    statusCode: (HttpStatusCode)statusCode);

                await context.HttpContext.Response.WriteAsJsonAsync(response, ErrorJson.Options);
            });
        }

        public static void RegisterApplicationLayers(this WebApplicationBuilder builder)
        {
            builder.Services.AddApplicationServices(builder.Configuration);
            builder.Services.AddInfrastructureServices(builder.Configuration);
            builder.Services.AddThirdPartyServices(typeof(AssemblyReference).Assembly);
        }

        public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IMediaService, MediaService>();

            var storageProvider = configuration["Storage:Provider"];
            if (storageProvider == "Azure")
            {
                services.AddScoped<IStorageService, AzureBlobStorageService>();
            }
            else
            {
                services.AddScoped<IStorageService, LocalStorageService>();
            }

            services.AddScoped<IProductService, ProductService>();
        }

        public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<CatalogContext>(c =>
                c.UseSqlServer(configuration.GetConnectionString("CatalogConnection")));
            services.AddTransient<ExceptionHandlingMiddleware>();
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        }

        public static void AddThirdPartyServices(this IServiceCollection services, Assembly assembly)
        {
            services.AddAutoMapper(cfg => cfg.AddMaps(assembly));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));
            services.AddValidatorsFromAssembly(assembly);
        }

        public static async Task ApplySeedAsync(this IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var services = scope.ServiceProvider;

            try
            {
                var context = services.GetRequiredService<CatalogContext>();
                await CatalogDataSeed.SeedAsync(context);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during migration and seeding: {ex.Message}");
            }
        }
    }
}

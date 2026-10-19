using FluentValidation;
using Identity.Application.Interfaces;
using Identity.Application.Options;
using Identity.Application.Validators;
using Identity.Core.Entities;
using Identity.Infrastructure.Data;
using Identity.Infrastructure.Data.Interceptors;
using Identity.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // AuditLogInterceptor reads ICurrentUserService, which Identity.API registers.
            services.AddScoped<AuditLogInterceptor>();
            services.AddDbContext<IdentityDbContext>((serviceProvider, options) =>
                options.UseSqlServer(configuration.GetConnectionString("IdentityConnection"))
                       .AddInterceptors(serviceProvider.GetRequiredService<AuditLogInterceptor>()));

            services.Configure<JwtOptions>(configuration.GetSection("Jwt"));

            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
            // Singleton: loads the PEM and builds the RSA key once, not on every request.
            services.AddSingleton<ITokenService, JwtTokenService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IActivityLogger, ActivityLogger>();
            services.AddScoped<ILogQueryService, LogQueryService>();

            services.AddValidatorsFromAssemblyContaining<RegisterRequestValidator>();

            return services;
        }
    }
}

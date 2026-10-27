using Catalog.API.Settings;
using LiteCommerce.Shared.Authorization;
using LiteCommerce.Shared.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Net;

namespace Catalog.API.Extensions
{
    public static class AuthenticationExtension
    {
        public static void ConfigureJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var identity = configuration.GetSection(IdentitySettings.SectionName).Get<IdentitySettings>()
                ?? throw new InvalidOperationException(
                    $"Missing '{IdentitySettings.SectionName}' configuration section. Catalog cannot validate access tokens.");

            if (string.IsNullOrWhiteSpace(identity.Authority))
                throw new InvalidOperationException($"'{IdentitySettings.SectionName}:Authority' is required.");
            services.Configure<IdentitySettings>(configuration.GetSection(IdentitySettings.SectionName));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = identity.Authority;
                    options.RequireHttpsMetadata = identity.RequireHttpsMetadata;

                    // Keep claim names as they are in the token; the default map rewrites
                    // sub/email/role into the long WS-Federation URIs.
                    options.MapInboundClaims = false;

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        // Identity's issuer is a logical name, not the authority URL, so it must be stated here.
                        ValidIssuer = identity.Issuer,
                        ValidAudience = identity.Audience,
                        RoleClaimType = AppClaimTypes.Role,
                        NameClaimType = AppClaimTypes.Email,
                        // Identity issues with ClockSkew = 0; mirror it so a token expires at the same instant on both sides.
                        ClockSkew = TimeSpan.Zero
                    };

                    // Every response body in this API is a BaseResponse, including 401/403.
                    options.Events = new JwtBearerEvents
                    {
                        // The challenge only carries a short error description, which hides
                        // why a token was rejected. Log the real exception and the key ids
                        // that were actually available.
                        OnAuthenticationFailed = context =>
                        {
                            var logger = context.HttpContext.RequestServices
                                .GetRequiredService<ILoggerFactory>()
                                .CreateLogger("Catalog.Authentication");

                            logger.LogError(context.Exception,
                                "JWT validation failed. Authority={Authority}, ValidIssuer={Issuer}, ValidAudience={Audience}",
                                identity.Authority, identity.Issuer, identity.Audience);

                            return Task.CompletedTask;
                        },
                        OnChallenge = async context =>
                        {
                            context.HandleResponse();

                            if (context.Response.HasStarted)
                                return;

                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            context.Response.ContentType = "application/json";

                            var message = string.IsNullOrEmpty(context.ErrorDescription)
                                ? "Authentication is required to access this resource."
                                : context.ErrorDescription;

                            await context.Response.WriteAsJsonAsync(
                                BaseResponse<object>.Failure(message, statusCode: HttpStatusCode.Unauthorized),
                                ErrorJson.Options);
                        },
                        OnForbidden = async context =>
                        {
                            if (context.Response.HasStarted)
                                return;

                            context.Response.StatusCode = StatusCodes.Status403Forbidden;
                            context.Response.ContentType = "application/json";

                            await context.Response.WriteAsJsonAsync(
                                BaseResponse<object>.Failure(
                                    "You do not have permission to perform this action.",
                                    statusCode: HttpStatusCode.Forbidden),
                                ErrorJson.Options);
                        }
                    };
                });
        }

        public static void ConfigureAuthorizationPolicies(this IServiceCollection services)
        {
            services.AddPermissionAuthorization();

            services.AddAuthorization(options =>
            {
                // Secure by default: anything public must opt out with an explicit [AllowAnonymous].
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
            });
        }
    }
}

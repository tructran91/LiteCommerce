using Identity.API.Services;
using Identity.Application.Interfaces;
using Identity.Application.Options;
using LiteCommerce.Shared.Authorization;
using Identity.Infrastructure;
using Identity.Infrastructure.Data;
using Identity.Infrastructure.Security;
using Identity.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.OpenApi;
using Scalar.AspNetCore;
using LiteCommerce.Shared.Models;
using System.Net;
using System.Security.Cryptography;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi("v1", options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Info = new()
        {
            Title = "Identity API",
            Version = "v1",
            Description = "LiteCommerce Identity API for authentication and authorization",
        };

        // Bearer JWT scheme + global requirement: Scalar shows one auth input applied to every request.
        document.Components ??= new();
        document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();
        document.Components.SecuritySchemes["Bearer"] = new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            Description = "Paste the access token from POST /api/auth/login. The 'Bearer ' prefix is added automatically."
        };
        document.Security ??= new List<OpenApiSecurityRequirement>();
        document.Security.Add(new OpenApiSecurityRequirement
        {
            [new OpenApiSecuritySchemeReference("Bearer", document)] = new List<string>()
        });

        return Task.CompletedTask;
    });
});

builder.Services.AddInfrastructureServices(builder.Configuration);

// Who/where for the audit and activity logs (read by AuditLogInterceptor and ActivityLogger).
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

// Behind a TLS-terminating proxy: real scheme for the discovery URLs, real client IP for the
// rate limiter. Only loopback and the listed proxies are trusted.
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;

    foreach (var proxy in builder.Configuration.GetSection("ForwardedHeaders:KnownProxies").Get<string[]>() ?? [])
        options.KnownProxies.Add(System.Net.IPAddress.Parse(proxy));
});

// Same policy as Catalog. Tighten to the real admin origins before production.
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy => policy
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());
});

var jwt = builder.Configuration.GetSection("Jwt").Get<JwtOptions>() ?? new JwtOptions();

// Auto-generate RSA key pair in Development if PEM files are missing
if (builder.Environment.IsDevelopment()
    && !string.IsNullOrWhiteSpace(jwt.PrivateKeyPath)
    && !string.IsNullOrWhiteSpace(jwt.PublicKeyPath)
    && (!File.Exists(jwt.PrivateKeyPath) || !File.Exists(jwt.PublicKeyPath)))
{
    using var rsa = RSA.Create(2048);
    var dir = Path.GetDirectoryName(jwt.PrivateKeyPath);
    if (!string.IsNullOrEmpty(dir))
        Directory.CreateDirectory(dir);

    File.WriteAllText(jwt.PrivateKeyPath, rsa.ExportRSAPrivateKeyPem());
    File.WriteAllText(jwt.PublicKeyPath, rsa.ExportRSAPublicKeyPem());

    Console.WriteLine($"[Identity] Dev RSA key pair generated at {jwt.PrivateKeyPath} and {jwt.PublicKeyPath}");
}

var validationKey = RsaKeyLoader.Load(jwt.PublicKeyPath, "Public");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // Keep claim names as they are in the token; the default map rewrites
        // sub/email/role into the long WS-Federation URIs.
        options.MapInboundClaims = false;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwt.Issuer,
            ValidAudience = jwt.Audience,
            IssuerSigningKey = validationKey,
            RoleClaimType = JwtTokenService.RoleClaimType,
            NameClaimType = JwtRegisteredClaimNames.Email,
            ClockSkew = TimeSpan.Zero
        };
    });
// Same AdminOnly policy as Catalog; the log endpoints use it.
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(AppPolicies.AdminOnly, policy => policy
        .RequireAuthenticatedUser()
        .RequireRole(AppRoles.Admin));
});

// Per-IP throttle on the auth endpoints (the controller opts in with [EnableRateLimiting("auth")]).
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.AddPolicy("auth", httpContext => RateLimitPartition.GetFixedWindowLimiter(
        httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
        _ => new FixedWindowRateLimiterOptions
        {
            PermitLimit = 20,
            Window = TimeSpan.FromMinutes(1),
            QueueLimit = 0
        }));
    options.OnRejected = async (context, cancellationToken) =>
    {
        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
        await context.HttpContext.Response.WriteAsJsonAsync(
            BaseResponse<object>.Failure("Too many requests. Please try again later.", statusCode: HttpStatusCode.TooManyRequests),
            cancellationToken);
    };
});

var app = builder.Build();

app.UseForwardedHeaders();

// Scalar/OpenAPI docs: on for Development, or whenever OpenApi:Enabled is true (e.g. IIS hosting).
//if (app.Environment.IsDevelopment())
//{
//    app.MapOpenApi();
//    app.MapScalarApiReference();
//}
app.MapOpenApi();
app.MapScalarApiReference();

app.UseCors("CorsPolicy");

app.UseRateLimiter();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await IdentityDbInitializer.SeedAdminAsync(app.Services, app.Configuration);

app.Run();

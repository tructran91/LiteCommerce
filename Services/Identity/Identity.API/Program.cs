using Identity.Application.Options;
using Identity.Infrastructure;
using Identity.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
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

var jwt = builder.Configuration.GetSection("Jwt").Get<JwtOptions>() ?? new JwtOptions();

if (string.IsNullOrWhiteSpace(jwt.PublicKeyPath) || !File.Exists(jwt.PublicKeyPath))
    throw new InvalidOperationException(
        $"RSA public key not found at '{jwt.PublicKeyPath}'. Check Jwt:PublicKeyPath configuration.");

var rsa = RSA.Create();
rsa.ImportFromPem(File.ReadAllText(jwt.PublicKeyPath));
var validationKey = new RsaSecurityKey(rsa) { KeyId = jwt.KeyId };

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwt.Issuer,
            ValidAudience = jwt.Audience,
            IssuerSigningKey = validationKey,
            ClockSkew = TimeSpan.Zero
        };
    });
builder.Services.AddAuthorization();

// Per-IP throttle on the auth endpoints (the controller opts in with [EnableRateLimiting("auth")]).
// Behind a reverse proxy, enable ForwardedHeaders so RemoteIpAddress is the client, not the proxy.
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

// Scalar/OpenAPI docs: on for Development, or whenever OpenApi:Enabled is true (e.g. IIS hosting).
//if (app.Environment.IsDevelopment())
//{
//    app.MapOpenApi();
//    app.MapScalarApiReference();
//}
app.MapOpenApi();
app.MapScalarApiReference();

app.UseRateLimiter();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await IdentityDbInitializer.SeedAdminAsync(app.Services, app.Configuration);

app.Run();

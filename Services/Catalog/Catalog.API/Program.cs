using Catalog.API.Extensions;
using Catalog.API.Middlewares;
using LiteCommerce.Shared.Logging;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.RegisterApplicationLayers();
builder.Services.ConfigureSwagger();
builder.Services.ConfigureCorsAllowAny();
builder.Host.UseSerilog(Logging.ConfigureLogger);

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapOpenApi();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/openapi/v1.json", "Catalog API v1");
});

app.UseCors("CorsPolicy");
app.UseMiddleware<ExceptionHandlingMiddleware>();
//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.Services.ApplySeedAsync();

app.Run();

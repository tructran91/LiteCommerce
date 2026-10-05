using Catalog.API.Extensions;
using Catalog.API.Middlewares;
using LiteCommerce.Shared.Logging;
using Scalar.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.RegisterApplicationLayers();
builder.Services.ConfigureOpenApi();
builder.Services.ConfigureCorsAllowAny();
builder.Host.UseSerilog(Logging.ConfigureLogger);

builder.Services.AddControllers().ConfigureBaseResponseErrors();

var app = builder.Build();

//if (app.Environment.IsDevelopment())
//{
//    app.MapOpenApi();
//    app.MapScalarApiReference();
//}
app.MapOpenApi();
app.MapScalarApiReference();

app.UseCors("CorsPolicy");
app.UseBaseResponseStatusCodePages();
app.UseMiddleware<ExceptionHandlingMiddleware>();
//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.Services.ApplySeedAsync(app.Configuration);

app.Run();

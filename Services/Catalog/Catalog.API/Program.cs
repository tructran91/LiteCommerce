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
app.UseSwagger();
app.UseSwaggerUI();
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseCors("CorsPolicy");
app.UseMiddleware<ExceptionHandlingMiddleware>();
//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.Services.ApplySeedAsync();

app.Run();

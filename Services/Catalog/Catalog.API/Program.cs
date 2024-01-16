using Catalog.API.Extensions;
using Catalog.API.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.AddLayerForApp();
builder.Services.ConfigureSwagger();
builder.Services.ConfigureCorsAllowAny();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

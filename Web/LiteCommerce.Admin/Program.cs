using Blazored.LocalStorage;
using BlazorTable;
using LiteCommerce.Admin;
using LiteCommerce.Admin.ApiClients;
using LiteCommerce.Admin.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Refit;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#main-wrapper");
builder.RootComponents.Add<HeadOutlet>("head::after");

var baseUrl = builder.Configuration["ApiSettings:BaseUrl"];
if (string.IsNullOrEmpty(baseUrl))
{
    baseUrl = builder.HostEnvironment.BaseAddress;
}

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(baseUrl) });
builder.Services.AddRefitClient<IBrandApi>().ConfigureHttpClient(c => c.BaseAddress = new Uri(baseUrl));

// Register for internal service
builder.Services.AddScoped<IMenuService, MenuService>();
builder.Services.AddScoped<IBrandService, BrandService>();

// Register for app
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddBlazorTable();

await builder.Build().RunAsync();

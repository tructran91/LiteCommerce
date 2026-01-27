using Blazored.LocalStorage;
using Blazored.Toast;
using BlazorTable;
using CurrieTechnologies.Razor.SweetAlert2;
using LiteCommerce.Admin;
using LiteCommerce.Admin.ApiClients;
using LiteCommerce.Admin.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Refit;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#main-wrapper");
builder.RootComponents.Add<HeadOutlet>("head::after");

var catalogUrl = builder.Configuration["ApiSettings:CatalogUrl"];
if (string.IsNullOrEmpty(catalogUrl))
{
    catalogUrl = builder.HostEnvironment.BaseAddress;
}

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(catalogUrl) });
builder.Services.AddRefitClient<IBrandApi>().ConfigureHttpClient(c => c.BaseAddress = new Uri(catalogUrl));
builder.Services.AddRefitClient<ICategoryApi>().ConfigureHttpClient(c => c.BaseAddress = new Uri(catalogUrl));
builder.Services.AddRefitClient<IProductOptionApi>().ConfigureHttpClient(c => c.BaseAddress = new Uri(catalogUrl));
builder.Services.AddRefitClient<IProductAttributeGroupApi>().ConfigureHttpClient(c => c.BaseAddress = new Uri(catalogUrl));
builder.Services.AddRefitClient<IProductAttributeApi>().ConfigureHttpClient(c => c.BaseAddress = new Uri(catalogUrl));

// Register for internal service
builder.Services.AddScoped<IMenuService, MenuService>();

// Register for app
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddBlazoredToast();
builder.Services.AddBlazorTable();
builder.Services.AddSweetAlert2();

await builder.Build().RunAsync();

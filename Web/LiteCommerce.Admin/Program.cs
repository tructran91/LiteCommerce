using LiteCommerce.Admin.ApiClients;
using LiteCommerce.Admin.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor;
using MudBlazor.Services;
using Refit;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<LiteCommerce.Admin.App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// MudBlazor
builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;
    config.SnackbarConfiguration.PreventDuplicates = false;
    config.SnackbarConfiguration.NewestOnTop = false;
    config.SnackbarConfiguration.ShowCloseIcon = true;
    config.SnackbarConfiguration.VisibleStateDuration = 4000;
    config.SnackbarConfiguration.HideTransitionDuration = 300;
    config.SnackbarConfiguration.ShowTransitionDuration = 300;
    config.SnackbarConfiguration.SnackbarVariant = MudBlazor.Variant.Filled;
});

// API Clients
var catalogUrl = builder.Configuration["ApiSettings:CatalogUrl"]
    ?? builder.HostEnvironment.BaseAddress;

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(catalogUrl) });
builder.Services.AddRefitClient<IBrandApi>().ConfigureHttpClient(c => c.BaseAddress = new Uri(catalogUrl));
builder.Services.AddRefitClient<ICategoryApi>().ConfigureHttpClient(c => c.BaseAddress = new Uri(catalogUrl));
builder.Services.AddRefitClient<IProductOptionApi>().ConfigureHttpClient(c => c.BaseAddress = new Uri(catalogUrl));
builder.Services.AddRefitClient<IProductAttributeGroupApi>().ConfigureHttpClient(c => c.BaseAddress = new Uri(catalogUrl));
builder.Services.AddRefitClient<IProductAttributeApi>().ConfigureHttpClient(c => c.BaseAddress = new Uri(catalogUrl));
builder.Services.AddRefitClient<IProductTemplateApi>().ConfigureHttpClient(c => c.BaseAddress = new Uri(catalogUrl));
builder.Services.AddRefitClient<IProductApi>().ConfigureHttpClient(c => c.BaseAddress = new Uri(catalogUrl));
builder.Services.AddRefitClient<IProductPriceApi>().ConfigureHttpClient(c => c.BaseAddress = new Uri(catalogUrl));

// System Service
builder.Services.AddScoped<AppSettingsService>();
builder.Services.AddScoped<MenuService>();

await builder.Build().RunAsync();

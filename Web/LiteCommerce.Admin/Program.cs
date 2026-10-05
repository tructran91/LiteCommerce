using LiteCommerce.Admin.ApiClients;
using LiteCommerce.Admin.Models.Common;
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
var refitSettings = ApiClientSettings.Create();
builder.Services.AddRefitClient<IBrandApi>(refitSettings).ConfigureHttpClient(c => c.BaseAddress = new Uri(catalogUrl));
builder.Services.AddRefitClient<ICategoryApi>(refitSettings).ConfigureHttpClient(c => c.BaseAddress = new Uri(catalogUrl));
builder.Services.AddRefitClient<IProductOptionApi>(refitSettings).ConfigureHttpClient(c => c.BaseAddress = new Uri(catalogUrl));
builder.Services.AddRefitClient<IProductAttributeGroupApi>(refitSettings).ConfigureHttpClient(c => c.BaseAddress = new Uri(catalogUrl));
builder.Services.AddRefitClient<IProductAttributeApi>(refitSettings).ConfigureHttpClient(c => c.BaseAddress = new Uri(catalogUrl));
builder.Services.AddRefitClient<IProductTemplateApi>(refitSettings).ConfigureHttpClient(c => c.BaseAddress = new Uri(catalogUrl));
builder.Services.AddRefitClient<IProductApi>(refitSettings).ConfigureHttpClient(c => c.BaseAddress = new Uri(catalogUrl));
builder.Services.AddRefitClient<IProductPriceApi>(refitSettings).ConfigureHttpClient(c => c.BaseAddress = new Uri(catalogUrl));
builder.Services.AddRefitClient<IAuditLogApi>(refitSettings).ConfigureHttpClient(c => c.BaseAddress = new Uri(catalogUrl));
builder.Services.AddRefitClient<IActivityLogApi>(refitSettings).ConfigureHttpClient(c => c.BaseAddress = new Uri(catalogUrl));

// System Service
var fileUploadSettings = new FileUploadSettings();
builder.Configuration.GetSection(FileUploadSettings.SectionName).Bind(fileUploadSettings);
builder.Services.AddSingleton(fileUploadSettings);
builder.Services.AddScoped<AppSettingsService>();
builder.Services.AddScoped<MenuService>();

await builder.Build().RunAsync();

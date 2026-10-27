using LiteCommerce.Admin.ApiClients;
using LiteCommerce.Admin.Handlers;
using LiteCommerce.Admin.Models.Common;
using LiteCommerce.Admin.Pages.Base;
using LiteCommerce.Admin.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using MudBlazor;
using MudBlazor.Services;
using Refit;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<LiteCommerce.Admin.App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Localization
builder.Services.AddLocalization();

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

// Translates MudBlazor's own component strings through our resx files.
builder.Services.AddTransient<MudLocalizer, AppMudLocalizer>();

// Auth
var identityUrl = builder.Configuration["ApiSettings:IdentityUrl"]
    ?? builder.HostEnvironment.BaseAddress;

builder.Services.AddAuthorizationCore();
// Singletons: IHttpClientFactory gives each handler chain its own DI scope, so scoped
// registrations would give AuthHeaderHandler a different token cache than the UI.
builder.Services.AddSingleton<TokenStorageService>();
builder.Services.AddSingleton<JwtAuthenticationStateProvider>();
builder.Services.AddSingleton<AuthenticationStateProvider>(sp => sp.GetRequiredService<JwtAuthenticationStateProvider>());
builder.Services.AddSingleton<AuthService>();
builder.Services.AddSingleton<PermissionService>();
builder.Services.AddTransient<AuthHeaderHandler>();

// API Clients
var catalogUrl = builder.Configuration["ApiSettings:CatalogUrl"]
    ?? builder.HostEnvironment.BaseAddress;

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(catalogUrl) });
var refitSettings = ApiClientSettings.Create();

// No AuthHeaderHandler on these two, or AuthService would depend on clients that depend
// on AuthService. The Identity endpoints are anonymous and the session check takes its
// token as an argument, so neither needs it.
builder.Services.AddRefitClient<IIdentityApi>(refitSettings).ConfigureHttpClient(c => c.BaseAddress = new Uri(identityUrl));
builder.Services.AddRefitClient<ISessionApi>(refitSettings).ConfigureHttpClient(c => c.BaseAddress = new Uri(catalogUrl));

builder.Services.AddRefitClient<IBrandApi>(refitSettings).ConfigureHttpClient(c => c.BaseAddress = new Uri(catalogUrl)).AddHttpMessageHandler<AuthHeaderHandler>();
builder.Services.AddRefitClient<ICategoryApi>(refitSettings).ConfigureHttpClient(c => c.BaseAddress = new Uri(catalogUrl)).AddHttpMessageHandler<AuthHeaderHandler>();
builder.Services.AddRefitClient<IProductOptionApi>(refitSettings).ConfigureHttpClient(c => c.BaseAddress = new Uri(catalogUrl)).AddHttpMessageHandler<AuthHeaderHandler>();
builder.Services.AddRefitClient<IProductAttributeGroupApi>(refitSettings).ConfigureHttpClient(c => c.BaseAddress = new Uri(catalogUrl)).AddHttpMessageHandler<AuthHeaderHandler>();
builder.Services.AddRefitClient<IProductAttributeApi>(refitSettings).ConfigureHttpClient(c => c.BaseAddress = new Uri(catalogUrl)).AddHttpMessageHandler<AuthHeaderHandler>();
builder.Services.AddRefitClient<IProductTemplateApi>(refitSettings).ConfigureHttpClient(c => c.BaseAddress = new Uri(catalogUrl)).AddHttpMessageHandler<AuthHeaderHandler>();
builder.Services.AddRefitClient<IProductApi>(refitSettings).ConfigureHttpClient(c => c.BaseAddress = new Uri(catalogUrl)).AddHttpMessageHandler<AuthHeaderHandler>();
builder.Services.AddRefitClient<IProductPriceApi>(refitSettings).ConfigureHttpClient(c => c.BaseAddress = new Uri(catalogUrl)).AddHttpMessageHandler<AuthHeaderHandler>();
builder.Services.AddRefitClient<IAuditLogApi>(refitSettings).ConfigureHttpClient(c => c.BaseAddress = new Uri(catalogUrl)).AddHttpMessageHandler<AuthHeaderHandler>();
builder.Services.AddRefitClient<IActivityLogApi>(refitSettings).ConfigureHttpClient(c => c.BaseAddress = new Uri(catalogUrl)).AddHttpMessageHandler<AuthHeaderHandler>();

// Identity's admin log endpoints (AdminOnly). They need the token, and AuthService does not depend on them, so no cycle.
builder.Services.AddRefitClient<IIdentityAuditLogApi>(refitSettings).ConfigureHttpClient(c => c.BaseAddress = new Uri(identityUrl)).AddHttpMessageHandler<AuthHeaderHandler>();
builder.Services.AddRefitClient<IIdentityActivityLogApi>(refitSettings).ConfigureHttpClient(c => c.BaseAddress = new Uri(identityUrl)).AddHttpMessageHandler<AuthHeaderHandler>();
builder.Services.AddRefitClient<IAdminUserApi>(refitSettings).ConfigureHttpClient(c => c.BaseAddress = new Uri(identityUrl)).AddHttpMessageHandler<AuthHeaderHandler>();
builder.Services.AddRefitClient<IAdminRoleApi>(refitSettings).ConfigureHttpClient(c => c.BaseAddress = new Uri(identityUrl)).AddHttpMessageHandler<AuthHeaderHandler>();
builder.Services.AddRefitClient<IProfileApi>(refitSettings).ConfigureHttpClient(c => c.BaseAddress = new Uri(identityUrl)).AddHttpMessageHandler<AuthHeaderHandler>();
builder.Services.AddRefitClient<IPasswordRecoveryApi>(refitSettings).ConfigureHttpClient(c => c.BaseAddress = new Uri(identityUrl));

// System Service
var fileUploadSettings = new FileUploadSettings();
builder.Configuration.GetSection(FileUploadSettings.SectionName).Bind(fileUploadSettings);
builder.Services.AddSingleton(fileUploadSettings);
builder.Services.AddScoped<AppSettingsService>();
builder.Services.AddScoped<LocalizationService>();
builder.Services.AddScoped<MenuService>();
builder.Services.AddScoped<FileDownloadService>();
builder.Services.AddScoped<DeleteOperationHelper>();

var host = builder.Build();

// Culture must be set before the first render, or the app boots in English.
// localStorage is read here (not through AppSettingsService) because that service
// is created per DI scope, after the host has already started.
try
{
    var jsRuntime = host.Services.GetRequiredService<IJSRuntime>();
    var savedSettings = await jsRuntime.InvokeAsync<string?>("localStorage.getItem", "app-settings");
    LocalizationService.Apply(LocalizationService.ReadSavedCulture(savedSettings));
}
catch
{
    LocalizationService.Apply(LocalizationService.DefaultCulture);
}

await host.RunAsync();

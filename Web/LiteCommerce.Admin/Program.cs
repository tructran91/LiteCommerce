using Blazored.LocalStorage;
using LiteCommerce.Admin;
using LiteCommerce.Admin.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#main-wrapper");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Register for internal service
builder.Services.AddScoped<IMenuService, MenuService>();

// Register for app
builder.Services.AddBlazoredLocalStorage();

await builder.Build().RunAsync();

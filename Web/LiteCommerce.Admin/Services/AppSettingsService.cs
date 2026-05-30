using LiteCommerce.Admin.Models.Application;
using Microsoft.JSInterop;
using System.Text.Json;

namespace LiteCommerce.Admin.Services
{
    public class AppSettingsService
    {
        private readonly IJSRuntime _js;
        private AppSettings _settings = new();

        public event Action? OnSettingsChanged;

        public AppSettings Settings => _settings;

        public AppSettingsService(IJSRuntime js)
        {
            _js = js;
        }

        public async Task LoadAsync()
        {
            try
            {
                var json = await _js.InvokeAsync<string>("localStorage.getItem", "app-settings");
                if (!string.IsNullOrEmpty(json))
                {
                    _settings = JsonSerializer.Deserialize<AppSettings>(json) ?? new AppSettings();
                }
            }
            catch { /* ignore on prerender */ }
        }

        public async Task SaveAsync()
        {
            var json = JsonSerializer.Serialize(_settings);
            await _js.InvokeVoidAsync("localStorage.setItem", "app-settings", json);
            OnSettingsChanged?.Invoke();
        }

        public async Task UpdateSettingAsync(Action<AppSettings> update)
        {
            update(_settings);
            await SaveAsync();
        }

        public string GetMudThemePrimaryColor() => _settings.PrimaryColor;
        public string GetMudThemeSecondaryColor() => _settings.SecondaryColor;
    }
}

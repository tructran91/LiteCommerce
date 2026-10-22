using LiteCommerce.Admin.Models.Application;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using System.Globalization;
using System.Text.Json;

namespace LiteCommerce.Admin.Services
{
    public class LocalizationService
    {
        public const string DefaultCulture = "en-US";

        public static readonly IReadOnlyList<CultureOption> SupportedCultures = new List<CultureOption>
        {
            new("vi-VN", "Tiếng Việt"),
            new("en-US", "English")
        };

        private readonly AppSettingsService _settingsService;
        private readonly IJSRuntime _js;
        private readonly NavigationManager _navigation;
        private readonly IStringLocalizer<Resources.SharedResource> _localizer;

        public LocalizationService(
            AppSettingsService settingsService,
            IJSRuntime js,
            NavigationManager navigation,
            IStringLocalizer<Resources.SharedResource> localizer)
        {
            _settingsService = settingsService;
            _js = js;
            _navigation = navigation;
            _localizer = localizer;
        }

        public string CurrentCulture => Normalize(CultureInfo.CurrentUICulture.Name);

        public string this[string key] => _localizer[key];

        public static string Normalize(string? culture)
        {
            if (string.IsNullOrWhiteSpace(culture))
                return DefaultCulture;

            foreach (var option in SupportedCultures)
            {
                if (string.Equals(option.Code, culture, StringComparison.OrdinalIgnoreCase))
                    return option.Code;
            }

            var language = culture.Split('-')[0];
            foreach (var option in SupportedCultures)
            {
                if (string.Equals(option.Code.Split('-')[0], language, StringComparison.OrdinalIgnoreCase))
                    return option.Code;
            }

            return DefaultCulture;
        }

        public static void Apply(string? culture)
        {
            var cultureInfo = CultureInfo.GetCultureInfo(Normalize(culture));
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
        }

        public static string ReadSavedCulture(string? appSettingsJson)
        {
            if (string.IsNullOrWhiteSpace(appSettingsJson))
                return DefaultCulture;

            try
            {
                var settings = JsonSerializer.Deserialize<AppSettings>(appSettingsJson);
                return Normalize(settings?.Language);
            }
            catch
            {
                return DefaultCulture;
            }
        }

        public async Task ChangeCultureAsync(string culture)
        {
            var normalized = Normalize(culture);

            await _settingsService.UpdateSettingAsync(s => s.Language = normalized);

            Apply(normalized);

            try
            {
                await _js.InvokeVoidAsync("document.documentElement.setAttribute", "lang", normalized);
            }
            catch
            {
                // JS interop is best effort; the reload below re-applies everything anyway.
            }

            _navigation.NavigateTo(_navigation.Uri, forceLoad: true);
        }
    }

    public record CultureOption(string Code, string Name);
}

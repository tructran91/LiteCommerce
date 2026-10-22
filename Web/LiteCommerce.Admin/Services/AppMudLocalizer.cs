using LiteCommerce.Admin.Resources;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace LiteCommerce.Admin.Services
{
    public class AppMudLocalizer : MudLocalizer
    {
        private readonly IStringLocalizer<SharedResource> _localizer;

        public AppMudLocalizer(IStringLocalizer<SharedResource> localizer)
        {
            _localizer = localizer;
        }

        public override LocalizedString this[string key] => _localizer[key];

        public override LocalizedString this[string key, params object[] arguments] => _localizer[key, arguments];
    }
}

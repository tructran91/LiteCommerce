using LiteCommerce.Admin.Models.Business.Auth;
using Microsoft.JSInterop;
using System.Text.Json;

namespace LiteCommerce.Admin.Services
{
    // Same localStorage approach as AppSettingsService, so no extra package is needed.
    // Tokens are cached in memory because AuthHeaderHandler reads them on every request.
    public class TokenStorageService
    {
        private const string StorageKey = "auth-tokens";

        private readonly IJSRuntime _js;
        private AuthTokens? _cached;
        private bool _loaded;

        // JwtAuthenticationStateProvider listens to this to follow every login, refresh and logout.
        public event Action? TokensChanged;

        public TokenStorageService(IJSRuntime js)
        {
            _js = js;
        }

        public async Task<AuthTokens?> GetAsync()
        {
            if (_loaded)
                return _cached;

            try
            {
                var json = await _js.InvokeAsync<string?>("localStorage.getItem", StorageKey);
                _cached = string.IsNullOrEmpty(json) ? null : JsonSerializer.Deserialize<AuthTokens>(json);
            }
            catch
            {
                _cached = null;
            }

            _loaded = true;
            return _cached;
        }

        public async Task SetAsync(AuthTokens tokens)
        {
            _cached = tokens;
            _loaded = true;
            await _js.InvokeVoidAsync("localStorage.setItem", StorageKey, JsonSerializer.Serialize(tokens));
            TokensChanged?.Invoke();
        }

        public async Task ClearAsync()
        {
            _cached = null;
            _loaded = true;
            await _js.InvokeVoidAsync("localStorage.removeItem", StorageKey);
            TokensChanged?.Invoke();
        }
    }
}

using LiteCommerce.Admin.ApiClients;
using LiteCommerce.Admin.Constants;
using LiteCommerce.Admin.Models.Business.Auth;
using Microsoft.AspNetCore.Components.Authorization;

namespace LiteCommerce.Admin.Services
{
    // Errors are resolved to a message here because the caller cannot tell a failed API
    // response from a missing token, and both end up on the same alert.
    public record AuthOutcome(bool IsSuccess, string? ErrorMessage);

    public class AuthService
    {
        // Refresh ahead of expiry so a token does not die while the request is in flight.
        private static readonly TimeSpan ExpirySkew = TimeSpan.FromSeconds(30);

        private readonly IIdentityApi _identityApi;
        private readonly ISessionApi _sessionApi;
        private readonly TokenStorageService _tokenStorage;
        private readonly AuthenticationStateProvider _authStateProvider;

        // One refresh at a time: parallel rotations would make Identity see a reused
        // refresh token, which it treats as theft and revokes every session.
        private readonly SemaphoreSlim _refreshLock = new(1, 1);

        public AuthService(
            IIdentityApi identityApi,
            ISessionApi sessionApi,
            TokenStorageService tokenStorage,
            AuthenticationStateProvider authStateProvider)
        {
            _identityApi = identityApi;
            _sessionApi = sessionApi;
            _tokenStorage = tokenStorage;
            _authStateProvider = authStateProvider;
        }

        public async Task<AuthOutcome> LoginAsync(string email, string password)
        {
            var response = await _identityApi.LoginAsync(new LoginRequest { Email = email, Password = password });

            if (!response.IsSuccess || response.Data is null || string.IsNullOrWhiteSpace(response.Data.AccessToken))
                return new AuthOutcome(false, response.GetErrorMessage(SystemMessages.LoginFailed));

            await _tokenStorage.SetAsync(new AuthTokens
            {
                AccessToken = response.Data.AccessToken,
                RefreshToken = response.Data.RefreshToken
            });

            // Identity authenticates every role, but this app is Admin-only. Asking Catalog
            // instead of checking the role here keeps the rejection message the API's.
            var session = await _sessionApi.GetSessionAsync(response.Data.AccessToken);
            if (!session.IsSuccess)
            {
                await _tokenStorage.ClearAsync();
                return new AuthOutcome(false, session.GetErrorMessage(SystemMessages.LoginFailed));
            }

            NotifyStateChanged();
            return new AuthOutcome(true, null);
        }

        // On success the server has revoked every refresh token, so the local ones are
        // dropped too and the caller is expected to send the user back to the login page.
        public async Task<AuthOutcome> ChangePasswordAsync(ChangePasswordRequest request)
        {
            var token = await GetAccessTokenAsync();

            if (string.IsNullOrWhiteSpace(token))
            {
                await ClearAndNotifyAsync();
                return new AuthOutcome(false, SystemMessages.SessionExpired);
            }

            var response = await _identityApi.ChangePasswordAsync(token, request);

            if (!response.IsSuccess)
                return new AuthOutcome(false, response.GetErrorMessage(SystemMessages.ErrorOccurred));

            await ClearAndNotifyAsync();
            return new AuthOutcome(true, null);
        }

        public async Task LogoutAsync()
        {
            var tokens = await _tokenStorage.GetAsync();

            if (!string.IsNullOrWhiteSpace(tokens?.RefreshToken))
            {
                try
                {
                    await _identityApi.RevokeAsync(new RevokeRequest { RefreshToken = tokens.RefreshToken });
                }
                catch
                {
                    // Best effort; the local token is cleared either way.
                }
            }

            await _tokenStorage.ClearAsync();
            NotifyStateChanged();
        }

        // Null means there is no usable token and the caller must send the request anonymously.
        public async Task<string?> GetAccessTokenAsync()
        {
            var tokens = await _tokenStorage.GetAsync();

            if (tokens is null || string.IsNullOrWhiteSpace(tokens.AccessToken))
                return null;

            if (!JwtParser.IsExpired(tokens.AccessToken, ExpirySkew))
                return tokens.AccessToken;

            return await RefreshAsync();
        }

        public async Task<string?> RefreshAsync()
        {
            await _refreshLock.WaitAsync();
            try
            {
                var tokens = await _tokenStorage.GetAsync();

                if (tokens is null || string.IsNullOrWhiteSpace(tokens.RefreshToken))
                    return null;

                // Another caller may have refreshed while this one waited on the lock.
                if (!JwtParser.IsExpired(tokens.AccessToken, ExpirySkew))
                    return tokens.AccessToken;

                try
                {
                    var response = await _identityApi.RefreshAsync(new RefreshRequest { RefreshToken = tokens.RefreshToken });

                    if (!response.IsSuccess || response.Data is null || string.IsNullOrWhiteSpace(response.Data.AccessToken))
                    {
                        await ClearAndNotifyAsync();
                        return null;
                    }

                    await _tokenStorage.SetAsync(new AuthTokens
                    {
                        AccessToken = response.Data.AccessToken,
                        RefreshToken = response.Data.RefreshToken
                    });

                    NotifyStateChanged();
                    return response.Data.AccessToken;
                }
                catch
                {
                    // Network failure: keep the tokens so a later retry can still succeed.
                    return null;
                }
            }
            finally
            {
                _refreshLock.Release();
            }
        }

        public async Task ClearAndNotifyAsync()
        {
            await _tokenStorage.ClearAsync();
            NotifyStateChanged();
        }

        private void NotifyStateChanged()
        {
            (_authStateProvider as JwtAuthenticationStateProvider)?.NotifyAuthenticationStateChanged();
        }
    }
}

using LiteCommerce.Admin.ApiClients;
using LiteCommerce.Admin.Models.Business.Auth;
using LiteCommerce.Admin.Resources;
using Microsoft.Extensions.Localization;
using System.Net;

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
        private readonly IStringLocalizer<SharedResource> _localizer;

        // One refresh at a time: parallel rotations would make Identity see a reused
        // refresh token, which it treats as theft and revokes every session.
        private readonly SemaphoreSlim _refreshLock = new(1, 1);

        public AuthService(
            IIdentityApi identityApi,
            ISessionApi sessionApi,
            TokenStorageService tokenStorage,
            IStringLocalizer<SharedResource> localizer)
        {
            _identityApi = identityApi;
            _sessionApi = sessionApi;
            _tokenStorage = tokenStorage;
            _localizer = localizer;
        }

        public async Task<AuthOutcome> LoginAsync(string email, string password)
        {
            var response = await _identityApi.LoginAsync(new LoginRequest { Email = email, Password = password });

            if (!response.IsSuccess || response.Data is null || string.IsNullOrWhiteSpace(response.Data.AccessToken))
                return new AuthOutcome(false, response.GetErrorMessage(_localizer["Common_LoginFailed"]));

            // Identity authenticates every role, but this app is Admin-only. Asking Catalog
            // instead of checking the role here keeps the rejection message the API's.
            // Checked before storing, so a failure leaves nothing in localStorage.
            var session = await _sessionApi.GetSessionAsync(response.Data.AccessToken);
            if (!session.IsSuccess)
            {
                await TryRevokeAsync(response.Data.RefreshToken);
                return new AuthOutcome(false, session.GetErrorMessage(_localizer["Common_LoginFailed"]));
            }

            await _tokenStorage.SetAsync(new AuthTokens
            {
                AccessToken = response.Data.AccessToken,
                RefreshToken = response.Data.RefreshToken
            });

            return new AuthOutcome(true, null);
        }

        // On success the server has revoked every refresh token, so the local ones are
        // dropped too and the caller is expected to send the user back to the login page.
        public async Task<AuthOutcome> ChangePasswordAsync(ChangePasswordRequest request)
        {
            var token = await GetAccessTokenAsync();

            if (string.IsNullOrWhiteSpace(token))
            {
                await ClearAsync();
                return new AuthOutcome(false, _localizer["Common_SessionExpired"]);
            }

            var response = await _identityApi.ChangePasswordAsync(token, request);

            if (!response.IsSuccess)
                return new AuthOutcome(false, response.GetErrorMessage(_localizer["Common_ErrorOccurred"]));

            await ClearAsync();
            return new AuthOutcome(true, null);
        }

        public async Task LogoutAsync()
        {
            var tokens = await _tokenStorage.GetAsync();

            await TryRevokeAsync(tokens?.RefreshToken);
            await ClearAsync();
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

        // rejectedToken: a token the API answered 401 to, refreshed even if not yet expired.
        public async Task<string?> RefreshAsync(string? rejectedToken = null)
        {
            await _refreshLock.WaitAsync();
            try
            {
                var tokens = await _tokenStorage.GetAsync();

                if (tokens is null || string.IsNullOrWhiteSpace(tokens.RefreshToken))
                    return null;

                // Another caller may have refreshed while this one waited on the lock.
                if (tokens.AccessToken != rejectedToken && !JwtParser.IsExpired(tokens.AccessToken, ExpirySkew))
                    return tokens.AccessToken;

                try
                {
                    var response = await _identityApi.RefreshAsync(new RefreshRequest { RefreshToken = tokens.RefreshToken });

                    if (response.IsSuccess && response.Data is not null && !string.IsNullOrWhiteSpace(response.Data.AccessToken))
                    {
                        await _tokenStorage.SetAsync(new AuthTokens
                        {
                            AccessToken = response.Data.AccessToken,
                            RefreshToken = response.Data.RefreshToken
                        });

                        return response.Data.AccessToken;
                    }

                    // Only a rejected refresh token ends the session; 429/5xx are transient.
                    if (response.StatusCode is HttpStatusCode.Unauthorized or HttpStatusCode.BadRequest)
                        await ClearAsync();

                    return null;
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

        public Task ClearAsync() => _tokenStorage.ClearAsync();

        // Best effort: a failed revoke must not block logout.
        private async Task TryRevokeAsync(string? refreshToken)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
                return;

            try
            {
                await _identityApi.RevokeAsync(new RevokeRequest { RefreshToken = refreshToken });
            }
            catch
            {
            }
        }
    }
}

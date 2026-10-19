using LiteCommerce.Admin.Services;
using System.Net;
using System.Net.Http.Headers;

namespace LiteCommerce.Admin.Handlers
{
    // Handling 401 here rather than in the pages is deliberate: ApiClientSettings suppresses
    // exceptions for JSON bodies, so a 401 reaches a page as IsSuccess = false and is
    // indistinguishable from a validation failure.
    public class AuthHeaderHandler : DelegatingHandler
    {
        private readonly AuthService _authService;

        public AuthHeaderHandler(AuthService authService)
        {
            _authService = authService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await _authService.GetAccessTokenAsync();

            if (string.IsNullOrWhiteSpace(token))
                return await base.SendAsync(request, cancellationToken);

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode != HttpStatusCode.Unauthorized)
                return response;

            // RefreshAsync decides whether a failure ends the session, so no clear here.
            var refreshed = await _authService.RefreshAsync(token);

            if (string.IsNullOrWhiteSpace(refreshed))
                return response;

            response.Dispose();

            var retry = await CloneAsync(request);
            retry.Headers.Authorization = new AuthenticationHeaderValue("Bearer", refreshed);
            var retryResponse = await base.SendAsync(retry, cancellationToken);

            if (retryResponse.StatusCode == HttpStatusCode.Unauthorized)
                await _authService.ClearAsync();

            return retryResponse;
        }

        // A sent HttpRequestMessage cannot be resent and its content stream is consumed,
        // so the retry needs a fresh message with a buffered body.
        private static async Task<HttpRequestMessage> CloneAsync(HttpRequestMessage request)
        {
            var clone = new HttpRequestMessage(request.Method, request.RequestUri)
            {
                Version = request.Version
            };

            foreach (var header in request.Headers)
                clone.Headers.TryAddWithoutValidation(header.Key, header.Value);

            foreach (var option in request.Options)
                clone.Options.Set(new HttpRequestOptionsKey<object?>(option.Key), option.Value);

            if (request.Content is not null)
            {
                var buffer = await request.Content.ReadAsByteArrayAsync();
                clone.Content = new ByteArrayContent(buffer);

                foreach (var header in request.Content.Headers)
                    clone.Content.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            return clone;
        }
    }
}

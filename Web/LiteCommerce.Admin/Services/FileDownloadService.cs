using Microsoft.JSInterop;

namespace LiteCommerce.Admin.Services
{
    public class FileDownloadService
    {
        private const string DefaultContentType = "application/octet-stream";

        private readonly IJSRuntime _jsRuntime;

        public FileDownloadService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task DownloadAsync(string fileName, byte[] content, string? contentType = null)
        {
            await _jsRuntime.InvokeVoidAsync(
                "liteCommerce.downloadFileFromBase64",
                fileName,
                Convert.ToBase64String(content),
                contentType ?? DefaultContentType);
        }

        public async Task DownloadAsync(HttpResponseMessage response, string fallbackFileName)
        {
            var content = await response.Content.ReadAsByteArrayAsync();
            var fileName = ResolveFileName(response) ?? fallbackFileName;
            var contentType = response.Content.Headers.ContentType?.MediaType ?? DefaultContentType;

            await DownloadAsync(fileName, content, contentType);
        }

        // Content-Disposition may carry the plain name, the RFC 5987 encoded name, or neither.
        public static string? ResolveFileName(HttpResponseMessage response)
        {
            var disposition = response.Content.Headers.ContentDisposition;

            var fileName = disposition?.FileName?.Trim('"');
            if (!string.IsNullOrWhiteSpace(fileName))
            {
                return fileName;
            }

            var encoded = disposition?.FileNameStar;
            if (string.IsNullOrWhiteSpace(encoded))
            {
                return null;
            }

            var separator = encoded.IndexOf("''", StringComparison.Ordinal);
            if (separator >= 0)
            {
                encoded = encoded[(separator + 2)..];
            }

            return Uri.UnescapeDataString(encoded.Trim('"'));
        }
    }
}

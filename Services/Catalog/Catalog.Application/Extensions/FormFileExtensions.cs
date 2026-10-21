using Microsoft.AspNetCore.Http;

namespace Catalog.Application.Extensions
{
    public static class FormFileExtensions
    {
        public static async Task<byte[]> ReadAllBytesAsync(this IFormFile file, CancellationToken cancellationToken = default)
        {
            await using var stream = file.OpenReadStream();
            using var buffer = new MemoryStream();
            await stream.CopyToAsync(buffer, cancellationToken);
            return buffer.ToArray();
        }
    }
}

using Microsoft.Extensions.Configuration;

namespace Catalog.Application.Services
{
    public class LocalStorageService : IStorageService
    {
        private readonly string _storagePath;

        public LocalStorageService(IConfiguration configuration)
        {
            _storagePath = configuration["Storage:LocalPath"];
        }

        public string GetFileUrl(string fileName, string? subFolder = null)
        {
            if (string.IsNullOrEmpty(subFolder))
            {
                return $"/{fileName}";
            }
            return $"/{subFolder}/{fileName}".Replace("\\", "/");
        }

        public async Task SaveFileAsync(Stream mediaBinaryStream, string fileName, string? subFolder = null)
        {
            var targetPath = _storagePath;
            if (!string.IsNullOrEmpty(subFolder))
            {
                targetPath = Path.Combine(_storagePath, subFolder);
            }
            Directory.CreateDirectory(targetPath);

            var filePath = Path.Combine(targetPath, fileName);
            using (var output = new FileStream(filePath, FileMode.Create))
            {
                await mediaBinaryStream.CopyToAsync(output);
            }
        }

        public async Task DeleteFileAsync(string fileName)
        {
            var filePath = Path.Combine(_storagePath, fileName);
            if (File.Exists(filePath))
            {
                await Task.Run(() => File.Delete(filePath));
            }
        }
    }
}

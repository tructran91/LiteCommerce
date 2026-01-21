using Microsoft.Extensions.Configuration;

namespace Catalog.Application.Services
{
    public class LocalStorageService : IStorageService
    {
        private const string _mediaRootFolder = "user-content";
        private readonly string _storagePath;

        public LocalStorageService(IConfiguration configuration)
        {
            var configuredPath = configuration["Storage:LocalPath"];
            _storagePath = Path.Combine(configuredPath, _mediaRootFolder);
        }

        public string GetFileUrl(string fileName)
        {
            return Path.Combine(_storagePath, fileName);
        }

        public async Task SaveFileAsync(Stream mediaBinaryStream, string fileName)
        {
            Directory.CreateDirectory(_storagePath);
            
            var filePath = Path.Combine(_storagePath, fileName);
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

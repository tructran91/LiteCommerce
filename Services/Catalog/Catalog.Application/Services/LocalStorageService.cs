using Microsoft.Extensions.Configuration;

namespace Catalog.Application.Services
{
    public class LocalStorageService : IStorageService
    {
        private readonly string _storagePath;

        public LocalStorageService(IConfiguration configuration)
        {
            _storagePath = configuration["Storage:Local:Path"];
        }

        private string GetFullPath(string? subFolder = null)
        {
            return string.IsNullOrEmpty(subFolder)
                ? _storagePath
                : Path.Combine(_storagePath, subFolder);
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
            var targetPath = GetFullPath(subFolder);
            Directory.CreateDirectory(targetPath);

            var filePath = Path.Combine(targetPath, fileName);
            await using var output = new FileStream(filePath, FileMode.Create);
            await mediaBinaryStream.CopyToAsync(output);
        }

        public Task MoveFileAsync(string fileName, string sourceSubFolder, string destSubFolder)
        {
            var sourcePath = Path.Combine(GetFullPath(sourceSubFolder), fileName);
            var destDir = GetFullPath(destSubFolder);
            Directory.CreateDirectory(destDir);
            var destPath = Path.Combine(destDir, fileName);

            if (File.Exists(sourcePath))
            {
                File.Move(sourcePath, destPath, overwrite: true);
            }

            return Task.CompletedTask;
        }

        public async Task MoveFolderContentAsync(string sourceSubFolder, string destSubFolder)
        {
            var sourceDirPath = GetFullPath(sourceSubFolder);
            if (!Directory.Exists(sourceDirPath))
                return;

            var files = Directory.GetFiles(sourceDirPath);
            foreach (var filePath in files)
            {
                var fileName = Path.GetFileName(filePath);
                await MoveFileAsync(fileName, sourceSubFolder, destSubFolder);
            }

            await DeleteFolderAsync(sourceSubFolder);
        }

        public Task DeleteFileAsync(string fileName)
        {
            var filePath = Path.Combine(GetFullPath(), fileName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            return Task.CompletedTask;
        }

        public Task DeleteFolderAsync(string subFolder)
        {
            var folderPath = GetFullPath(subFolder);
            if (Directory.Exists(folderPath))
            {
                Directory.Delete(folderPath, recursive: true);
            }

            return Task.CompletedTask;
        }
    }
}

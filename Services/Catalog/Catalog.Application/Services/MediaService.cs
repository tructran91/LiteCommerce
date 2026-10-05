using Catalog.Core.Entities;
using Microsoft.AspNetCore.Http;

namespace Catalog.Application.Services
{
    public class MediaService : IMediaService
    {
        private readonly IStorageService _storageService;

        public MediaService(IStorageService storageService)
        {
            _storageService = storageService;
        }

        public string GetMediaUrl(string? fileName, string? subFolder = null)
        {
            return string.IsNullOrEmpty(fileName) ? string.Empty : _storageService.GetFileUrl(fileName, subFolder);
        }

        public string GetMediaUrl(Media? media, string? subFolder = null)
        {
            return media is null ? string.Empty : GetMediaUrl(media.FileName, subFolder);
        }

        public string GetThumbnailUrl(Media? media, string? subFolder = null)
        {
            return GetMediaUrl(media, subFolder);
        }

        public async Task<string> SaveMediaAsync(IFormFile file, string? subFolder = null)
        {
            var ext = Path.GetExtension(file.FileName);
            var fileName = $"{Guid.NewGuid()}{ext}";

            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName, subFolder);

            return fileName;
        }

        public Task MoveContentImagesAsync(string tempFolderPath, string destFolderPath)
        {
            return _storageService.MoveFolderContentAsync(tempFolderPath, destFolderPath);
        }

        public Task DeleteMediaAsync(string fileName, string? subFolder = null)
        {
            return _storageService.DeleteFileAsync(fileName, subFolder);
        }
    }
}

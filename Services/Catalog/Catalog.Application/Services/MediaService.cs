using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Microsoft.AspNetCore.Http;

namespace Catalog.Application.Services
{
    public class MediaService : IMediaService
    {
        private readonly IBaseRepository<Media> _mediaRepository;
        private readonly IStorageService _storageService;

        public MediaService(IBaseRepository<Media> mediaRepository, IStorageService storageService)
        {
            _mediaRepository = mediaRepository;
            _storageService = storageService;
        }

        public string GetMediaUrl(string fileName, string? subFolder = null)
        {
            return string.IsNullOrEmpty(fileName) ? string.Empty : _storageService.GetFileUrl(fileName, subFolder);
        }

        public string GetMediaUrl(Media media, string? subFolder = null)
        {
            if (media == null)
            {
                return GetMediaUrl("no-image.png");
            }

            return GetMediaUrl(media.FileName, subFolder);
        }

        public string GetThumbnailUrl(Media media, string? subFolder = null)
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

        public Task DeleteMediaAsync(string fileName)
        {
            return _storageService.DeleteFileAsync(fileName);
        }

        public Task DeleteMediaAsync(Media media)
        {
            _mediaRepository.DeleteAsync(media);
            return DeleteMediaAsync(media.FileName);
        }
    }
}

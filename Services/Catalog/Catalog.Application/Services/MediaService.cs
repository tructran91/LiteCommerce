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

        public string GetMediaUrl(Media media)
        {
            if (media == null)
            {
                return GetMediaUrl("no-image.png");
            }

            return GetMediaUrl(media.FileName);
        }

        public string GetMediaUrl(string fileName)
        {
            return string.IsNullOrEmpty(fileName) ? string.Empty : _storageService.GetFileUrl(fileName);
        }

        public string GetThumbnailUrl(Media media)
        {
            return GetMediaUrl(media);
        }

        public async Task<string> SaveMediaAsync(IFormFile file)
        {
            var ext = Path.GetExtension(file.FileName);
            var fileName = $"{Guid.NewGuid()}{ext}";

            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);

            return fileName;
        }

        public Task DeleteMediaAsync(Media media)
        {
            _mediaRepository.DeleteAsync(media);
            return DeleteMediaAsync(media.FileName);
        }

        public Task DeleteMediaAsync(string fileName)
        {
            return _storageService.DeleteFileAsync(fileName);
        }
    }
}

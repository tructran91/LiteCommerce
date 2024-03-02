using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Microsoft.AspNetCore.Http;

namespace Catalog.Application.Services
{
    public class MediaService : IMediaService
    {
        private readonly IRepository<Media> _mediaRepository;
        private readonly IStorageService _storageService;

        public MediaService(IRepository<Media> mediaRepository, IStorageService storageService)
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
            return _storageService.GetMediaUrl(fileName);
        }

        public string GetThumbnailUrl(Media media)
        {
            return GetMediaUrl(media);
        }

        public async Task<string> SaveMediaAsync(IFormFile file)
        {
            var originalFileName = file.FileName;
            var ext = Path.GetExtension(originalFileName);
            var fileName = $"{Guid.NewGuid()}{ext}";

            await _storageService.SaveMediaAsync(file.OpenReadStream(), fileName);

            return fileName;
        }

        public Task DeleteMediaAsync(Media media)
        {
            _mediaRepository.DeleteAsync(media);
            return DeleteMediaAsync(media.FileName);
        }

        public Task DeleteMediaAsync(string fileName)
        {
            return _storageService.DeleteMediaAsync(fileName);
        }
    }
}

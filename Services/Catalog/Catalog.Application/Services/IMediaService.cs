using Catalog.Core.Entities;
using Microsoft.AspNetCore.Http;

namespace Catalog.Application.Services
{
    public interface IMediaService
    {
        string GetMediaUrl(string fileName, string? subFolder = null);

        string GetMediaUrl(Media media, string? subFolder = null);

        string GetThumbnailUrl(Media media, string? subFolder = null);

        Task<string> SaveMediaAsync(IFormFile file, string? subFolder = null);

        Task MoveContentImagesAsync(string tempFolderPath, string destFolderPath);

        Task DeleteMediaAsync(string fileName);

        Task DeleteMediaAsync(Media media);
    }
}

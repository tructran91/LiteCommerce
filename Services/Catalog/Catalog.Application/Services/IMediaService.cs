using Catalog.Core.Entities;
using Microsoft.AspNetCore.Http;

namespace Catalog.Application.Services
{
    public interface IMediaService
    {
        string GetMediaUrl(Media media);

        string GetMediaUrl(string fileName);

        string GetThumbnailUrl(Media media);

        Task<string> SaveMediaAsync(IFormFile file);

        Task DeleteMediaAsync(Media media);

        Task DeleteMediaAsync(string fileName);
    }
}

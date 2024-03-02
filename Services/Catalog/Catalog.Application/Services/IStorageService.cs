namespace Catalog.Application.Services
{
    public interface IStorageService
    {
        string GetMediaUrl(string fileName);

        Task SaveMediaAsync(Stream mediaBinaryStream, string fileName);

        Task DeleteMediaAsync(string fileName);
    }
}

namespace Catalog.Application.Services
{
    public interface IStorageService
    {
        string GetFileUrl(string fileName, string? subFolder = null);

        Task SaveFileAsync(Stream mediaBinaryStream, string fileName, string? subFolder = null);

        Task DeleteFileAsync(string fileName);
    }
}

namespace Catalog.Application.Services
{
    public interface IStorageService
    {
        string GetFileUrl(string fileName, string? subFolder = null);

        Task SaveFileAsync(Stream mediaBinaryStream, string fileName, string? subFolder = null);

        Task MoveFileAsync(string fileName, string sourceSubFolder, string destSubFolder);

        Task MoveFolderContentAsync(string sourceSubFolder, string destSubFolder);

        Task DeleteFileAsync(string fileName, string? subFolder = null);

        Task DeleteFolderAsync(string subFolder);
    }
}

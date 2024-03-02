namespace Catalog.Application.Services
{
    public class LocalStorageService : IStorageService
    {
        private const string _mediaRootFolder = "user-content";

        public string GetMediaUrl(string fileName)
        {
            return $"/{_mediaRootFolder}/{fileName}";
        }

        public async Task SaveMediaAsync(Stream mediaBinaryStream, string fileName)
        {
            var contentRootPath = Directory.GetCurrentDirectory();
            var filePath = Path.Combine(contentRootPath, _mediaRootFolder, fileName);
            using (var output = new FileStream(filePath, FileMode.Create))
            {
                await mediaBinaryStream.CopyToAsync(output);
            }
        }

        public async Task DeleteMediaAsync(string fileName)
        {
            var contentRootPath = Directory.GetCurrentDirectory();
            var filePath = Path.Combine(contentRootPath, _mediaRootFolder, fileName);
            if (File.Exists(filePath))
            {
                await Task.Run(() => File.Delete(filePath));
            }
        }
    }
}

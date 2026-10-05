namespace Catalog.Application.Settings
{
    public class FileUploadSettings
    {
        public const string SectionName = "FileUpload";

        public int MaxImageSizeMB { get; set; } = 5;

        public int MaxDocumentSizeMB { get; set; } = 10;
    }
}

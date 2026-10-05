namespace LiteCommerce.Admin.Models.Common
{
    public class FileUploadSettings
    {
        public const string SectionName = "FileUpload";

        public int MaxImageSizeMB { get; set; } = 5;

        public int MaxDocumentSizeMB { get; set; } = 10;

        public long MaxImageSize => MaxImageSizeMB * 1024L * 1024L;

        public long MaxDocumentSize => MaxDocumentSizeMB * 1024L * 1024L;

        public string ImageSizeExceededMessage => $"File size must not exceed {MaxImageSizeMB}MB";

        public string DocumentSizeExceededMessage => $"File size must not exceed {MaxDocumentSizeMB}MB";
    }
}

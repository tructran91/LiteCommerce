namespace Catalog.Application.Extensions
{
    public static class GuidExtensions
    {
        public static string ToStoragePath(this Guid id, string folderName)
        {
            return Path.Combine(folderName, id.ToString());
        }
    }
}

namespace LiteCommerce.Admin.Helpers
{
    public static class FileSizeFormatter
    {
        public static string Format(long bytes)
        {
            if (bytes >= 1024 * 1024)
            {
                return $"{bytes / 1024d / 1024d:0.#} MB";
            }

            return $"{bytes / 1024d:0.#} KB";
        }
    }
}

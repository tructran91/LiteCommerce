using Catalog.Core.Enums;

namespace Catalog.Core.Entities
{
    public class Media : BaseEntity
    {
        public string? Caption { get; set; }

        public int FileSize { get; set; }

        public string? FileName { get; set; }

        public MediaType MediaType { get; set; }

        public IList<ProductMedia> Products { get; set; } = new List<ProductMedia>();
    }
}

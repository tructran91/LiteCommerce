using Catalog.Core.Enums;

namespace Catalog.Core.Entities
{
    public class Media : BaseEntity
    {
        public string? Caption { get; set; }

        public long FileSize { get; set; }

        public string? FileName { get; set; }

        public MediaType MediaType { get; set; }
    }
}

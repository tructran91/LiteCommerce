using System.ComponentModel.DataAnnotations;

namespace Catalog.Core.Entities
{
    public class BaseEntity
    {
        [Required]
        public Guid Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public bool IsDeleted { get; set; }
    }
}

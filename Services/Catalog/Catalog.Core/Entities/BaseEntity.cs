using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Catalog.Core.Entities
{
    public abstract class BaseEntity
    {
        [Required]
        [Column(Order = 0)]
        public Guid Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        public bool IsDeleted { get; set; }
    }
}

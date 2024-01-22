using System.ComponentModel.DataAnnotations;

namespace Catalog.Core.Entities
{
    public class BaseEntity : IBaseEntity
    {
        [Required]
        public Guid Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public bool IsDeleted { get; set; }
    }
}

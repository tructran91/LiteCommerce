namespace Catalog.Core.Entities
{
    public class ProductOption : BaseEntity
    {
        public ProductOption()
        {

        }

        public ProductOption(Guid id)
        {
            Id = id;
        }

        public string Name { get; set; }
    }
}

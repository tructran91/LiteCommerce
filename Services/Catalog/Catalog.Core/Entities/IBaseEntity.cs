namespace Catalog.Core.Entities
{
    public interface IBaseEntity
    {
        Guid Id { get; set; }

        bool IsDeleted { get; set; }
    }
}

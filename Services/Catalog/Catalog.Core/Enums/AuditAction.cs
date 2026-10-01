namespace Catalog.Core.Enums
{
    public enum AuditAction
    {
        Created = 1,

        Modified,

        // IsDeleted flipped from false to true; the row is still in the table.
        SoftDeleted,

        // Row removed from the table (only ProductAttributeValue and similar non-BaseEntity rows).
        Deleted,
    }
}

namespace Catalog.Infrastructure.Data.Seeding
{
    public interface ISeedProfile
    {
        string Name { get; }

        Task SeedAsync(CatalogContext context, CancellationToken cancellationToken = default);
    }
}

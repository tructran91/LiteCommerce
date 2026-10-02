namespace Catalog.Application.Services
{
    public record SeedResult(string Profile, int BrandCount, int CategoryCount, int ProductCount);

    public interface IDatabaseSeeder
    {
        IReadOnlyList<string> GetAvailableProfiles();

        Task<bool> HasDataAsync(CancellationToken cancellationToken = default);

        Task<SeedResult> SeedAsync(string profile, CancellationToken cancellationToken = default);
    }
}

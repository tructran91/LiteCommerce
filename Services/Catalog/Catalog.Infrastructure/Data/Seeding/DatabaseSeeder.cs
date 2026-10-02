using Catalog.Application.Services;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Data.Seeding
{
    public class DatabaseSeeder : IDatabaseSeeder
    {
        private readonly CatalogContext _context;
        private readonly IReadOnlyList<ISeedProfile> _profiles;

        public DatabaseSeeder(CatalogContext context, IEnumerable<ISeedProfile> profiles)
        {
            _context = context;
            _profiles = profiles.ToList();
        }

        public IReadOnlyList<string> GetAvailableProfiles()
            => _profiles.Select(p => p.Name).ToList();

        public Task<bool> HasDataAsync(CancellationToken cancellationToken = default)
            => _context.Brands.AnyAsync(cancellationToken);

        public async Task<SeedResult> SeedAsync(string profile, CancellationToken cancellationToken = default)
        {
            var target = _profiles.FirstOrDefault(p => p.Name.Equals(profile, StringComparison.OrdinalIgnoreCase));
            if (target is null)
            {
                throw new ArgumentException($"Unknown seed profile '{profile}'.", nameof(profile));
            }

            // A user transaction must run inside the execution strategy, or a retrying strategy
            // (EnableRetryOnFailure) throws. Each attempt starts from a clean change tracker.
            var strategy = _context.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                _context.ChangeTracker.Clear();

                // Disposing without a commit rolls back. No explicit RollbackAsync(cancellationToken):
                // a cancelled token would throw from the rollback and hide the original error.
                await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

                await ClearAsync(cancellationToken);
                await target.SeedAsync(_context, cancellationToken);

                // Seed rows are not user actions; keep them out of the audit log.
                _context.IsAuditEnabled = false;
                try
                {
                    await _context.SaveChangesAsync(cancellationToken);
                }
                finally
                {
                    _context.IsAuditEnabled = true;
                }

                await transaction.CommitAsync(cancellationToken);
            });

            return new SeedResult(
                target.Name,
                await _context.Brands.CountAsync(cancellationToken),
                await _context.Categories.CountAsync(cancellationToken),
                await _context.Products.CountAsync(cancellationToken));
        }

        private async Task ClearAsync(CancellationToken cancellationToken)
        {
            // Children before parents; ExecuteDelete bypasses the change tracker
            // so the wipe itself writes no audit rows.
            await _context.ProductAttributeValues.IgnoreQueryFilters().ExecuteDeleteAsync(cancellationToken);
            await _context.ProductTemplateProductAttributes.IgnoreQueryFilters().ExecuteDeleteAsync(cancellationToken);
            await _context.ProductCategories.IgnoreQueryFilters().ExecuteDeleteAsync(cancellationToken);
            await _context.ProductMedias.IgnoreQueryFilters().ExecuteDeleteAsync(cancellationToken);
            await _context.ProductLinks.IgnoreQueryFilters().ExecuteDeleteAsync(cancellationToken);
            await _context.ProductPriceHistories.IgnoreQueryFilters().ExecuteDeleteAsync(cancellationToken);
            await _context.ProductOptionValues.IgnoreQueryFilters().ExecuteDeleteAsync(cancellationToken);
            await _context.Products.IgnoreQueryFilters().ExecuteDeleteAsync(cancellationToken);
            // Self-referencing Categories: leaves first, then roots.
            await _context.Categories.IgnoreQueryFilters().Where(c => c.ParentId != null).ExecuteDeleteAsync(cancellationToken);
            await _context.Categories.IgnoreQueryFilters().Where(c => c.ParentId == null).ExecuteDeleteAsync(cancellationToken);
            await _context.Brands.IgnoreQueryFilters().ExecuteDeleteAsync(cancellationToken);
            await _context.ProductAttributes.IgnoreQueryFilters().ExecuteDeleteAsync(cancellationToken);
            await _context.ProductAttributeGroups.IgnoreQueryFilters().ExecuteDeleteAsync(cancellationToken);
            await _context.ProductOptions.IgnoreQueryFilters().ExecuteDeleteAsync(cancellationToken);
            await _context.ProductTemplates.IgnoreQueryFilters().ExecuteDeleteAsync(cancellationToken);
            await _context.Medias.IgnoreQueryFilters().ExecuteDeleteAsync(cancellationToken);
        }
    }
}

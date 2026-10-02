using Catalog.Application.Database;

namespace Catalog.Application.Requests
{
    public record SeedDatabaseRequest
    {
        public string Profile { get; set; } = SeedProfiles.Default;
    }
}

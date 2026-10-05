using Identity.Core.Constants;
using Identity.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Identity.Infrastructure.Data
{
    public static class IdentityDbInitializer
    {
        public static async Task SeedAdminAsync(IServiceProvider services, IConfiguration configuration)
        {
            var logger = services.GetRequiredService<ILoggerFactory>().CreateLogger("Identity.Seed");

            var email = configuration["SeedAdmin:Email"]?.Trim().ToLowerInvariant();
            var password = configuration["SeedAdmin:Password"];
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                logger.LogWarning("Admin seeding skipped: SeedAdmin:Email/Password is not configured.");
                return;
            }

            using var scope = services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();
            var hasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher<User>>();

            if (await db.Users.AnyAsync(x => x.Email == email))
                return;

            var adminRole = await db.Roles.FirstAsync(x => x.Name == IdentityRoles.Admin);
            var admin = new User
            {
                Id = Guid.NewGuid(),
                Email = email,
                FullName = configuration["SeedAdmin:FullName"]?.Trim() ?? "Administrator",
                RoleId = adminRole.Id,
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            };
            admin.PasswordHash = hasher.HashPassword(admin, password);

            db.Users.Add(admin);
            await db.SaveChangesAsync();

            logger.LogInformation("Seeded admin user {Email}", email);
        }
    }
}

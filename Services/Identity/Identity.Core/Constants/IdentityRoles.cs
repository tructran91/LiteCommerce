namespace Identity.Core.Constants
{
    // Names of the seeded roles. Authorization keys on permissions, not on these: they exist
    // only so seeding and registration can find a specific row (RoleConfiguration.HasData,
    // AuthService.RegisterAsync, IdentityDbInitializer).
    public static class IdentityRoles
    {
        public const string Admin = "Admin";

        public const string Staff = "Staff";

        public const string Customer = "Customer";
    }
}

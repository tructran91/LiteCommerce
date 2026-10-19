namespace Identity.Core.Constants
{
    // Keep in sync with LiteCommerce.Shared.Authorization.AppRoles.
    // Identity.Core has no project references on purpose, hence the duplication.
    public static class IdentityRoles
    {
        public const string Admin = "Admin";

        public const string Staff = "Staff";

        public const string Customer = "Customer";
    }
}

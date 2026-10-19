namespace LiteCommerce.Shared.Authorization
{
    // Short JWT claim names as they appear on the wire. Identity writes the role with
    // ClaimTypes.Role, which JwtSecurityTokenHandler serialises down to "role".
    public static class AppClaimTypes
    {
        public const string Subject = "sub";

        public const string Email = "email";

        public const string Role = "role";

        public const string JwtId = "jti";
    }
}

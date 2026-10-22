namespace Catalog.API.Settings
{
    // Catalog never holds the signing key: JwtBearer pulls it from Identity's JWKS endpoint.
    public class IdentitySettings
    {
        public const string SectionName = "Identity";

        // Base URL of the Identity service. JwtBearer appends /.well-known/openid-configuration.
        public string Authority { get; set; } = string.Empty;

        public string Issuer { get; set; } = string.Empty;

        public string Audience { get; set; } = string.Empty;

        public bool RequireHttpsMetadata { get; set; } = true;

    }
}

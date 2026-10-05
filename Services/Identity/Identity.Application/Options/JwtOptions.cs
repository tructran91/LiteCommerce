namespace Identity.Application.Options
{
    public class JwtOptions
    {
        public string Issuer { get; set; } = string.Empty;

        public string Audience { get; set; } = string.Empty;

        // Key id in the JWT header. Bump it whenever keys rotate so verifiers can pick the right key.
        public string KeyId { get; set; } = "identity-v1";

        // PEM file paths. Prod must point at real key files (KeyVault mount / env override),
        // never commit prod keys. RS256: signer needs the private key, verifiers only the public key.
        public string PrivateKeyPath { get; set; } = string.Empty;

        public string PublicKeyPath { get; set; } = string.Empty;

        public int AccessTokenMinutes { get; set; } = 15;

        public int RefreshTokenDays { get; set; } = 7;
    }
}

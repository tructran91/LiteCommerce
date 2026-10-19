using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace Identity.Infrastructure.Security
{
    public static class RsaKeyLoader
    {
        // kid = RFC 7638 thumbprint, so a new key gets a new kid and verifiers refetch the JWKS
        // instead of failing the signature check against a cached key with the same kid.
        public static RsaSecurityKey Load(string path, string description)
        {
            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
                throw new InvalidOperationException(
                    $"RSA {description} key not found at '{path}'. Check Jwt:{description}KeyPath configuration.");

            var rsa = RSA.Create();
            rsa.ImportFromPem(File.ReadAllText(path));

            var key = new RsaSecurityKey(rsa);
            key.KeyId = Base64UrlEncoder.Encode(key.ComputeJwkThumbprint());
            return key;
        }
    }
}

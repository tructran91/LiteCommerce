using System.Security.Claims;
using System.Text.Json;

namespace LiteCommerce.Admin.Services
{
    // Reads the JWT payload for display and routing only. The signature is never checked
    // here: that is the API's job, and a client-side check proves nothing anyway.
    public static class JwtParser
    {
        public const string RoleClaim = "role";
        public const string EmailClaim = "email";
        public const string SubjectClaim = "sub";
        public const string ExpiryClaim = "exp";

        public static IReadOnlyList<Claim> ParseClaims(string accessToken)
        {
            var payload = ReadPayload(accessToken);
            if (payload is null)
                return Array.Empty<Claim>();

            var claims = new List<Claim>();

            foreach (var pair in payload)
            {
                if (pair.Value.ValueKind == JsonValueKind.Array)
                {
                    claims.AddRange(pair.Value.EnumerateArray()
                        .Select(item => new Claim(pair.Key, item.ToString())));
                }
                else
                {
                    claims.Add(new Claim(pair.Key, pair.Value.ToString()));
                }
            }

            return claims;
        }

        public static DateTime? GetExpiry(string accessToken)
        {
            var payload = ReadPayload(accessToken);

            if (payload is null
                || !payload.TryGetValue(ExpiryClaim, out var exp)
                || !exp.TryGetInt64(out var seconds))
            {
                return null;
            }

            return DateTimeOffset.FromUnixTimeSeconds(seconds).UtcDateTime;
        }

        // Treats a token within the skew as already expired. Identity validates with
        // ClockSkew = 0, so there is no grace period on the server side.
        public static bool IsExpired(string accessToken, TimeSpan skew)
        {
            var expiry = GetExpiry(accessToken);
            return expiry is null || expiry.Value - skew <= DateTime.UtcNow;
        }

        private static Dictionary<string, JsonElement>? ReadPayload(string accessToken)
        {
            if (string.IsNullOrWhiteSpace(accessToken))
                return null;

            var parts = accessToken.Split('.');
            if (parts.Length != 3)
                return null;

            try
            {
                return JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(
                    DecodeBase64Url(parts[1]));
            }
            catch
            {
                return null;
            }
        }

        private static byte[] DecodeBase64Url(string value)
        {
            var padded = value.Replace('-', '+').Replace('_', '/');

            padded += (padded.Length % 4) switch
            {
                2 => "==",
                3 => "=",
                _ => string.Empty
            };

            return Convert.FromBase64String(padded);
        }
    }
}

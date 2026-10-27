namespace LiteCommerce.Shared.Authorization
{
    // Shared by the APIs and the Blazor client so a permission check gives the same
    // answer on either side. Linked into the client, so it must stay dependency-free.
    public static class PermissionMatcher
    {
        private const char ClaimSeparator = ',';

        // A granted entry may be "*", a group wildcard ("catalog.product.*"), or an exact code.
        public static bool IsGranted(IEnumerable<string> granted, string required)
        {
            if (string.IsNullOrEmpty(required))
                return false;

            foreach (var entry in granted)
            {
                if (string.IsNullOrEmpty(entry))
                    continue;

                if (entry.Length == 1 && entry[0] == '*')
                    return true;

                if (string.Equals(entry, required, StringComparison.Ordinal))
                    return true;

                if (entry.EndsWith(AppPermissions.WildcardSuffix, StringComparison.Ordinal))
                {
                    // Keep the trailing dot, or "catalog.product.*" would also match
                    // "catalog.product-price.view".
                    var prefix = entry.AsSpan(0, entry.Length - 1);
                    if (required.AsSpan().StartsWith(prefix, StringComparison.Ordinal))
                        return true;
                }
            }

            return false;
        }

        public static string[] Split(string? claimValue)
            => string.IsNullOrWhiteSpace(claimValue)
                ? Array.Empty<string>()
                : claimValue.Split(
                    ClaimSeparator,
                    StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        public static string Join(IEnumerable<string> permissions)
            => string.Join(ClaimSeparator, permissions);

        // Collapses fully granted groups into wildcards to keep the JWT small.
        // Unknown codes are dropped: nothing checks for them, so they cannot grant anything.
        public static List<string> Compress(IEnumerable<string> granted)
        {
            var set = granted
                .Where(AppPermissions.AllCodes.Contains)
                .ToHashSet(StringComparer.Ordinal);

            if (set.Count == 0)
                return new List<string>();

            if (set.Count == AppPermissions.AllCodes.Count)
                return new List<string> { AppPermissions.Wildcard };

            var result = new List<string>();
            var covered = new HashSet<string>(StringComparer.Ordinal);

            foreach (var group in AppPermissions.AllCodes.GroupBy(GroupPrefix, StringComparer.Ordinal))
            {
                // A one-member group gains nothing from a wildcard.
                if (group.Count() < 2 || !group.All(set.Contains))
                    continue;

                result.Add(group.Key + AppPermissions.WildcardSuffix);
                foreach (var code in group)
                    covered.Add(code);
            }

            result.AddRange(set.Where(code => !covered.Contains(code)));
            result.Sort(StringComparer.Ordinal);
            return result;
        }

        // For pre-ticking the permission picker, never for an authorization decision.
        public static List<string> Expand(IEnumerable<string> granted)
        {
            var entries = granted as ICollection<string> ?? granted.ToList();

            return AppPermissions.AllCodes
                .Where(code => IsGranted(entries, code))
                .OrderBy(code => code, StringComparer.Ordinal)
                .ToList();
        }

        private static string GroupPrefix(string code)
        {
            var index = code.LastIndexOf(AppPermissions.Separator);
            return index <= 0 ? code : code[..index];
        }
    }
}

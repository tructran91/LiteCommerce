using Microsoft.AspNetCore.Authorization;

namespace LiteCommerce.Shared.Authorization
{
    // Encodes the permission into the policy name; PermissionPolicyProvider decodes it.
    // Stacking several on one action ANDs them, as ASP.NET does for [Authorize].
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public sealed class HasPermissionAttribute : AuthorizeAttribute
    {
        public const string PolicyPrefix = "perm:";

        public HasPermissionAttribute(string permission)
        {
            if (string.IsNullOrWhiteSpace(permission))
                throw new ArgumentException("Permission code is required.", nameof(permission));

            Permission = permission;
            Policy = PolicyPrefix + permission;
        }

        public string Permission { get; }

        public static bool TryGetPermission(string policyName, out string permission)
        {
            if (!string.IsNullOrEmpty(policyName)
                && policyName.StartsWith(PolicyPrefix, StringComparison.Ordinal)
                && policyName.Length > PolicyPrefix.Length)
            {
                permission = policyName[PolicyPrefix.Length..];
                return true;
            }

            permission = string.Empty;
            return false;
        }
    }
}

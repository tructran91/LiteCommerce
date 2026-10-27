using Microsoft.AspNetCore.Authorization;

namespace LiteCommerce.Shared.Authorization
{
    public sealed class PermissionRequirement : IAuthorizationRequirement
    {
        public PermissionRequirement(string permission)
        {
            if (string.IsNullOrWhiteSpace(permission))
                throw new ArgumentException("Permission code is required.", nameof(permission));

            Permission = permission;
        }

        public string Permission { get; }

        public override string ToString() => $"Permission:{Permission}";
    }
}

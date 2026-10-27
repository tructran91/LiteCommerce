namespace LiteCommerce.Shared.Authorization
{
    public sealed record PermissionDescriptor(
        string Code,
        string Service,
        string GroupName,
        string DisplayName,
        string? Description = null);
}

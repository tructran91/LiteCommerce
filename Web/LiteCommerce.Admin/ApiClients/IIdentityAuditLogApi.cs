namespace LiteCommerce.Admin.ApiClients
{
    // Identity's audit endpoints have the same routes and shape as Catalog's, so this only exists to get
    // a second Refit registration pointed at IdentityUrl (Refit allows one registration per interface).
    public interface IIdentityAuditLogApi : IAuditLogApi
    {
    }
}
